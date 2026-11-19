namespace lab18v14.Services;

public class NetworkClient
{
    private readonly int _failuresBeforeSuccess;
    private int _calls;

    public NetworkClient(int failuresBeforeSuccess = 2)
    {
        _failuresBeforeSuccess = failuresBeforeSuccess;
    }

    public int Calls => _calls;

    public bool PostUserProfile(string url, string profileJson)
    {
        _calls++;

        if (_calls <= _failuresBeforeSuccess)
        {
            throw new HttpRequestException(
                $"Не вдалося надіслати профіль на {url} (імітація тимчасової помилки, виклик №{_calls})");
        }

        Console.WriteLine($"    [NetworkClient] Профіль надіслано на {url} ({profileJson.Length} символів)");
        return true;
    }
}
