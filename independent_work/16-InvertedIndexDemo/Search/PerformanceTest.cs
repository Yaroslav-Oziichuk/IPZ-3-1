using System.Diagnostics;
using InvertedIndexDemo.Models;

namespace InvertedIndexDemo.Search;

/// <summary>Результат серії вимірювань для одного набору даних і одного слова.</summary>
public record BenchmarkResult(
    int DocumentCount,
    string Word,
    int Found,
    double BuildIndexMs,
    double LinearMicroseconds,
    double IndexMicroseconds)
{
    /// <summary>У скільки разів пошук через індекс швидший за лінійний.</summary>
    public double Speedup => IndexMicroseconds > 0 ? LinearMicroseconds / IndexMicroseconds : double.PositiveInfinity;

    /// <summary>
    /// Після скількох запитів індекс окупається: час побудови індексу ділиться на
    /// економію часу на одному запиті (лінійний мінус індексний).
    /// </summary>
    public double BreakEvenQueries
    {
        get
        {
            double savedPerQueryMs = (LinearMicroseconds - IndexMicroseconds) / 1000.0;
            return savedPerQueryMs > 0 ? BuildIndexMs / savedPerQueryMs : double.PositiveInfinity;
        }
    }
}

public static class PerformanceTest
{
    /// <summary>
    /// Разове порівняння, як у методичці: вимірює один запуск у тіках Stopwatch.
    /// Для кількох документів результат шумний (JIT, кеші), тому перед замірами виконується прогрів.
    /// </summary>
    public static void CompareSearchMethods(List<Document> documents, string searchWord)
    {
        var sw = new Stopwatch();

        string normalizedWord = TextProcessor.Normalize(searchWord).First();

        // Прогрів (JIT-компіляція, ініціалізація Regex), щоб перший замір не був спотворений
        _ = LinearSearch(documents, normalizedWord);

        // 1. Лінійний пошук
        sw.Start();
        var linearResults = LinearSearch(documents, normalizedWord);
        sw.Stop();
        Console.WriteLine($"  Linear search:  {sw.ElapsedTicks,8} ticks");

        // 2. Inverted Index
        var index = new InvertedIndex();
        foreach (var doc in documents)
            index.AddDocument(doc);

        _ = index.SearchSingleWord(searchWord);   // прогрів

        sw.Restart();
        var indexResults = index.SearchSingleWord(searchWord);
        sw.Stop();
        Console.WriteLine($"  Inverted Index: {sw.ElapsedTicks,8} ticks");

        // Перевірка коректності
        Console.WriteLine($"  Linear found: {linearResults.Count}, Index found: {indexResults.Count}");
    }

    /// <summary>
    /// Точніше порівняння: кожен варіант виконується багато разів, береться середній час запиту.
    /// Окремо вимірюється час побудови індексу.
    /// </summary>
    public static BenchmarkResult Benchmark(List<Document> documents, string searchWord, int repeats)
    {
        string normalizedWord = TextProcessor.Normalize(searchWord).First();

        // Побудова індексу
        var buildWatch = Stopwatch.StartNew();
        var index = new InvertedIndex();
        foreach (var doc in documents)
            index.AddDocument(doc);
        buildWatch.Stop();

        // Прогрів
        var linearWarm = LinearSearch(documents, normalizedWord);
        var indexWarm = index.SearchSingleWord(searchWord);

        if (linearWarm.Count != indexWarm.Count)
            throw new InvalidOperationException(
                $"Результати не збігаються: лінійний {linearWarm.Count}, індекс {indexWarm.Count}.");

        // Лінійний пошук: на великих наборах менше повторів, щоб тест не тривав надто довго
        int linearRepeats = Math.Max(1, repeats * 10 / Math.Max(10, documents.Count / 10));
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < linearRepeats; i++)
            _ = LinearSearch(documents, normalizedWord);
        sw.Stop();
        double linearUs = sw.Elapsed.TotalMicroseconds / linearRepeats;

        // Пошук через індекс
        sw.Restart();
        for (int i = 0; i < repeats; i++)
            _ = index.SearchSingleWord(searchWord);
        sw.Stop();
        double indexUs = sw.Elapsed.TotalMicroseconds / repeats;

        return new BenchmarkResult(
            documents.Count, searchWord, indexWarm.Count,
            buildWatch.Elapsed.TotalMilliseconds, linearUs, indexUs);
    }

    /// <summary>Лінійний пошук без індексу: токенізація кожного документа при кожному запиті.</summary>
    private static List<Document> LinearSearch(List<Document> documents, string normalizedWord) =>
        documents.Where(d => TextProcessor.Normalize(d.Content).Contains(normalizedWord)).ToList();
}
