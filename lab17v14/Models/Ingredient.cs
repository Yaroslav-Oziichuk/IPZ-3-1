namespace lab17v14.Models;

public class Ingredient
{
    public int Id { get; }
    public string Name { get; }
    public List<Supplier> Suppliers { get; }

    public Ingredient(int id, string name, params Supplier[] suppliers)
    {
        Id = id;
        Name = name;
        Suppliers = suppliers.ToList();
    }

    public override string ToString() => Name;
}
