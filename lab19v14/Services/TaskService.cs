using lab19v14.Common;
using lab19v14.Exceptions;
using lab19v14.Models;

namespace lab19v14.Services;

/// <summary>Сервіс керування задачами (варіант 14).</summary>
public class TaskService
{
    private readonly Dictionary<int, TaskItem> _tasks = new();

    public TaskService()
    {
        Add(new TaskItem(1, "Підготувати звіт"));
        Add(new TaskItem(2, "Оновити документацію", isCompleted: true, assignee: "Марія"));
        Add(new TaskItem(3, "Виправити помилку #42", assignee: "Олег"));
        Add(new TaskItem(4, "Код-рев'ю модуля оплати"));
        Add(new TaskItem(5, "Розгорнути реліз 1.2", isCompleted: true, assignee: "Олег"));
    }

    public IEnumerable<TaskItem> Tasks => _tasks.Values.OrderBy(t => t.Id);

    private void Add(TaskItem task) => _tasks[task.Id] = task;

    /// <summary>
    /// Позначає задачу виконаною. Спроба виконати вже виконану задачу означає порушення
    /// послідовності операцій у коді-виклику, тому метод кидає TaskAlreadyCompletedException.
    /// </summary>
    /// <exception cref="TaskAlreadyCompletedException">Задача вже виконана.</exception>
    /// <exception cref="KeyNotFoundException">Задачі з таким Id не існує.</exception>
    public void CompleteTask(int taskId)
    {
        if (!_tasks.TryGetValue(taskId, out TaskItem? task))
            throw new KeyNotFoundException($"Завдання #{taskId} не знайдено.");

        if (task.IsCompleted)
            throw new TaskAlreadyCompletedException(taskId);

        task.IsCompleted = true;
    }

    /// <summary>
    /// Призначає виконавця. Відсутність задачі або порожнє ім'я - очікувані ситуації
    /// (наприклад, помилка вводу користувача), тому замість винятку повертається Result.
    /// </summary>
    public Result<bool> AssignTask(int taskId, string assignee)
    {
        if (!_tasks.TryGetValue(taskId, out TaskItem? task))
            return Result<bool>.Failure($"Завдання #{taskId} не знайдено.");

        if (string.IsNullOrWhiteSpace(assignee))
            return Result<bool>.Failure("Ім'я виконавця не може бути порожнім.");

        task.Assignee = assignee.Trim();
        return Result<bool>.Success(true);
    }

    /// <summary>
    /// Безвинятковий варіант CompleteTask: ловить очікувані винятки й перетворює їх
    /// на Result через Result&lt;T&gt;.Failure(Exception).
    /// </summary>
    public Result<bool> TryCompleteTask(int taskId)
    {
        try
        {
            CompleteTask(taskId);
            return Result<bool>.Success(true);
        }
        catch (Exception ex) when (ex is TaskAlreadyCompletedException or KeyNotFoundException)
        {
            return Result<bool>.Failure(ex);
        }
    }
}
