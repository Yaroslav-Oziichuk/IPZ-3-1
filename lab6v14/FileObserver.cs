namespace Lab10v14;

public class FileObserver : IObserver<int>, IObserver<string>
{
    private readonly string _fileName;

    public FileObserver(string fileName) => _fileName = fileName;

    public void Update(int value) =>
        Console.WriteLine($"  [File:{_fileName}] Число {value} записано у файл");

    public void Update(string value) =>
        Console.WriteLine($"  [File:{_fileName}] Подію \"{value}\" записано у файл");
}
