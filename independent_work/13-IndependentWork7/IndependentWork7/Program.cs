using System.Text;
using IndependentWork7;

Console.OutputEncoding = Encoding.UTF8;

// ---------------------------------------------------------------------------
Console.WriteLine("=== Cache<string, string>, ємність 3 ===");
// ---------------------------------------------------------------------------
var cities = new Cache<string, string>(3);

cities.Add("UA", "Україна");
cities.Add("PL", "Польща");
cities.Add("DE", "Німеччина");
Console.WriteLine($"Додано 3 елементи, Count = {cities.Count}");
Console.WriteLine($"Get(\"PL\") = {cities.Get("PL")}");
Console.WriteLine($"ContainsKey(\"UA\") = {cities.ContainsKey("UA")}");

Console.WriteLine("Додаємо четвертий елемент (FR), кеш переповнений:");
cities.Add("FR", "Франція");
Console.WriteLine($"  Count = {cities.Count}");
Console.WriteLine($"  ContainsKey(\"UA\") = {cities.ContainsKey("UA")}  <- найстаріший елемент видалено");
Console.WriteLine($"  ContainsKey(\"FR\") = {cities.ContainsKey("FR")}");

Console.WriteLine("Додаємо п'ятий елемент (IT):");
cities.Add("IT", "Італія");
Console.WriteLine($"  ContainsKey(\"PL\") = {cities.ContainsKey("PL")}  <- Get не змінює порядок видалення (це FIFO, а не LRU)");

try
{
    cities.Get("UA");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"Get(\"UA\") -> KeyNotFoundException: {ex.Message}");
}

// ---------------------------------------------------------------------------
Console.WriteLine();
Console.WriteLine("=== Cache<int, UserProfile>, ємність 2 ===");
// ---------------------------------------------------------------------------
var users = new Cache<int, UserProfile>(2);
users.Add(1, new UserProfile(1, "Олена"));
users.Add(2, new UserProfile(2, "Андрій"));
Console.WriteLine($"Get(1) = {users.Get(1)}");

users.Add(2, new UserProfile(2, "Андрій (оновлено)"));
Console.WriteLine($"Після оновлення ключа 2: Count = {users.Count}, Get(2) = {users.Get(2)}");

users.Add(3, new UserProfile(3, "Марія"));
Console.WriteLine($"Після додавання ключа 3: ContainsKey(1) = {users.ContainsKey(1)}, " +
                  $"ContainsKey(2) = {users.ContainsKey(2)}, ContainsKey(3) = {users.ContainsKey(3)}");

public record UserProfile(int Id, string Name);
