namespace DictionaryHashSetDemo.Models;

public class Person
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }

    public override string ToString() => $"{Name} ({Age})";
}

public class PersonNameComparer : IEqualityComparer<Person>
{
    public bool Equals(Person? x, Person? y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x is null || y is null) return false;
        return string.Equals(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode(Person obj) =>
        StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Name);
}
