namespace IndependentWork7;

public class Cache<TKey, TValue> where TKey : notnull
{
    private readonly int _capacity;

    private readonly Dictionary<TKey, TValue> _cache;

    private readonly Queue<TKey> _keysOrder;

    public Cache(int capacity)
    {
        if (capacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(capacity), capacity, "Ємність кешу має бути додатною.");

        _capacity = capacity;
        _cache = new Dictionary<TKey, TValue>(capacity);
        _keysOrder = new Queue<TKey>(capacity);
    }

    public int Count => _cache.Count;

    public int Capacity => _capacity;

    public void Add(TKey key, TValue value)
    {
        if (_cache.ContainsKey(key))
        {
            _cache[key] = value;
            return;
        }

        if (_cache.Count >= _capacity)
        {
            TKey oldest = _keysOrder.Dequeue();
            _cache.Remove(oldest);
        }

        _cache[key] = value;
        _keysOrder.Enqueue(key);
    }

    public TValue Get(TKey key)
    {
        if (!_cache.TryGetValue(key, out TValue? value))
            throw new KeyNotFoundException($"Ключ '{key}' не знайдено в кеші.");

        return value;
    }

    public bool ContainsKey(TKey key) => _cache.ContainsKey(key);
}
