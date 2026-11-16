namespace Lab16v14.Models;

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Budget { get; set; }

    public Department(int id, string name, decimal budget)
    {
        Id = id;
        Name = name;
        Budget = budget;
    }

    public override string ToString() => $"{Name} (бюджет {Budget:N0})";
}
