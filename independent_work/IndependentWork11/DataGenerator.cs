namespace IndependentWork11;

public static class DataGenerator
{
    private static readonly string[] Categories = { "Електроніка", "Одяг", "Книги", "Спорт", "Дім" };
    private static readonly string[] Cities =
        { "Рівне", "Київ", "Львів", "Одеса", "Харків", "Дніпро", "Луцьк", "Тернопіль", "Житомир", "Вінниця" };

    public static List<Product> GenerateProducts(Random random, int count = 60)
    {
        var list = new List<Product>();
        for (int i = 1; i <= count; i++)
        {
            list.Add(new Product
            {
                Id = i,
                Name = $"Товар {i}",
                Category = Categories[random.Next(Categories.Length)],
                UnitPrice = random.Next(50, 5000) + 0.99m
            });
        }
        return list;
    }

    public static List<Customer> GenerateCustomers(Random random, int count = 80)
    {
        var list = new List<Customer>();
        for (int i = 1; i <= count; i++)
        {
            list.Add(new Customer
            {
                Id = i,
                Name = $"Клієнт {i}",
                City = Cities[random.Next(Cities.Length)]
            });
        }
        return list;
    }

    public static List<Sale> GenerateSales(Random random, int count, int productCount, int customerCount)
    {
        var start = new DateTime(2023, 1, 1);
        int days = (new DateTime(2025, 12, 31) - start).Days;
        var list = new List<Sale>();
        for (int i = 1; i <= count; i++)
        {
            list.Add(new Sale
            {
                Id = i,
                ProductId = random.Next(1, productCount + 1),
                CustomerId = random.Next(1, customerCount + 1),
                Quantity = random.Next(1, 11),
                SaleDate = start.AddDays(random.Next(days + 1))
            });
        }
        return list;
    }
}
