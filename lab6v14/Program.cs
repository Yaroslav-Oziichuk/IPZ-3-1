using System.Text;

namespace Lab10v14;

public static class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var consoleObserver = new ConsoleObserver("Головний");
        var fileObserver = new FileObserver("events.log");

        Console.WriteLine("Колекція IObserver<int>");
        List<IObserver<int>> intObservers = new() { consoleObserver, fileObserver };

        foreach (var number in new[] { 10, 25 })
        {
            Console.WriteLine($"Передаємо значення {number}:");
            foreach (var observer in intObservers)
            {
                observer.Update(number);
            }
        }

        Console.WriteLine();
        Console.WriteLine("Колекція EventPublisher");
        var stock = new StockPricePublisher();
        var weather = new WeatherUpdatePublisher();
        List<EventPublisher> publishers = new() { stock, weather };

        foreach (var publisher in publishers)
        {
            publisher.Subscribe(consoleObserver);
            publisher.Subscribe(fileObserver);
            publisher.Subscribe(consoleObserver);
        }

        foreach (var publisher in publishers)
        {
            publisher.NotifySubscribers("Тестова подія");
        }

        Console.WriteLine();
        Console.WriteLine("Специфічні події нащадків");
        stock.SetPrice("AAPL", 189.5m);
        weather.UpdateWeather("Рівне", 14.3);

        Console.WriteLine();
        Console.WriteLine("Відписка");
        foreach (var publisher in publishers)
        {
            publisher.Unsubscribe(fileObserver);
            publisher.NotifySubscribers("Після відписки FileObserver");
        }
    }
}

