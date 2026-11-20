using IndependentWork7;

namespace IndependentWork7.Tests;

public class CacheTests
{
    // ---------------------------------------------------------------- конструктор

    [Fact]
    public void Constructor_ZeroCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(0));
    }

    [Fact]
    public void Constructor_NegativeCapacity_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Cache<int, string>(-5));
    }

    // ---------------------------------------------------------------- Add

    [Fact]
    public void Add_NewItem_ItemIsStored()
    {
        var cache = new Cache<string, int>(3);

        cache.Add("one", 1);

        Assert.True(cache.ContainsKey("one"));
        Assert.Equal(1, cache.Get("one"));
    }

    [Fact]
    public void Add_SeveralItemsWithinCapacity_AllItemsAreStored()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        Assert.Equal("a", cache.Get(1));
        Assert.Equal("b", cache.Get(2));
        Assert.Equal("c", cache.Get(3));
    }

    [Fact]
    public void Add_ExistingKey_UpdatesValueWithoutChangingCount()
    {
        var cache = new Cache<string, int>(3);
        cache.Add("key", 1);

        cache.Add("key", 2);

        Assert.Equal(2, cache.Get("key"));
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void Add_ExistingKey_DoesNotRefreshItsPositionInFifoOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "first");
        cache.Add(2, "second");

        cache.Add(1, "first-updated"); // ключ 1 залишається найстарішим
        cache.Add(3, "third");         // має витіснити ключ 1, а не 2

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
    }

    // ---------------------------------------------------------------- Get

    [Fact]
    public void Get_ExistingKey_ReturnsStoredValue()
    {
        var cache = new Cache<string, string>(2);
        cache.Add("UA", "Україна");

        string value = cache.Get("UA");

        Assert.Equal("Україна", value);
    }

    [Fact]
    public void Get_MissingKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("a", 1);

        Assert.Throws<KeyNotFoundException>(() => cache.Get("missing"));
    }

    [Fact]
    public void Get_OnEmptyCache_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<int, int>(1);

        Assert.Throws<KeyNotFoundException>(() => cache.Get(42));
    }

    [Fact]
    public void Get_EvictedKey_ThrowsKeyNotFoundException()
    {
        var cache = new Cache<int, string>(1);
        cache.Add(1, "old");
        cache.Add(2, "new");

        Assert.Throws<KeyNotFoundException>(() => cache.Get(1));
    }

    // ---------------------------------------------------------------- ContainsKey

    [Fact]
    public void ContainsKey_ExistingKey_ReturnsTrue()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("a", 1);

        Assert.True(cache.ContainsKey("a"));
    }

    [Fact]
    public void ContainsKey_MissingKey_ReturnsFalse()
    {
        var cache = new Cache<string, int>(2);
        cache.Add("a", 1);

        Assert.False(cache.ContainsKey("b"));
    }

    [Fact]
    public void ContainsKey_OnEmptyCache_ReturnsFalse()
    {
        var cache = new Cache<string, int>(2);

        Assert.False(cache.ContainsKey("a"));
    }

    [Fact]
    public void ContainsKey_AfterEviction_ReturnsFalseForEvictedKey()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        Assert.False(cache.ContainsKey(1));
    }

    // ---------------------------------------------------------------- Count

    [Fact]
    public void Count_NewCache_IsZero()
    {
        var cache = new Cache<int, int>(5);

        Assert.Equal(0, cache.Count);
    }

    [Fact]
    public void Count_AfterAddingItems_EqualsNumberOfItems()
    {
        var cache = new Cache<int, int>(5);

        cache.Add(1, 10);
        cache.Add(2, 20);

        Assert.Equal(2, cache.Count);
    }

    [Fact]
    public void Count_WhenCapacityExceeded_NeverExceedsCapacity()
    {
        var cache = new Cache<int, int>(3);

        for (int i = 0; i < 10; i++)
            cache.Add(i, i * 10);

        Assert.Equal(3, cache.Count);
    }

    // ---------------------------------------------------------------- політика FIFO

    [Fact]
    public void Add_WhenCacheIsFull_RemovesOldestItem()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        cache.Add(4, "d");

        Assert.False(cache.ContainsKey(1));
        Assert.Equal(3, cache.Count);
    }

    [Fact]
    public void Add_WhenCacheIsFull_KeepsNewerItemsAndAddsNewOne()
    {
        var cache = new Cache<int, string>(3);
        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        cache.Add(4, "d");

        Assert.Equal("b", cache.Get(2));
        Assert.Equal("c", cache.Get(3));
        Assert.Equal("d", cache.Get(4));
    }

    [Fact]
    public void Add_RepeatedOverflow_EvictsItemsInInsertionOrder()
    {
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Add(3, "c"); // видаляє 1
        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));

        cache.Add(4, "d"); // видаляє 2
        Assert.False(cache.ContainsKey(2));
        Assert.True(cache.ContainsKey(3));
        Assert.True(cache.ContainsKey(4));
    }

    [Fact]
    public void Add_CapacityOne_EachNewItemReplacesPreviousOne()
    {
        var cache = new Cache<string, int>(1);

        cache.Add("a", 1);
        cache.Add("b", 2);

        Assert.False(cache.ContainsKey("a"));
        Assert.Equal(2, cache.Get("b"));
        Assert.Equal(1, cache.Count);
    }

    [Fact]
    public void Get_DoesNotAffectEvictionOrder()
    {
        // FIFO, а не LRU: читання елемента не «омолоджує» його.
        var cache = new Cache<int, string>(2);
        cache.Add(1, "a");
        cache.Add(2, "b");

        cache.Get(1);
        cache.Add(3, "c");

        Assert.False(cache.ContainsKey(1));
        Assert.True(cache.ContainsKey(2));
    }

    [Fact]
    public void Add_ExactlyAtCapacity_DoesNotEvictAnything()
    {
        var cache = new Cache<int, string>(3);

        cache.Add(1, "a");
        cache.Add(2, "b");
        cache.Add(3, "c");

        Assert.Equal(3, cache.Count);
        Assert.True(cache.ContainsKey(1));
    }
}
