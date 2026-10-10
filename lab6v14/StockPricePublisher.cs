namespace Lab10v14;

public class StockPricePublisher : EventPublisher
{
    public override string Name => "StockPrice";

    public override void Subscribe(IObserver<string> observer) => Observers.Add(observer);

    public override void Unsubscribe(IObserver<string> observer) => Observers.Remove(observer);

    public void SetPrice(string ticker, decimal price) =>
        NotifySubscribers($"{ticker}: {price:F2} USD");
}
