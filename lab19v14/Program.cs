using System.Text;
using lab19v14.Common;
using lab19v14.Exceptions;
using lab19v14.Services;

Console.OutputEncoding = Encoding.UTF8;

var service = new TaskService();

PrintHeader("0. Початковий стан задач");
PrintTasks(service);

// ---------------------------------------------------------------------------
PrintHeader("1. Власний виняток: CompleteTask (try-catch)");
// ---------------------------------------------------------------------------
foreach (int id in new[] { 1, 2, 99 })
{
    try
    {
        service.CompleteTask(id);
        Console.WriteLine($"  CompleteTask({id}): задачу виконано.");
    }
    catch (TaskAlreadyCompletedException ex)
    {
        Console.WriteLine($"  CompleteTask({id}): TaskAlreadyCompletedException, TaskId = {ex.TaskId}");
        Console.WriteLine($"    Повідомлення: {ex.Message}");
    }
    catch (KeyNotFoundException ex)
    {
        Console.WriteLine($"  CompleteTask({id}): KeyNotFoundException");
        Console.WriteLine($"    Повідомлення: {ex.Message}");
    }
}

// ---------------------------------------------------------------------------
PrintHeader("2. Result<bool>: AssignTask (IsSuccess / Error)");
// ---------------------------------------------------------------------------
PrintResult("AssignTask(3, \"Олена\")", service.AssignTask(3, "Олена"));
PrintResult("AssignTask(99, \"Олена\")", service.AssignTask(99, "Олена"));
PrintResult("AssignTask(1, \"   \")", service.AssignTask(1, "   "));

// ---------------------------------------------------------------------------
PrintHeader("3. Result.Failure(Exception): TryCompleteTask");
// ---------------------------------------------------------------------------
PrintResult("TryCompleteTask(4)", service.TryCompleteTask(4));
PrintResult("TryCompleteTask(5)", service.TryCompleteTask(5));
PrintResult("TryCompleteTask(100)", service.TryCompleteTask(100));

PrintHeader("4. Підсумковий стан задач");
PrintTasks(service);

static void PrintResult(string call, Result<bool> result)
{
    if (result.IsSuccess)
        Console.WriteLine($"  {call}: успіх, Value = {result.Value}");
    else
        Console.WriteLine($"  {call}: помилка, Error = {result.Error}");
}

static void PrintTasks(TaskService service)
{
    foreach (var task in service.Tasks)
        Console.WriteLine($"  {task}");
}

static void PrintHeader(string title)
{
    Console.WriteLine();
    Console.WriteLine(new string('=', 78));
    Console.WriteLine(title);
    Console.WriteLine(new string('=', 78));
}
