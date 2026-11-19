namespace lab18v14.Services;

public static class RetryHelper
{
    public static readonly TimeSpan DefaultInitialDelay = TimeSpan.FromMilliseconds(500);

    public static T ExecuteWithRetry<T>(
        Func<T> operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);
        ArgumentOutOfRangeException.ThrowIfNegative(retryCount);

        if (initialDelay <= TimeSpan.Zero)
            initialDelay = DefaultInitialDelay;

        shouldRetry ??= _ => true;

        for (int attempt = 0; ; attempt++)
        {
            try
            {
                Log($"Спроба {attempt + 1} з {retryCount + 1}...", ConsoleColor.Cyan);
                T result = operation();
                Log($"Спроба {attempt + 1}: успіх.", ConsoleColor.Green);
                return result;
            }
            catch (Exception ex)
            {
                Log($"Спроба {attempt + 1}: помилка {ex.GetType().Name}: {ex.Message}", ConsoleColor.Yellow);

                if (!shouldRetry(ex))
                {
                    Log($"{ex.GetType().Name} не належить до тимчасових, повтор не виконується.", ConsoleColor.Red);
                    throw;
                }

                if (attempt >= retryCount)
                {
                    Log($"Усі спроби ({retryCount + 1}) вичерпано, помилка передається далі.", ConsoleColor.Red);
                    throw;
                }

                // Експоненційна затримка: initialDelay * 2^attempt
                TimeSpan delay = TimeSpan.FromTicks(initialDelay.Ticks * (1L << attempt));
                Log($"Очікування {delay.TotalMilliseconds:F0} мс " +
                    $"({initialDelay.TotalMilliseconds:F0} мс * 2^{attempt}) перед повтором...", ConsoleColor.DarkYellow);
                Thread.Sleep(delay);
            }
        }
    }

    public static void ExecuteWithRetry(
        Action operation,
        int retryCount = 3,
        TimeSpan initialDelay = default,
        Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        ExecuteWithRetry<bool>(() =>
        {
            operation();
            return true;
        }, retryCount, initialDelay, shouldRetry);
    }

    private static void Log(string message, ConsoleColor color)
    {
        ConsoleColor previous = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine($"  [{DateTime.Now:HH:mm:ss.fff}] [Retry] {message}");
        Console.ForegroundColor = previous;
    }
}
