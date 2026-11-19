namespace lab18v14.Services;

public class FileProcessor
{
    private readonly int _failuresBeforeSuccess;
    private int _calls;

    public FileProcessor(int failuresBeforeSuccess = 3)
    {
        _failuresBeforeSuccess = failuresBeforeSuccess;
    }

    public int Calls => _calls;

    public void UpdateUserProfile(string path, string profileJson)
    {
        _calls++;

        if (_calls <= _failuresBeforeSuccess)
        {
            throw new FileNotFoundException(
                $"Файл профілю не знайдено (імітація тимчасової помилки, виклик №{_calls})", path);
        }

        Console.WriteLine($"    [FileProcessor] Профіль збережено у \"{path}\" ({profileJson.Length} символів)");
    }
}
