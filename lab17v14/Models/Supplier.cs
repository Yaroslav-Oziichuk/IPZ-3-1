namespace lab17v14.Models;

public class Supplier
{
    public int Id { get; }
    public string Name { get; }

    public Supplier(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => $"{Name} (#{Id})";
}
