namespace lab14v14;

public class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }

    public override string ToString()
    {
        return $"{Name} — {Price} грн";
    }
}
