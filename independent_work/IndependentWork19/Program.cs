using IndependentWork19;
using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Timeout;

Console.OutputEncoding = System.Text.Encoding.UTF8;

DemoCustomException();
await Scenario1_RetryWithTimeoutAsync();
Scenario2_CircuitBreaker();
await Scenario3_RetryWithFallbackAsync();

static void Log(string msg) => Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {msg}");

// ======================================================================
// Власний виняток
// ======================================================================
static void DemoCustomException()
{
    Console.WriteLine("===== Власний виняток ProfileUpdateException =====");
    try
    {
        ValidateProfile(userId: 14, email: "yaroslav.example.com");
    }
    catch (ProfileUpdateException ex)
    {
        Console.WriteLine($"Помилка: {ex.Message} (користувач {ex.UserId}, поле '{ex.FieldName}')");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Інша помилка: {ex.Message}");
    }
    finally
    {
        Console.WriteLine("Валідацію завершено (finally).\n");
    }

    // Приклад з innerException
    try
    {
        try { int.Parse("abc"); }
        catch (FormatException inner)
        {
            throw new ProfileUpdateException("Не вдалося прочитати вік користувача.", inner);
        }
    }
    catch (ProfileUpdateException ex)
    {
        Console.WriteLine($"{ex.Message} Причина: {ex.InnerException?.GetType().Name}\n");
    }
}

static void ValidateProfile(int userId, string email)
{
    if (!email.Contains('@'))
        throw new ProfileUpdateException("Email не містить символ '@'.", userId, "Email");
}

// ======================================================================
// Сценарій 1: зовнішній API профілів — Retry + Timeout
// ======================================================================
static async Task Scenario1_RetryWithTimeoutAsync()
{
    Console.WriteLine("===== Сценарій 1: Retry + Timeout (зовнішній API профілів) =====");
    int call = 0;

    // Перший виклик «зависає», другий — мережева помилка, третій — успіх.
    async ValueTask<string> UpdateProfileViaApi(CancellationToken ct)
    {
        call++;
        Log($"Виклик API №{call}");
        if (call == 1) { await Task.Delay(2000, ct); }          // перевищить таймаут
        if (call == 2) throw new HttpRequestException("503 Service Unavailable");
        await Task.Delay(50, ct);
        return "200 OK: профіль оновлено";
    }

    // Перша додана стратегія — зовнішня: Retry обгортає Timeout, тож таймаут діє на КОЖНУ спробу окремо.
    var pipeline = new ResiliencePipelineBuilder()
        .AddRetry(new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder()
                .Handle<HttpRequestException>()
                .Handle<TimeoutRejectedException>(),
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromMilliseconds(200),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            OnRetry = args =>
            {
                Log($"Retry #{args.AttemptNumber + 1} через {args.RetryDelay.TotalMilliseconds:F0} мс. " +
                    $"Причина: {args.Outcome.Exception?.GetType().Name}");
                return default;
            }
        })
        .AddTimeout(TimeSpan.FromMilliseconds(500))
        .Build();

    try
    {
        string result = await pipeline.ExecuteAsync(async ct => await UpdateProfileViaApi(ct));
        Log($"Результат: {result}\n");
    }
    catch (Exception ex)
    {
        Log($"Остаточна невдача: {ex.GetType().Name}: {ex.Message}\n");
    }
}

// ======================================================================
// Сценарій 2: база даних — Circuit Breaker
// ======================================================================
static void Scenario2_CircuitBreaker()
{
    Console.WriteLine("===== Сценарій 2: Circuit Breaker (база даних) =====");
    int dbCalls = 0;

    // БД «лежить» перші 4 реальні звернення, потім відновлюється.
    void SaveProfileToDb()
    {
        dbCalls++;
        Log($"  -> реальне звернення до БД №{dbCalls}");
        if (dbCalls <= 4)
            throw new DatabaseUnavailableException("Не вдалося встановити з'єднання з БД.");
    }

    var pipeline = new ResiliencePipelineBuilder()
        .AddCircuitBreaker(new CircuitBreakerStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<DatabaseUnavailableException>(),
            FailureRatio = 0.5,
            MinimumThroughput = 4,
            SamplingDuration = TimeSpan.FromSeconds(10),
            BreakDuration = TimeSpan.FromSeconds(2),
            OnOpened = args => { Log($"*** Circuit OPEN на {args.BreakDuration.TotalSeconds:F0} с"); return default; },
            OnHalfOpened = args => { Log("*** Circuit HALF-OPEN: пробний запит"); return default; },
            OnClosed = args => { Log("*** Circuit CLOSED: сервіс відновився"); return default; }
        })
        .Build();

    void TryCall(int n)
    {
        Log($"Запит №{n}");
        try
        {
            pipeline.Execute(() => { SaveProfileToDb(); });
            Log("  Успіх: профіль збережено");
        }
        catch (BrokenCircuitException)
        {
            Log("  Відхилено миттєво: коло розімкнене (до БД не звертались)");
        }
        catch (DatabaseUnavailableException ex)
        {
            Log($"  Помилка БД: {ex.Message}");
        }
    }

    for (int i = 1; i <= 7; i++) TryCall(i);

    Log("Чекаємо 2.2 с, поки мине BreakDuration...");
    Thread.Sleep(2200);

    TryCall(8);
    TryCall(9);
    Console.WriteLine();
}

// ======================================================================
// Сценарій 3: Fallback + Retry (черга повідомлень / кеш)
// ======================================================================
static async Task Scenario3_RetryWithFallbackAsync()
{
    Console.WriteLine("===== Сценарій 3: Fallback + Retry (читання профілю) =====");
    int call = 0;

    ValueTask<string> ReadProfileFromServer(CancellationToken ct)
    {
        call++;
        Log($"Звернення до сервера профілів №{call}");
        throw new HttpRequestException("Сервер профілів недоступний.");
    }

    // Fallback — зовнішня стратегія: спрацьовує, коли Retry вже вичерпав усі спроби.
    var pipeline = new ResiliencePipelineBuilder<string>()
        .AddFallback(new FallbackStrategyOptions<string>
        {
            ShouldHandle = new PredicateBuilder<string>().Handle<HttpRequestException>(),
            FallbackAction = args => Outcome.FromResultAsValueTask("профіль із локального кешу (застарілі дані)"),
            OnFallback = args =>
            {
                Log("Fallback: сервер недоступний, віддаємо кешоване значення");
                return default;
            }
        })
        .AddRetry(new RetryStrategyOptions<string>
        {
            ShouldHandle = new PredicateBuilder<string>().Handle<HttpRequestException>(),
            MaxRetryAttempts = 2,
            Delay = TimeSpan.FromMilliseconds(150),
            BackoffType = DelayBackoffType.Exponential,
            UseJitter = true,
            OnRetry = args =>
            {
                Log($"Retry #{args.AttemptNumber + 1} через {args.RetryDelay.TotalMilliseconds:F0} мс");
                return default;
            }
        })
        .Build();

    string result = await pipeline.ExecuteAsync(async ct => await ReadProfileFromServer(ct));
    Log($"Користувач отримав: {result}\n");
}
