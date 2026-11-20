using System.Net.Http;
using IndependentWork14;

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Параметри (варіант 14): 4 повтори, початкова затримка 200 мс, jitter ±20 %.
// Орієнтовні затримки: 200 -> 400 -> 800 -> 1600 мс (кожна ±20 %).
var policy = new RetryPolicy(maxRetries: 4, initialDelay: TimeSpan.FromMilliseconds(200), jitterFactor: 0.2);

// Сценарій користувача: оновлення профілю через нестабільний сервіс.
Console.WriteLine("===== Сценарій 1: 3 невдачі, потім успіх =====");
int calls1 = 0;
string result = policy.Execute(() =>
{
    calls1++;
    if (calls1 <= 3)
        throw new HttpRequestException($"Сервіс профілів тимчасово недоступний (виклик {calls1}).");
    return "Профіль користувача оновлено";
});
Console.WriteLine($"Результат: {result}\n");

Console.WriteLine("===== Сценарій 2: операція завжди падає =====");
try
{
    policy.Execute<string>(() => throw new HttpRequestException("Сервер не відповідає."));
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"Main перехопив остаточний виняток: {ex.GetType().Name}: {ex.Message}\n");
}

Console.WriteLine("===== Сценарій 3: shouldRetry забороняє повтор для ArgumentException =====");
try
{
    policy.Execute<string>(
        () => throw new ArgumentException("Некоректний JSON профілю."),
        shouldRetry: ex => ex is HttpRequestException or IOException);
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Main перехопив: {ex.GetType().Name}: {ex.Message}\n");
}

Console.WriteLine("===== Сценарій 4: shouldRetry дозволяє повтор для IOException =====");
int calls4 = 0;
string result4 = policy.Execute(() =>
{
    calls4++;
    if (calls4 == 1) throw new IOException("Файл профілю заблокований іншим процесом.");
    return "Файл профілю записано";
}, shouldRetry: ex => ex is HttpRequestException or IOException);
Console.WriteLine($"Результат: {result4}");
