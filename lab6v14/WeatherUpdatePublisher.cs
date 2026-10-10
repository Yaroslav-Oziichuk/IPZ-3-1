namespace Lab10v14;

public class WeatherUpdatePublisher : EventPublisher
{
    public override string Name => "WeatherUpdate";

    public override void Subscribe(IObserver<string> observer)
    {
        if (!Observers.Contains(observer))
        {
            Observers.Add(observer);
        }
    }

    public override void Unsubscribe(IObserver<string> observer) => Observers.Remove(observer);

    public void UpdateWeather(string city, double temperature) =>
        NotifySubscribers($"{city}: {temperature:F1}");
}
