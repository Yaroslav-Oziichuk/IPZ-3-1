namespace Lab9SelfV14.Models;

/// <summary>Товар для демонстрації утиліт колекцій.</summary>
public class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;

    public override string ToString() => $"{Name} ({Price} грн)";
}
