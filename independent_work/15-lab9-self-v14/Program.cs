using System.Text;
using Lab9SelfV14;
using Lab9SelfV14.Models;

Console.OutputEncoding = Encoding.UTF8;

var numbers = new List<int> { 3, 8, 15, 4, 23, 42, 16, 7 };
var words = new List<string> { "delegate", "lambda", "func", "action", "predicate", "linq" };
var products = new List<Product>
{
    new() { Name = "Ноутбук",       Price = 25000, Category = "Комп'ютери" },
    new() { Name = "Миша",          Price = 500,   Category = "Аксесуари" },
    new() { Name = "Клавіатура",    Price = 1500,  Category = "Аксесуари" },
    new() { Name = "Монітор",       Price = 7200,  Category = "Комп'ютери" },
    new() { Name = "USB-кабель",    Price = 150,   Category = "Аксесуари" },
};

PrintHeader("1. List<int>: " + string.Join(", ", numbers));
var squares = CollectionUtils.Map(numbers, n => n * n);
Console.WriteLine("Map (квадрати):            " + string.Join(", ", squares));

var asText = CollectionUtils.Map(numbers, n => n % 2 == 0 ? $"{n}(парне)" : $"{n}(непарне)");
Console.WriteLine("Map (int -> string):       " + string.Join(", ", asText));

Console.WriteLine("MaxBy (найбільше число):   " + CollectionUtils.MaxBy(numbers, n => n));
Console.WriteLine("MaxBy (найбільша остача від ділення на 10): " + CollectionUtils.MaxBy(numbers, n => n % 10));

var evens = CollectionUtils.Filter(numbers, n => n % 2 == 0);
Console.WriteLine("Filter (парні):            " + string.Join(", ", evens));

var greaterThan10 = CollectionUtils.Filter(numbers, n => n > 10);
Console.Write("ForEach (числа > 10):      ");
CollectionUtils.ForEach(greaterThan10, n => Console.Write(n + " "));
Console.WriteLine();

int sum = 0;
CollectionUtils.ForEach(numbers, n => sum += n);   // лямбда із захопленням змінної (closure)
Console.WriteLine("ForEach (сума через closure): " + sum);

// ---------------------------------------------------------------------------
PrintHeader("2. List<string>: " + string.Join(", ", words));
// ---------------------------------------------------------------------------
Console.WriteLine("Map (ToUpper):             " + string.Join(", ", CollectionUtils.Map(words, w => w.ToUpper())));
Console.WriteLine("Map (довжини):             " + string.Join(", ", CollectionUtils.Map(words, w => w.Length)));
Console.WriteLine("MaxBy (найдовше слово):    " + CollectionUtils.MaxBy(words, w => w.Length));
Console.WriteLine("MaxBy (алфавітно останнє): " + CollectionUtils.MaxBy(words, w => w));
Console.WriteLine("Filter (починається з 'l'): " + string.Join(", ", CollectionUtils.Filter(words, w => w.StartsWith('l'))));
Console.WriteLine("Filter (довше за 5):       " + string.Join(", ", CollectionUtils.Filter(words, w => w.Length > 5)));
Console.WriteLine("ForEach (з нумерацією):");
int index = 1;
CollectionUtils.ForEach(words, w => Console.WriteLine($"  {index++}. {w}"));

// ---------------------------------------------------------------------------
PrintHeader("3. List<Product>");
// ---------------------------------------------------------------------------
Console.WriteLine("Map (назви):               " + string.Join(", ", CollectionUtils.Map(products, p => p.Name)));

var mostExpensive = CollectionUtils.MaxBy(products, p => p.Price);
Console.WriteLine($"MaxBy (найдорожчий):       {mostExpensive.Name} - {mostExpensive.Price}");

var cheapest = CollectionUtils.MaxBy(products, p => -p.Price);
Console.WriteLine($"MaxBy (найдешевший, ключ -Price): {cheapest.Name} - {cheapest.Price}");

var expensive = CollectionUtils.Filter(products, p => p.Price > 1000);
Console.WriteLine("Filter (дорожчі за 1000):");
CollectionUtils.ForEach(expensive, p => Console.WriteLine($"  - {p.Name}"));

Console.WriteLine("ForEach (усі товари):");
CollectionUtils.ForEach(products, p => Console.WriteLine($"  {p.Name}: {p.Price} грн"));

// Комбінування: Filter -> Map (ланцюжок, як у LINQ)
var accessoryNames = CollectionUtils.Map(
    CollectionUtils.Filter(products, p => p.Category == "Аксесуари"),
    p => p.Name.ToUpper());
Console.WriteLine("Filter + Map (аксесуари, великими літерами): " + string.Join(", ", accessoryNames));

PrintHeader("4. Відкладене виконання (Map і Filter ледачі, ForEach негайний)");
int calls = 0;
var lazy = CollectionUtils.Map(numbers, n => { calls++; return n * 2; });
Console.WriteLine($"Після створення Map: mapper викликано {calls} раз(ів)");
_ = lazy.ToList();
Console.WriteLine($"Після перебору:      mapper викликано {calls} раз(ів)");

PrintHeader("5. Порівняння з LINQ: результати збігаються");
Console.WriteLine("Map    == Select:  " + CollectionUtils.Map(numbers, n => n * n).SequenceEqual(numbers.Select(n => n * n)));
Console.WriteLine("Filter == Where:   " + CollectionUtils.Filter(products, p => p.Price > 1000).SequenceEqual(products.Where(p => p.Price > 1000)));
Console.WriteLine("MaxBy  == MaxBy:   " + ReferenceEquals(CollectionUtils.MaxBy(products, p => p.Price), products.MaxBy(p => p.Price)));

PrintHeader("6. Граничні випадки");
try
{
    CollectionUtils.MaxBy(new List<int>(), n => n);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine("MaxBy(порожня колекція): InvalidOperationException - " + ex.Message);
}

try
{
    CollectionUtils.Map<int, int>(null!, n => n);
}
catch (ArgumentNullException ex)
{
    Console.WriteLine("Map(null, ...): ArgumentNullException, параметр = " + ex.ParamName);
}

Console.WriteLine("Filter(порожня колекція): " + CollectionUtils.Filter(new List<int>(), n => true).Count() + " елементів");

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 70));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 70));
}
