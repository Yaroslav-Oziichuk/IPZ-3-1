namespace IndependentWork11;

public class SalesAnalytics
{
    private readonly List<Product> _products;
    private readonly List<Customer> _customers;
    private readonly List<Sale> _sales;

    public SalesAnalytics(List<Product> products, List<Customer> customers, List<Sale> sales)
    {
        _products = products;
        _customers = customers;
        _sales = sales;
    }

    public List<string> TopProductsByCategory()
    {
        var query = _sales
            .GroupBy(s => s.ProductId)
            .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(s => s.Quantity) })
            .Join(_products, t => t.ProductId, p => p.Id,
                  (t, p) => new { p.Name, p.Category, t.TotalQuantity })
            .GroupBy(x => x.Category)
            .Select(g => new
            {
                Category = g.Key,
                Top = g.OrderByDescending(x => x.TotalQuantity).Take(3).ToList()
            })
            .OrderBy(x => x.Category);

        var lines = new List<string>();
        foreach (var c in query)
        {
            lines.Add($"[{c.Category}]");
            int place = 1;
            foreach (var p in c.Top)
                lines.Add($"   {place++}. {p.Name} — {p.TotalQuantity} шт.");
        }
        return lines;
    }

    public List<string> RevenueByCity()
    {
        return _sales
            .Join(_products, s => s.ProductId, p => p.Id,
                  (s, p) => new { s.CustomerId, Revenue = s.Quantity * p.UnitPrice })
            .Join(_customers, x => x.CustomerId, c => c.Id,
                  (x, c) => new { c.City, x.Revenue })
            .GroupBy(x => x.City)
            .Select(g => new { City = g.Key, Total = g.Sum(x => x.Revenue) })
            .OrderByDescending(x => x.Total)
            .Select(x => $"{x.City,-12} {x.Total,14:N2} грн")
            .ToList();
    }

    public List<string> SalesByMonth()
    {
        return _sales
            .Join(_products, s => s.ProductId, p => p.Id,
                  (s, p) => new { s.SaleDate, s.Quantity, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => new { x.SaleDate.Year, x.SaleDate.Month })
            .Select(g => new
            {
                g.Key.Year,
                g.Key.Month,
                Units = g.Sum(x => x.Quantity),
                Revenue = g.Sum(x => x.Revenue)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .Select(x => $"{x.Year}-{x.Month:D2}: {x.Units,4} шт., {x.Revenue,12:N2} грн")
            .ToList();
    }

    public List<string> LoyalCustomers()
    {
        return _sales
            .Join(_products, s => s.ProductId, p => p.Id,
                  (s, p) => new { s.CustomerId, p.Category, Revenue = s.Quantity * p.UnitPrice })
            .GroupBy(x => x.CustomerId)
            .Select(g => new
            {
                CustomerId = g.Key,
                Categories = g.Select(x => x.Category).Distinct().Count(),
                Revenue = g.Sum(x => x.Revenue)
            })
            .Where(x => x.Categories >= 3)
            .Join(_customers, x => x.CustomerId, c => c.Id,
                  (x, c) => new { c.Name, c.City, x.Categories, x.Revenue })
            .OrderByDescending(x => x.Revenue)
            .Take(5)
            .Select(x => $"{x.Name} ({x.City}): {x.Categories} категорій, {x.Revenue:N2} грн")
            .ToList();
    }
}
