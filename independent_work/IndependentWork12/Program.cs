using System.Collections.Concurrent;
using System.Diagnostics;

namespace IndependentWork12;

public static class Program
{
    private const int Seed = 14; 

    public static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine($"Ядер процесора: {Environment.ProcessorCount}\n");

        // Прогрів JIT, щоб перше вимірювання не було несправедливо повільним.
        var warm = GenerateData(10_000);
        warm.Where(IsPrime).Select(n => Math.Sqrt(n)).ToList();
        warm.AsParallel().Where(IsPrime).Select(n => Math.Sqrt(n)).ToList();

        Console.WriteLine("##### ЧАСТИНА 1. Важка операція (перевірка на просте число + sqrt) #####");
        foreach (int size in new[] { 1_000_000, 5_000_000, 10_000_000 }) // 5M — необов'язковий
            CompareHeavy(size);

        Console.WriteLine("##### ЧАСТИНА 2. Дешева операція (x * 2) — PLINQ не завжди вигідний #####");
        CompareCheap(1_000_000);

        Console.WriteLine("##### ЧАСТИНА 3. Побічні ефекти та потокобезпечність #####");
        DemonstrateSideEffects();
    }

    // ---------- Дані та операція ----------

    private static List<int> GenerateData(int size)
    {
        var random = new Random(Seed);
        var list = new List<int>(size);
        for (int i = 0; i < size; i++)
            list.Add(random.Next(2, 10_000_000));
        return list;
    }

    private static bool IsPrime(int n)
    {
        if (n < 2) return false;
        if (n % 2 == 0) return n == 2;
        for (int i = 3; (long)i * i <= n; i += 2)
            if (n % i == 0) return false;
        return true;
    }

    // ---------- Порівняння продуктивності ----------

    private static void CompareHeavy(int size)
    {
        var data = GenerateData(size);
        Console.WriteLine($"--- Розмір колекції: {size:N0} ---");

        var sw = Stopwatch.StartNew();
        var linqResult = data.Where(IsPrime).Select(n => Math.Sqrt(n)).ToList();
        sw.Stop();
        var linqTime = sw.Elapsed;

        sw.Restart();
        var plinqResult = data.AsParallel().Where(IsPrime).Select(n => Math.Sqrt(n)).ToList();
        sw.Stop();
        var plinqTime = sw.Elapsed;

        Console.WriteLine($"LINQ : {linqTime.TotalMilliseconds,10:F1} мс, знайдено {linqResult.Count:N0}");
        Console.WriteLine($"PLINQ: {plinqTime.TotalMilliseconds,10:F1} мс, знайдено {plinqResult.Count:N0}");
        Console.WriteLine($"Прискорення: x{linqTime.TotalMilliseconds / plinqTime.TotalMilliseconds:F2}\n");
    }

    private static void CompareCheap(int size)
    {
        var data = GenerateData(size);
        Console.WriteLine($"--- Розмір колекції: {size:N0} ---");

        var sw = Stopwatch.StartNew();
        long linqSum = data.Select(n => (long)n * 2).Sum();
        sw.Stop();
        var linqTime = sw.Elapsed;

        sw.Restart();
        long plinqSum = data.AsParallel().Select(n => (long)n * 2).Sum();
        sw.Stop();
        var plinqTime = sw.Elapsed;

        Console.WriteLine($"LINQ : {linqTime.TotalMilliseconds,10:F2} мс (сума {linqSum})");
        Console.WriteLine($"PLINQ: {plinqTime.TotalMilliseconds,10:F2} мс (сума {plinqSum})");
        Console.WriteLine($"Прискорення: x{linqTime.TotalMilliseconds / plinqTime.TotalMilliseconds:F2}\n");
    }

    // ---------- Побічні ефекти ----------

    private static void DemonstrateSideEffects()
    {
        var data = GenerateData(1_000_000);
        int expected = data.Count(x => x % 2 == 0);
        Console.WriteLine($"Очікуваний результат (послідовний підрахунок парних): {expected:N0}\n");

        // 1) Спільний лічильник без синхронізації.
        Console.WriteLine("1) counter++ без синхронізації (5 запусків):");
        for (int run = 1; run <= 5; run++)
        {
            int counter = 0;
            data.AsParallel().ForAll(x => { if (x % 2 == 0) counter++; });
            Console.WriteLine($"   запуск {run}: {counter:N0} {(counter == expected ? "(збіглося)" : "(НЕВІРНО)")}");
        }

        // Виправлення A: lock
        {
            int counter = 0;
            object sync = new();
            data.AsParallel().ForAll(x => { if (x % 2 == 0) lock (sync) { counter++; } });
            Console.WriteLine($"   виправлення lock       : {counter:N0} {(counter == expected ? "OK" : "ПОМИЛКА")}");
        }
        // Виправлення B: Interlocked
        {
            int counter = 0;
            data.AsParallel().ForAll(x => { if (x % 2 == 0) Interlocked.Increment(ref counter); });
            Console.WriteLine($"   виправлення Interlocked: {counter:N0} {(counter == expected ? "OK" : "ПОМИЛКА")}");
        }
        // Виправлення C: взагалі без спільного стану
        {
            int counter = data.AsParallel().Count(x => x % 2 == 0);
            Console.WriteLine($"   PLINQ Count() без стану: {counter:N0} {(counter == expected ? "OK" : "ПОМИЛКА")}");
        }

        // 2) Спільний List<int> — не потокобезпечний.
        Console.WriteLine("\n2) List<int>.Add з кількох потоків (3 запуски):");
        for (int run = 1; run <= 3; run++)
        {
            var shared = new List<int>();
            try
            {
                data.AsParallel().Where(x => x % 2 == 0).ForAll(x => shared.Add(x));
                Console.WriteLine($"   запуск {run}: {shared.Count:N0} елементів {(shared.Count == expected ? "(збіглося)" : "(ВТРАЧЕНО ДАНІ)")}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"   запуск {run}: виняток {ex.GetType().Name} -> {ex.InnerException?.GetType().Name}");
            }
        }

        // Виправлення: ConcurrentBag<T>
        var bag = new ConcurrentBag<int>();
        data.AsParallel().Where(x => x % 2 == 0).ForAll(x => bag.Add(x));
        Console.WriteLine($"   виправлення ConcurrentBag: {bag.Count:N0} елементів {(bag.Count == expected ? "OK" : "ПОМИЛКА")}");
    }
}
