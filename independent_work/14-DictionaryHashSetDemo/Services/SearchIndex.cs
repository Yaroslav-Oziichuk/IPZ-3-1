using System.Text.RegularExpressions;

namespace DictionaryHashSetDemo.Services;

/// <summary>Простий пошуковий індекс: слово -> множина документів, де воно зустрічається.</summary>
public class SearchIndex
{
    private static readonly Regex WordRegex = new(@"[\p{L}\p{N}]+[#+]*", RegexOptions.Compiled);

    private readonly Dictionary<string, HashSet<int>> _invertedIndex = new();

    public int UniqueWords => _invertedIndex.Count;

    /// <summary>Розбиває текст на слова та додає документ до індексу.</summary>
    public void AddDocument(int docId, string content)
    {
        ArgumentNullException.ThrowIfNull(content);

        foreach (Match match in WordRegex.Matches(content.ToLowerInvariant()))
        {
            string word = match.Value;

            if (!_invertedIndex.TryGetValue(word, out HashSet<int>? docs))
            {
                docs = new HashSet<int>();
                _invertedIndex[word] = docs;
            }

            docs.Add(docId);
        }
    }

    /// <summary>Усі документи з цим словом (порожня множина, якщо слова немає).</summary>
    public HashSet<int> Search(string word)
    {
        string normalized = word.Trim().ToLowerInvariant();

        return _invertedIndex.TryGetValue(normalized, out HashSet<int>? docs)
            ? new HashSet<int>(docs)   // копія: зовнішній код не зіпсує індекс
            : new HashSet<int>();
    }

    /// <summary>Документи, що містять ВСІ слова (перетин множин).</summary>
    public HashSet<int> SearchMultiple(params string[] words)
    {
        if (words.Length == 0)
            return new HashSet<int>();

        HashSet<int> result = Search(words[0]);

        foreach (string word in words.Skip(1))
        {
            if (result.Count == 0)
                break;

            result.IntersectWith(Search(word));
        }

        return result;
    }
}
