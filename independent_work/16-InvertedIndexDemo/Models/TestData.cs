namespace InvertedIndexDemo.Models;

public static class TestData
{
    public static List<Document> CreateBaseDocuments() => new()
    {
        new Document
        {
            Id = 1,
            Title = "Introduction to C#",
            Content = "C# is a modern object-oriented programming language developed by Microsoft."
        },
        new Document
        {
            Id = 2,
            Title = "Java Basics",
            Content = "Java is a popular programming language known for its portability and performance."
        },
        new Document
        {
            Id = 3,
            Title = "Python Overview",
            Content = "Python is a high-level programming language with simple syntax."
        },
        new Document
        {
            Id = 4,
            Title = "C# Advanced Features",
            Content = "C# supports LINQ, async/await, and many modern programming paradigms."
        },
        new Document
        {
            Id = 5,
            Title = "Comparing Languages",
            Content = "C#, Java, and Python are among the most popular programming languages today."
        }
    };

    public static List<Document> Generate(int count, int seed = 42)
    {
        var baseDocs = CreateBaseDocuments();
        var random = new Random(seed);
        var result = new List<Document>(count);

        for (int i = 1; i <= count; i++)
        {
            Document source = baseDocs[(i - 1) % baseDocs.Count];
            var extraWords = Enumerable.Range(0, 5).Select(_ => $"term{random.Next(1000)}");

            result.Add(new Document
            {
                Id = i,
                Title = $"{source.Title} #{i}",
                Content = source.Content + " " + string.Join(' ', extraWords)
            });
        }

        return result;
    }
}
