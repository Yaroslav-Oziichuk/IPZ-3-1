using System.Text.RegularExpressions;

namespace InvertedIndexDemo.Search;

/// <summary>
/// Обробка тексту. Усі способи пошуку (лінійний та через індекс) повинні користуватися
/// саме цими методами, щоб нормалізація була однаковою.
/// </summary>
public static class TextProcessor
{
    // Слово = послідовність літер/цифр, за якою можуть іти символи '#' або '+'.
    // Так «C#» і «C++» лишаються окремими термінами, а не перетворюються на «c».
    private static readonly Regex WordRegex = new(@"[\p{L}\p{N}]+[#+]*", RegexOptions.Compiled);

    private static readonly HashSet<string> StopWords = new()
    {
        "a", "an", "the", "is", "are", "was", "were",
        "in", "on", "at", "to", "for", "of", "and", "or"
    };

    /// <summary>
    /// Розбиває текст на слова, видаляє пунктуацію, переводить у нижній регістр.
    /// Приклад: "Hello, world!" -> ["hello", "world"].
    /// </summary>
    public static IEnumerable<string> Tokenize(string text)
    {
        if (string.IsNullOrEmpty(text))
            yield break;

        foreach (Match match in WordRegex.Matches(text.ToLowerInvariant()))
            yield return match.Value;
    }

    /// <summary>Видаляє стоп-слова (a, an, the, is, are, etc.).</summary>
    public static IEnumerable<string> RemoveStopWords(IEnumerable<string> words)
    {
        return words.Where(w => !StopWords.Contains(w));
    }

    /// <summary>
    /// Повна нормалізація: токенізація + видалення стоп-слів.
    /// Єдина точка входу для індексу, запитів і лінійного пошуку.
    /// </summary>
    public static IEnumerable<string> Normalize(string text) => RemoveStopWords(Tokenize(text));
}
