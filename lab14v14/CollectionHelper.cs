namespace lab14v14;

public static class CollectionHelper<T>
{
    public static IEnumerable<T> Take(IEnumerable<T> collection, int count)
    {
        if (collection == null)
            throw new ArgumentNullException(nameof(collection), "Колекція не може бути null.");

        if (count <= 0)
            yield break;

        int currentCount = 0;
        foreach (var item in collection)
        {
            yield return item;
            currentCount++;
            
            if (currentCount >= count)
                break;
        }
    }
}