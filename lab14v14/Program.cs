namespace lab14v14;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Демонстрація роботи CollectionHelper<T>.Take\n");

        // 1. Демонстрація з колекцією List<int>
        List<int> numbers = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80 };
        int takeNumbersCount = 5;

        Console.WriteLine($"Початковий список чисел: {string.Join(", ", numbers)}");
        Console.WriteLine($"Перші {takeNumbersCount} елементів:");

        IEnumerable<int> firstFiveNumbers = CollectionHelper<int>.Take(numbers, takeNumbersCount);

        // Використання Action<T> через лямбда-вираз для виводу
        Action<int> printInt = n => Console.Write($"{n} ");
        foreach (var num in firstFiveNumbers)
        {
            printInt(num);
        }
        Console.WriteLine("\n" + new string('-', 40));

        // 2. Демонстрація з колекцією об'єктів List<Product>
        List<Product> products = new List<Product>
        {
            new Product("Ноутбук", 35000),
            new Product("Мишка", 800),
            new Product("Клавіатура", 2200),
            new Product("Монітор", 11500),
            new Product("Навушники", 3100)
        };

        int takeProductsCount = 3;

        Console.WriteLine($"\nПовний список продуктів ({products.Count} шт.):");
        products.ForEach(p => Console.WriteLine($" - {p}"));

        Console.WriteLine($"\nПерші {takeProductsCount} продукти:");
        IEnumerable<Product> firstThreeProducts = CollectionHelper<Product>.Take(products, takeProductsCount);

        // Використання Action<Product> для форматованого виводу
        Action<Product> printProduct = p => Console.WriteLine($" [Top {takeProductsCount}] {p}");
        foreach (var prod in firstThreeProducts)
        {
            printProduct(prod);
        }
    }
}
