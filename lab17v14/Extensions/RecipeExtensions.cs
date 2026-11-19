using lab17v14.Models;

namespace lab17v14.Extensions;

public static class RecipeExtensions
{
    public static bool HasIngredient(this Recipe recipe, string ingredientName)
    {
        ArgumentNullException.ThrowIfNull(recipe);

        if (string.IsNullOrWhiteSpace(ingredientName))
            return false;

        string name = ingredientName.Trim();
        return recipe.Ingredients.Any(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
