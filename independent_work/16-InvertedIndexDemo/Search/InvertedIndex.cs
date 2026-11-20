using InvertedIndexDemo.Models;

namespace InvertedIndexDemo.Search;

/// <summary>Інвертований індекс: слово -> множина ідентифікаторів документів.</summary>
public class InvertedIndex
{
    // Слово -> множина ID документів
    private readonly Dictionary<string, HashSet<int>> _index;

    // ID -> документ (для швидкого отримання)
    private readonly Dictionary<int, Document> _documents;

    public InvertedIndex()
    {
        _index = new Dictionary<string, HashSet<int>>();
        _documents = new Dictionary<int, Document>();
    }

    public int DocumentCount => _documents.Count;
    public int UniqueWords => _index.Keys.Count;

    /// <summary>Додає документ до індексу.</summary>
    public void AddDocument(Document doc)
    {
        ArgumentNullException.ThrowIfNull(doc);

        // Якщо документ із таким Id вже є, спершу прибираємо його старі входження.
        if (_documents.ContainsKey(doc.Id))
            RemoveFromIndex(doc.Id);

        _documents[doc.Id] = doc;

        foreach (string word in TextProcessor.Normalize(doc.Content))
        {
            if (!_index.TryGetValue(word, out HashSet<int>? ids))
            {
                ids = new HashSet<int>();
                _index[word] = ids;
            }

            ids.Add(doc.Id); // HashSet ігнорує повтори: слово, що зустрілося двічі, дає один запис
        }
    }

    /// <summary>Шукає документи, що містять слово.</summary>
    public List<Document> SearchSingleWord(string word)
    {
        string? normalized = NormalizeQueryWord(word);
        if (normalized is null || !_index.TryGetValue(normalized, out HashSet<int>? ids))
            return new List<Document>();

        return ToDocuments(ids);
    }

    /// <summary>Шукає документи, що містять ВСІ слова (AND) через HashSet.IntersectWith.</summary>
    public List<Document> SearchMultipleWords(params string[] words)
    {
        List<string> terms = NormalizeQuery(words);
        if (terms.Count == 0)
            return new List<Document>();

        HashSet<int>? result = null;

        foreach (string term in terms)
        {
            // Якщо хоч одного слова немає в індексі, то AND-результат порожній.
            if (!_index.TryGetValue(term, out HashSet<int>? ids))
                return new List<Document>();

            if (result is null)
                result = new HashSet<int>(ids);   // копія, щоб не змінити сам індекс
            else
                result.IntersectWith(ids);

            if (result.Count == 0)
                break;
        }

        return ToDocuments(result!);
    }

    /// <summary>Шукає документи, що містять БУДЬ-ЯКЕ слово (OR) через HashSet.UnionWith.</summary>
    public List<Document> SearchAnyWord(params string[] words)
    {
        var result = new HashSet<int>();

        foreach (string term in NormalizeQuery(words))
        {
            if (_index.TryGetValue(term, out HashSet<int>? ids))
                result.UnionWith(ids);
        }

        return ToDocuments(result);
    }

    // ---------------------------------------------------------------- допоміжні методи

    /// <summary>
    /// Нормалізує слово запиту тими ж правилами, що й текст документів.
    /// Повертає null, якщо слово складається лише зі стоп-слів/пунктуації.
    /// </summary>
    private static string? NormalizeQueryWord(string word) =>
        TextProcessor.Normalize(word).FirstOrDefault();

    private static List<string> NormalizeQuery(string[] words) =>
        words.SelectMany(TextProcessor.Normalize).Distinct().ToList();

    private List<Document> ToDocuments(IEnumerable<int> ids) =>
        ids.OrderBy(id => id).Select(id => _documents[id]).ToList();

    private void RemoveFromIndex(int docId)
    {
        var emptyWords = new List<string>();

        foreach (var (word, ids) in _index)
        {
            if (ids.Remove(docId) && ids.Count == 0)
                emptyWords.Add(word);
        }

        foreach (string word in emptyWords)
            _index.Remove(word);
    }
}
