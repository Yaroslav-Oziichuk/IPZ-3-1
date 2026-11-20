using System.Text;
using InvertedIndexDemo.Models;
using InvertedIndexDemo.Search;

Console.OutputEncoding = Encoding.UTF8;

var documents = TestData.CreateBaseDocuments();

var index = new InvertedIndex();
foreach (var doc in documents)
    index.AddDocument(doc);

Console.WriteLine($"Loaded {index.DocumentCount} documents");
Console.WriteLine($"Index contains {index.UniqueWords} unique words");

// ---------------------------------------------------------------------------
PrintHeader("Частина 1. Демонстрація пошуку (AND / OR / одне слово)");
// ---------------------------------------------------------------------------
PrintResults("SearchSingleWord(\"python\")", index.SearchSingleWord("python"));
PrintResults("SearchMultipleWords(\"programming\", \"language\")  [AND]", index.SearchMultipleWords("programming", "language"));
PrintResults("SearchMultipleWords(\"c#\", \"async\")              [AND]", index.SearchMultipleWords("c#", "async"));
PrintResults("SearchMultipleWords(\"java\", \"python\")           [AND]", index.SearchMultipleWords("java", "python"));
PrintResults("SearchAnyWord(\"linq\", \"portability\")            [OR]", index.SearchAnyWord("linq", "portability"));
PrintResults("SearchSingleWord(\"the\")  (стоп-слово)", index.SearchSingleWord("the"));
PrintResults("SearchSingleWord(\"kotlin\") (немає в індексі)", index.SearchSingleWord("kotlin"));

// ---------------------------------------------------------------------------
PrintHeader("Частина 2. Одноразове порівняння (тіки Stopwatch), 10 і 1000 документів");
// ---------------------------------------------------------------------------
foreach (int count in new[] { 10, 1000 })
{
    Console.WriteLine($"{count} документів, слово \"programming\":");
    PerformanceTest.CompareSearchMethods(TestData.Generate(count), "programming");
}

// ---------------------------------------------------------------------------
PrintHeader("Частина 3. Усереднені вимірювання (мкс на один запит)");
// ---------------------------------------------------------------------------
Console.WriteLine($"{"Docs",6} | {"Слово",-11} | {"Знайдено",8} | {"Побудова, мс",12} | {"Лінійний, мкс",13} | {"Індекс, мкс",11} | {"Прискорення",11} | {"Окупність, запитів",18}");
Console.WriteLine(new string('-', 112));

foreach (int count in new[] { 10, 100, 1000, 10000 })
{
    var data = TestData.Generate(count);

    foreach (string word in new[] { "programming", "term7" })
    {
        BenchmarkResult r = PerformanceTest.Benchmark(data, word, repeats: 2000);
        string breakEven = double.IsInfinity(r.BreakEvenQueries) ? "-" : $"{r.BreakEvenQueries:F1}";
        Console.WriteLine(
            $"{r.DocumentCount,6} | {r.Word,-11} | {r.Found,8} | {r.BuildIndexMs,12:F3} | {r.LinearMicroseconds,13:F2} | {r.IndexMicroseconds,11:F2} | {r.Speedup,10:F1}x | {breakEven,18}");
    }
}

// ---------------------------------------------------------------------------
PrintHeader("Частина 4. Інтерактивний пошук");
// ---------------------------------------------------------------------------
while (true)
{
    Console.Write("Enter search query (or 'exit'): ");
    string? query = Console.ReadLine();

    // null - кінець введення (наприклад, ввід перенаправлено з файлу)
    if (query is null)
        break;

    // Перенаправлений ввід не «відлунюється» в консоль, тому виводимо запит самі.
    if (Console.IsInputRedirected)
        Console.WriteLine(query);

    if (query.Trim().Equals("exit", StringComparison.OrdinalIgnoreCase))
        break;

    string[] words = query.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    Console.WriteLine("\n--- Search Results (AND) ---");
    var results = index.SearchMultipleWords(words);

    foreach (var doc in results)
    {
        Console.WriteLine(doc);
        Console.WriteLine($"  {doc.Content.Substring(0, Math.Min(100, doc.Content.Length))}...");
    }

    Console.WriteLine($"\nFound {results.Count} documents\n");
}

static void PrintResults(string title, List<Document> results)
{
    string list = results.Count == 0 ? "(нічого)" : string.Join(", ", results);
    Console.WriteLine($"  {title}\n      -> {results.Count}: {list}");
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 78));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 78));
}
