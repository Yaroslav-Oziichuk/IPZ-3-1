using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using DictionaryHashSetDemo.Models;
using DictionaryHashSetDemo.Services;

Console.OutputEncoding = Encoding.UTF8;

// ===========================================================================
PrintHeader("Сценарій 1. FileMetadataCache (Dictionary<string, FileMetadata>)");
// ===========================================================================
var cache = new FileMetadataCache();

cache.Add(MakeFile("report.pdf", "Квартальний звіт", new DateTime(2026, 9, 12, 14, 30, 0)));
cache.Add(MakeFile("photo.jpg", "binary-image-data", new DateTime(2026, 10, 3, 9, 15, 0)));
cache.Add(MakeFile("notes.txt", "Нотатки до лекції", new DateTime(2026, 11, 1, 18, 45, 0)));
Console.WriteLine($"Додано файлів: {cache.Count}");

Console.WriteLine($"Contains(\"photo.jpg\")   = {cache.Contains("photo.jpg")}");
Console.WriteLine($"Contains(\"PHOTO.JPG\")   = {cache.Contains("PHOTO.JPG")}  <- регістронезалежний ключ");
Console.WriteLine($"Contains(\"video.mp4\")   = {cache.Contains("video.mp4")}");
Console.WriteLine($"Get(\"report.pdf\")       = {cache.Get("report.pdf")}");
Console.WriteLine($"Get(\"missing.bin\")      = {(cache.Get("missing.bin") is null ? "null" : "знайдено")}");

bool addedDuplicate = cache.Add(MakeFile("REPORT.PDF", "Інший вміст", DateTime.Now));
Console.WriteLine($"Add(\"REPORT.PDF\") повторно -> {addedDuplicate} (дублікат не додано, Count = {cache.Count})");

cache.AddOrUpdate(MakeFile("notes.txt", "Нотатки до лекції (оновлені)", new DateTime(2026, 11, 20, 10, 0, 0)));
Console.WriteLine($"AddOrUpdate(notes.txt) -> {cache.Get("notes.txt")}");

Console.WriteLine($"Remove(\"photo.jpg\")     = {cache.Remove("photo.jpg")}");
Console.WriteLine($"Remove(\"photo.jpg\") ще раз = {cache.Remove("photo.jpg")}");
Console.WriteLine("Вміст кешу:");
foreach (FileMetadata m in cache.GetAll())
    Console.WriteLine($"  {m}");

// ===========================================================================
PrintHeader("Сценарій 2. TagManager (Dictionary<string, HashSet<string>>)");
// ===========================================================================
var tags = new TagManager();

foreach (string tag in new[] { "csharp", "dotnet", "linq", "collections" }) tags.AddTag("article-1", tag);
foreach (string tag in new[] { "CSharp", "async", "dotnet" })               tags.AddTag("article-2", tag);
foreach (string tag in new[] { "python", "linq" })                           tags.AddTag("article-3", tag);

bool dup = tags.AddTag("article-1", "CSHARP");
Console.WriteLine($"AddTag(article-1, \"CSHARP\") -> {dup} (тег уже є, регістр не важливий)");

Console.WriteLine($"Теги article-1: {Join(tags.GetTags("article-1"))}");
Console.WriteLine($"Теги article-2: {Join(tags.GetTags("article-2"))}");
Console.WriteLine($"Теги article-3: {Join(tags.GetTags("article-3"))}");

Console.WriteLine($"GetAllUniqueTags()                     [UnionWith]:     {Join(tags.GetAllUniqueTags())}");
Console.WriteLine($"FindCommonTags(article-1, article-2)   [IntersectWith]: {Join(tags.FindCommonTags("article-1", "article-2"))}");
Console.WriteLine($"FindCommonTags(article-1, article-3)   [IntersectWith]: {Join(tags.FindCommonTags("article-1", "article-3"))}");
Console.WriteLine($"FindTagsOnlyInFirst(article-1, art-2)  [ExceptWith]:    {Join(tags.FindTagsOnlyInFirst("article-1", "article-2"))}");
Console.WriteLine($"FindCommonTags(article-1, неіснуюча)   [порожня множина]: {Join(tags.FindCommonTags("article-1", "article-999"))}");

Console.WriteLine($"RemoveTag(article-1, \"linq\") = {tags.RemoveTag("article-1", "linq")}");
Console.WriteLine($"Теги article-1 після видалення: {Join(tags.GetTags("article-1"))}");
Console.WriteLine($"Оригінал не зіпсовано після FindCommonTags: {Join(tags.GetTags("article-1"))}");

// ===========================================================================
PrintHeader("Сценарій 3 (додатково). SearchIndex (Dictionary<string, HashSet<int>>)");
// ===========================================================================
var searchIndex = new SearchIndex();
searchIndex.AddDocument(1, "C# is a great programming language");
searchIndex.AddDocument(2, "Java is also a programming language");
searchIndex.AddDocument(3, "C# and Java are popular");

Console.WriteLine($"Унікальних слів в індексі: {searchIndex.UniqueWords}");
Console.WriteLine($"Search(\"c#\")                    -> {{{string.Join(", ", searchIndex.Search("c#").Order())}}}");
Console.WriteLine($"Search(\"JAVA\")                  -> {{{string.Join(", ", searchIndex.Search("JAVA").Order())}}}");
Console.WriteLine($"Search(\"kotlin\")                -> {{{string.Join(", ", searchIndex.Search("kotlin").Order())}}}");
Console.WriteLine($"SearchMultiple(\"c#\", \"java\")    -> {{{string.Join(", ", searchIndex.SearchMultiple("c#", "java").Order())}}}");
Console.WriteLine($"SearchMultiple(\"programming\", \"language\") -> {{{string.Join(", ", searchIndex.SearchMultiple("programming", "language").Order())}}}");

// ===========================================================================
PrintHeader("Додатково. Власний IEqualityComparer і регістр ключів");
// ===========================================================================
var caseInsensitive = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["Hello"] = 1 };
Console.WriteLine($"Dictionary з OrdinalIgnoreCase: [\"HELLO\"] = {caseInsensitive["HELLO"]}");

var caseSensitive = new Dictionary<string, int> { ["Hello"] = 1 };
Console.WriteLine($"Dictionary зі стандартним порівнянням: ContainsKey(\"HELLO\") = {caseSensitive.ContainsKey("HELLO")}");

var people = new HashSet<Person>(new PersonNameComparer());
bool first = people.Add(new Person { Name = "John", Age = 25 });
bool second = people.Add(new Person { Name = "john", Age = 30 });
Console.WriteLine($"HashSet<Person> з PersonNameComparer: Add(John) = {first}, Add(john) = {second}, Count = {people.Count}");

var withoutComparer = new HashSet<Person>();
withoutComparer.Add(new Person { Name = "John", Age = 25 });
withoutComparer.Add(new Person { Name = "John", Age = 30 });
Console.WriteLine($"HashSet<Person> без comparer (порівняння за посиланням): Count = {withoutComparer.Count}");

// ===========================================================================
PrintHeader("Додатково. Вимірювання: List.Contains проти HashSet.Contains");
// ===========================================================================
foreach (int size in new[] { 1_000, 10_000, 50_000 })
{
    var list = Enumerable.Range(0, size).ToList();
    var set = new HashSet<int>(list);
    int lookups = 2_000;

    // Шукаємо елементи з кінця, тобто для List це найгірший випадок
    var sw = Stopwatch.StartNew();
    int foundInList = 0;
    for (int i = 0; i < lookups; i++)
        if (list.Contains(size - 1 - (i % 100))) foundInList++;
    sw.Stop();
    double listMs = sw.Elapsed.TotalMilliseconds;

    sw.Restart();
    int foundInSet = 0;
    for (int i = 0; i < lookups; i++)
        if (set.Contains(size - 1 - (i % 100))) foundInSet++;
    sw.Stop();
    double setMs = sw.Elapsed.TotalMilliseconds;

    Console.WriteLine($"  {size,6} елементів, {lookups} пошуків: List {listMs,9:F3} мс | HashSet {setMs,7:F3} мс (знайдено {foundInList}/{foundInSet})");
}

static FileMetadata MakeFile(string name, string content, DateTime modified) => new()
{
    FileName = name,
    Size = Encoding.UTF8.GetByteCount(content) * 1024,
    LastModified = modified,
    Hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(content)))[..12].ToLowerInvariant()
};

static string Join(IEnumerable<string> items)
{
    var sorted = items.OrderBy(i => i, StringComparer.OrdinalIgnoreCase).ToList();
    return sorted.Count == 0 ? "{}" : "{" + string.Join(", ", sorted) + "}";
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 78));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 78));
}
