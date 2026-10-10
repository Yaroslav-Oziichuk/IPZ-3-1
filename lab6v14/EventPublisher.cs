namespace Lab10v14;

public abstract class EventPublisher
{
    protected readonly List<IObserver<string>> Observers = new();

    public abstract string Name { get; }

    public abstract void Subscribe(IObserver<string> observer);

    public abstract void Unsubscribe(IObserver<string> observer);

    public void NotifySubscribers(string eventData)
    {
        Console.WriteLine($"[{Name}] Розсилка {Observers.Count} підписникам: {eventData}");
        foreach (var observer in Observers.ToList())
        {
            observer.Update(eventData);
        }
    }
}
