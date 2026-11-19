namespace lab19v14.Models;

public class TaskItem
{
    public int Id { get; }
    public string Title { get; }
    public bool IsCompleted { get; set; }
    public string? Assignee { get; set; }

    public TaskItem(int id, string title, bool isCompleted = false, string? assignee = null)
    {
        Id = id;
        Title = title;
        IsCompleted = isCompleted;
        Assignee = assignee;
    }

    public override string ToString()
    {
        string status = IsCompleted ? "виконано" : "в роботі";
        string assignee = Assignee ?? "не призначено";
        return $"#{Id} {Title,-26} | {status,-8} | виконавець: {assignee}";
    }
}
