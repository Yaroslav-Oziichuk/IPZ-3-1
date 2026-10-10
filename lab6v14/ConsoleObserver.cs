namespace Lab10v14;

public class ConsoleObserver : IObserver<int>, IObserver<string>
{
    public string Name { get; }

    public ConsoleObserver(string name) => Name = name;

    public void Update(int value) =>
        Console.WriteLine($"  [Console:{Name}] Отримано число: {value}");

    public void Update(string value) =>
        Console.WriteLine($"  [Console:{Name}] Отримано подію: {value}");
}
