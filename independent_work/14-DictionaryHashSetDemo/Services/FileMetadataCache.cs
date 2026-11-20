using DictionaryHashSetDemo.Models;

namespace DictionaryHashSetDemo.Services;

/// <summary>
/// Кеш метаданих файлів. Ключ - ім'я файлу.
/// Dictionary дає пошук за ключем за O(1) в середньому, на відміну від List, де потрібен O(n) перебір.
/// Порівняння імен регістронезалежне (як шляхи у Windows): "Report.PDF" і "report.pdf" - це один файл.
/// </summary>
public class FileMetadataCache
{
    private readonly Dictionary<string, FileMetadata> _cache =
        new(StringComparer.OrdinalIgnoreCase);

    public int Count => _cache.Count;

    /// <summary>Додає метадані. Повертає false, якщо файл із таким іменем уже є (старі дані не перезаписуються).</summary>
    public bool Add(FileMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        return _cache.TryAdd(metadata.FileName, metadata);
    }

    /// <summary>Додає метадані або оновлює наявні (upsert).</summary>
    public void AddOrUpdate(FileMetadata metadata)
    {
        ArgumentNullException.ThrowIfNull(metadata);
        _cache[metadata.FileName] = metadata;
    }

    /// <summary>Повертає метадані або null, якщо файлу немає в кеші.</summary>
    public FileMetadata? Get(string fileName) =>
        _cache.TryGetValue(fileName, out FileMetadata? metadata) ? metadata : null;

    /// <summary>Видаляє запис. Повертає true, якщо запис існував.</summary>
    public bool Remove(string fileName) => _cache.Remove(fileName);

    /// <summary>Перевіряє наявність файлу в кеші.</summary>
    public bool Contains(string fileName) => _cache.ContainsKey(fileName);

    /// <summary>Усі записи, впорядковані за іменем.</summary>
    public IEnumerable<FileMetadata> GetAll() => _cache.Values.OrderBy(m => m.FileName, StringComparer.OrdinalIgnoreCase);
}
