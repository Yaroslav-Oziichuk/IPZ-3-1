namespace lab17v14.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> source, int page, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "Номер сторінки починається з 1.");
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Розмір сторінки має бути додатним.");

        return source.Skip((page - 1) * pageSize).Take(pageSize);
    }

    public static int PageCount<T>(this IEnumerable<T> source, int pageSize)
    {
        ArgumentNullException.ThrowIfNull(source);
        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Розмір сторінки має бути додатним.");

        return (source.Count() + pageSize - 1) / pageSize;
    }
}
