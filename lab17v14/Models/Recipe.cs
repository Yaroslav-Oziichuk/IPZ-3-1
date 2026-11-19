namespace lab17v14.Models;

public class Recipe
{
    public int Id { get; }
    public string Name { get; }
    public List<Ingredient> Ingredients { get; }

    public Recipe(int id, string name, params Ingredient[] ingredients)
    {
        Id = id;
        Name = name;
        Ingredients = ingredients.ToList();
    }

    public override string ToString() => Name;
}
