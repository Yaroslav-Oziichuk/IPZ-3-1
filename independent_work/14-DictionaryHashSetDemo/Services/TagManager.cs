namespace DictionaryHashSetDemo.Services;

/// <summary>
/// Керування тегами статей блогу: ArticleId -> множина тегів.
/// HashSet гарантує унікальність тегів статті та дає швидкі теоретико-множинні операції.
/// Теги нечутливі до регістру: "C#" і "c#" - один тег.
/// </summary>
public class TagManager
{
    private readonly Dictionary<string, HashSet<string>> _articleTags = new();

    public int ArticleCount => _articleTags.Count;

    /// <summary>Додає тег до статті. Повертає false, якщо такий тег у статті вже є.</summary>
    public bool AddTag(string articleId, string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(articleId);
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        if (!_articleTags.TryGetValue(articleId, out HashSet<string>? tags))
        {
            tags = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _articleTags[articleId] = tags;
        }

        return tags.Add(tag.Trim());
    }

    /// <summary>Видаляє тег зі статті. Повертає true, якщо тег було видалено.</summary>
    public bool RemoveTag(string articleId, string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(articleId);
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);

        return _articleTags.TryGetValue(articleId, out HashSet<string>? tags) && tags.Remove(tag.Trim());
    }

    /// <summary>Теги однієї статті (копія; порожня множина, якщо статті немає).</summary>
    public HashSet<string> GetTags(string articleId) =>
        _articleTags.TryGetValue(articleId, out HashSet<string>? tags)
            ? new HashSet<string>(tags, tags.Comparer)
            : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>Усі унікальні теги по всіх статтях (UnionWith).</summary>
    public HashSet<string> GetAllUniqueTags()
    {
        var all = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (HashSet<string> tags in _articleTags.Values)
            all.UnionWith(tags);

        return all;
    }

    /// <summary>Спільні теги двох статей (IntersectWith).</summary>
    public HashSet<string> FindCommonTags(string articleId1, string articleId2)
    {
        HashSet<string> result = GetTags(articleId1);   // копія, оригінал не змінюється
        result.IntersectWith(GetTags(articleId2));
        return result;
    }

    /// <summary>Теги, які є в першій статті, але відсутні в другій (ExceptWith).</summary>
    public HashSet<string> FindTagsOnlyInFirst(string articleId1, string articleId2)
    {
        HashSet<string> result = GetTags(articleId1);
        result.ExceptWith(GetTags(articleId2));
        return result;
    }
}
