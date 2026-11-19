using System.Text;
using lab17v14.Data;
using lab17v14.Extensions;
using lab17v14.Models;

Console.OutputEncoding = Encoding.UTF8;

List<Recipe> recipes = SampleData.CreateRecipes();

// 0. Тестові дані
PrintHeader("0. Тестові дані: рецепти та кількість інгредієнтів");
foreach (Recipe r in recipes)
    Console.WriteLine($"  [{r.Id,2}] {r.Name,-30} інгредієнтів: {r.Ingredients.Count}");

// 1. SelectMany: унікальні постачальники з усіх інгредієнтів усіх рецептів
PrintHeader("1. SelectMany: унікальні постачальники");

// Select повертає колекцію колекцій: по одному вкладеному списку на рецепт.
IEnumerable<List<Ingredient>> nested = recipes.Select(r => r.Ingredients);
Console.WriteLine($"  Select:     {nested.Count()} вкладених списків (по одному на рецепт)");

// SelectMany «розгортає» вкладені списки в одну плоску послідовність.
List<Ingredient> allIngredients = recipes.SelectMany(r => r.Ingredients).ToList();
Console.WriteLine($"  SelectMany: {allIngredients.Count} інгредієнтів в одному плоскому списку");

List<Supplier> allSuppliers = recipes
    .SelectMany(r => r.Ingredients)
    .SelectMany(i => i.Suppliers)
    .ToList();
Console.WriteLine($"  Постачальників разом із дублікатами: {allSuppliers.Count}");

// Синтаксис методів: SelectMany + SelectMany + DistinctBy
List<Supplier> uniqueMethod = recipes
    .SelectMany(r => r.Ingredients)
    .SelectMany(i => i.Suppliers)
    .DistinctBy(s => s.Id)
    .OrderBy(s => s.Id)
    .ToList();

// Синтаксис запитів: кілька from підряд компілюється в ті самі SelectMany
List<Supplier> uniqueQuery = (from r in recipes
                              from i in r.Ingredients
                              from s in i.Suppliers
                              select s)
                             .DistinctBy(s => s.Id)
                             .OrderBy(s => s.Id)
                             .ToList();

Console.WriteLine($"  Унікальних постачальників: {uniqueMethod.Count}");
foreach (Supplier s in uniqueMethod)
    Console.WriteLine($"    #{s.Id} {s.Name}");

Console.WriteLine($"  Результати двох синтаксисів збігаються: " +
                  $"{uniqueMethod.Select(s => s.Id).SequenceEqual(uniqueQuery.Select(s => s.Id))}");

// 2. Пагінація: 3 рецепти на сторінку (Skip/Take через Paginate)
PrintHeader("2. Пагінація: 3 рецепти на сторінку");

const int pageSize = 3;
int totalPages = recipes.PageCount(pageSize);

for (int page = 1; page <= totalPages; page++)
{
    Console.WriteLine($"  --- Сторінка {page} з {totalPages} ---");
    foreach (Recipe r in recipes.Paginate(page, pageSize))
        Console.WriteLine($"    [{r.Id,2}] {r.Name}");
}

int outOfRange = recipes.Paginate(totalPages + 1, pageSize).Count();
Console.WriteLine($"  Сторінка {totalPages + 1} (поза діапазоном): елементів {outOfRange}");

// 3. Власний метод розширення HasIngredient для Recipe
PrintHeader("3. Метод розширення HasIngredient(string) для Recipe");

foreach (string ingredient in new[] { "Картопля", "яйця", "  Лосось ", "Трюфель" })
{
    List<string> found = recipes
        .Where(r => r.HasIngredient(ingredient))
        .Select(r => r.Name)
        .ToList();

    string result = found.Count > 0 ? string.Join(", ", found) : "жодного рецепта";
    Console.WriteLine($"  \"{ingredient.Trim()}\" → {found.Count}: {result}");
}

Recipe borscht = recipes[0];
Console.WriteLine($"  Прямий виклик: {borscht.Name}.HasIngredient(\"Буряк\") = {borscht.HasIngredient("Буряк")}");
Console.WriteLine($"  Прямий виклик: {borscht.Name}.HasIngredient(\"Мед\")   = {borscht.HasIngredient("Мед")}");

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 70));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 70));
}
