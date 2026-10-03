using System;

namespace sw3v14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Використання об'єкта через блок using");
            using (CustomHttpClient client1 = new CustomHttpClient("https://api.example.com"))
            {
                client1.Get("users");
            } 
            Console.WriteLine();

            Console.WriteLine("Явний виклик Dispose() без using");
            CustomHttpClient client2 = new CustomHttpClient("https://api.weather.com");
            client2.Get("v1/forecast");
            client2.Dispose(); 

            Console.WriteLine("Спроба повторного виклику Dispose():");
            client2.Dispose();
            Console.WriteLine();

            Console.WriteLine("Демонстрація роботи деструктора через GC");
            CreateUnmanagedClient();

            Console.WriteLine("Викликаємо Garbage Collector...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private static void CreateUnmanagedClient()
        {
            CustomHttpClient client3 = new CustomHttpClient("https://api.orphaned-resource.org");
            client3.Get("data");
        }
    }
}