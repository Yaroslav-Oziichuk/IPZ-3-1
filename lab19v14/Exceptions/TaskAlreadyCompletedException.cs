using System.Runtime.Serialization;

namespace lab19v14.Exceptions;

[Serializable]
public class TaskAlreadyCompletedException : Exception
{
    private const string DefaultMessage = "Завдання вже виконано.";

    public int TaskId { get; }


    public TaskAlreadyCompletedException() : base(DefaultMessage)
    {
    }

    public TaskAlreadyCompletedException(string message) : base(message)
    {
    }

    public TaskAlreadyCompletedException(string message, Exception innerException)
        : base(message, innerException)
    {
    }


    public TaskAlreadyCompletedException(int taskId)
        : base($"Завдання #{taskId} вже виконано.")
    {
        TaskId = taskId;
    }

    public TaskAlreadyCompletedException(int taskId, Exception innerException)
        : base($"Завдання #{taskId} вже виконано.", innerException)
    {
        TaskId = taskId;
    }

#pragma warning disable SYSLIB0051, CS0672
    protected TaskAlreadyCompletedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        TaskId = info.GetInt32(nameof(TaskId));
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        base.GetObjectData(info, context);
        info.AddValue(nameof(TaskId), TaskId);
    }
#pragma warning restore SYSLIB0051, CS0672
}
