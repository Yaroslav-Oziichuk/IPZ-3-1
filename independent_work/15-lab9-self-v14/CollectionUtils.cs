namespace Lab9SelfV14;

public static class CollectionUtils
{
    public static IEnumerable<TResult> Map<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(mapper);

        return MapIterator(collection, mapper);
    }

    private static IEnumerable<TResult> MapIterator<TSource, TResult>(
        IEnumerable<TSource> collection,
        Func<TSource, TResult> mapper)
    {
        foreach (TSource item in collection)
            yield return mapper(item);
    }

    public static T MaxBy<T, TKey>(
        IEnumerable<T> collection,
        Func<T, TKey> keySelector)
        where TKey : IComparable<TKey>
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(keySelector);

        T maxItem = default!;
        TKey maxKey = default!;
        bool first = true;

        foreach (T item in collection)
        {
            TKey key = keySelector(item);

            if (first || key.CompareTo(maxKey) > 0)
            {
                maxKey = key;
                maxItem = item;
                first = false;
            }
        }

        if (first)
            throw new InvalidOperationException("Неможливо знайти максимум у порожній колекції.");

        return maxItem;
    }

    public static IEnumerable<T> Filter<T>(
        IEnumerable<T> collection,
        Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(predicate);

        return FilterIterator(collection, predicate);
    }

    private static IEnumerable<T> FilterIterator<T>(IEnumerable<T> collection, Predicate<T> predicate)
    {
        foreach (T item in collection)
        {
            if (predicate(item))
                yield return item;
        }
    }

    public static void ForEach<T>(
        IEnumerable<T> collection,
        Action<T> action)
    {
        ArgumentNullException.ThrowIfNull(collection);
        ArgumentNullException.ThrowIfNull(action);

        foreach (T item in collection)
            action(item);
    }
}
