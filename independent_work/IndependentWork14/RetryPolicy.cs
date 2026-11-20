namespace IndependentWork14;

public class RetryPolicy
{
    private readonly int _maxRetries;
    private readonly TimeSpan _initialDelay;
    private readonly double _jitterFactor;
    private readonly Random _random = new();

    public RetryPolicy(int maxRetries, TimeSpan initialDelay, double jitterFactor = 0.2)
    {
        if (maxRetries < 0)
            throw new ArgumentOutOfRangeException(nameof(maxRetries), "Кількість повторів не може бути від'ємною.");
        if (initialDelay < TimeSpan.Zero)
            throw new ArgumentOutOfRangeException(nameof(initialDelay), "Затримка не може бути від'ємною.");
        if (jitterFactor < 0 || jitterFactor > 1)
            throw new ArgumentOutOfRangeException(nameof(jitterFactor), "Jitter має бути в межах 0..1.");

        _maxRetries = maxRetries;
        _initialDelay = initialDelay;
        _jitterFactor = jitterFactor;
    }

    public T Execute<T>(Func<T> operation, Func<Exception, bool>? shouldRetry = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        for (int retryAttempt = 0; ; retryAttempt++)
        {
            int attemptNumber = retryAttempt + 1;
            Log($"Спроба {attemptNumber} з {_maxRetries + 1}...");

            try
            {
                T result = operation();
                Log($"Спроба {attemptNumber}: УСПІХ.");
                return result;
            }
            catch (Exception ex)
            {
                Log($"Спроба {attemptNumber}: невдача — {ex.GetType().Name}: {ex.Message}");

                // Предикат shouldRetry; якщо не заданий — повторюємо будь-який виняток.
                bool retryable = shouldRetry?.Invoke(ex) ?? true;
                if (!retryable)
                {
                    Log("Виняток не є тимчасовим (shouldRetry = false) — повтор не виконується.");
                    throw;
                }

                if (retryAttempt >= _maxRetries)
                {
                    Log($"Спроби вичерпано ({_maxRetries} повторів) — виняток передається далі.");
                    throw;
                }

                TimeSpan delay = CalculateDelay(retryAttempt);
                Log($"Очікування {delay.TotalMilliseconds:F0} мс перед повтором...");
                Thread.Sleep(delay);
            }
        }
    }

    private TimeSpan CalculateDelay(int retryAttempt)
    {
        double jitter = 1 + _random.NextDouble() * _jitterFactor * 2 - _jitterFactor;
        double ms = _initialDelay.TotalMilliseconds * Math.Pow(2, retryAttempt) * jitter;
        return TimeSpan.FromMilliseconds(ms);
    }

    private static void Log(string message) =>
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] {message}");
}
