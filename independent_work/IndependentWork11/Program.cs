using System.Diagnostics;
using IndependentWork11;

Console.OutputEncoding = System.Text.Encoding.UTF8;

var random = new Random(14);
var products = DataGenerator.GenerateProducts(random, 60);
var customers = DataGenerator.GenerateCustomers(random, 80);
var sales = DataGenerator.GenerateSales(random, 600, products.Count, customers.Count);

Console.WriteLine($"Товарів: {products.Count}, клієнтів: {customers.Count}, продажів: {sales.Count}\n");

var analytics = new SalesAnalytics(products, customers, sales);

Run("Пайплайн 1: Найпопулярніші продукти за категоріями", analytics.TopProductsByCategory);
Run("Пайплайн 2: Загальний дохід за містами", analytics.RevenueByCity);
Run("Пайплайн 3: Продажі за місяцями та роками", analytics.SalesByMonth);
Run("Пайплайн 4 (додатково): Лояльні клієнти", analytics.LoyalCustomers);

// Для кожного пайплайну створюється власний Stopwatch.
static void Run(string title, Func<List<string>> pipeline)
{
    var sw = Stopwatch.StartNew();
    var lines = pipeline();
    sw.Stop();

    Console.WriteLine($"=== {title} ===");
    foreach (var line in lines)
        Console.WriteLine(line);
    Console.WriteLine($"Час виконання: {sw.Elapsed.TotalMilliseconds:F3} мс\n");
}
