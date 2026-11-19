using System.Text;
using lab18v14.Services;

Console.OutputEncoding = Encoding.UTF8;

const string profilePath = "profiles/user_14.json";
const string profileUrl = "https://api.example.com/users/14/profile";
const string profileJson = "{\"id\":14,\"name\":\"Ярослав\",\"email\":\"yaroslav@example.com\"}";

TimeSpan initialDelay = TimeSpan.FromMilliseconds(200);

static bool IsTransient(Exception ex) => ex is FileNotFoundException or HttpRequestException;

PrintHeader("1. FileProcessor.UpdateUserProfile: 3 невдачі (FileNotFoundException), потім успіх");
var fileProcessor = new FileProcessor(failuresBeforeSuccess: 3);

RetryHelper.ExecuteWithRetry(
    () => fileProcessor.UpdateUserProfile(profilePath, profileJson),
    retryCount: 3,
    initialDelay: initialDelay,
    shouldRetry: IsTransient);

Console.WriteLine($"  Підсумок: метод викликано разів: {fileProcessor.Calls}, операція завершилась успішно.");

PrintHeader("2. NetworkClient.PostUserProfile: 2 невдачі (HttpRequestException), потім успіх");
var networkClient = new NetworkClient(failuresBeforeSuccess: 2);

bool posted = RetryHelper.ExecuteWithRetry(
    () => networkClient.PostUserProfile(profileUrl, profileJson),
    retryCount: 3,
    initialDelay: initialDelay,
    shouldRetry: IsTransient);

Console.WriteLine($"  Підсумок: метод викликано разів: {networkClient.Calls}, результат = {posted}.");

PrintHeader("3. shouldRetry: вибіркова обробка (нетимчасові винятки не повторюються)");
Console.WriteLine("  3.1 IOException (не FileNotFoundException): повтору немає");
int ioAttempts = 0;
try
{
    RetryHelper.ExecuteWithRetry<bool>(
        () =>
        {
            ioAttempts++;
            throw new IOException("Диск переповнено (постійна помилка)");
        },
        retryCount: 3,
        initialDelay: initialDelay,
        shouldRetry: IsTransient);
}
catch (IOException ex)
{
    Console.WriteLine($"  Main: перехоплено {ex.GetType().Name}, спроб було {ioAttempts}.");
}

Console.WriteLine();
Console.WriteLine("  3.2 ArgumentException (некоректний JSON): повтору немає");
int argAttempts = 0;
try
{
    RetryHelper.ExecuteWithRetry<bool>(
        () =>
        {
            argAttempts++;
            throw new ArgumentException("Некоректний JSON профілю");
        },
        retryCount: 3,
        initialDelay: initialDelay,
        shouldRetry: IsTransient);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"  Main: перехоплено {ex.GetType().Name}, спроб було {argAttempts}.");
}

PrintHeader("4. Спроби вичерпано: retryCount = 2, а FileProcessor потребує 3 невдач");
var stubbornFileProcessor = new FileProcessor(failuresBeforeSuccess: 3);
try
{
    RetryHelper.ExecuteWithRetry(
        () => stubbornFileProcessor.UpdateUserProfile(profilePath, profileJson),
        retryCount: 2,
        initialDelay: initialDelay,
        shouldRetry: IsTransient);
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"  Main: операція не вдалась (спроб: {stubbornFileProcessor.Calls}), виняток: {ex.GetType().Name}.");
}

PrintHeader("5. Таблиця затримок: initialDelay = 200 мс, delay = initialDelay * 2^attempt");
for (int attempt = 0; attempt < 4; attempt++)
{
    double delayMs = initialDelay.TotalMilliseconds * Math.Pow(2, attempt);
    Console.WriteLine($"  Повтор №{attempt + 1}: 200 * 2^{attempt} = {delayMs,5:F0} мс");
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 78));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 78));
}
