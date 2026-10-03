using System;

namespace sw3v14
{
    public class CustomHttpClient : IDisposable
    {
        private bool _disposed = false; 
        private bool _isConnected; 
        private string _baseUrl;

        public string BaseUrl
        {
            get => _baseUrl;
            set => _baseUrl = value;
        }

        public bool IsConnected => _isConnected;

        public CustomHttpClient(string baseUrl)
        {
            _baseUrl = baseUrl;
            _isConnected = true;
            Console.WriteLine($"[HttpClient] Створено об'єкт. З'єднання з '{_baseUrl}' встановлено.");
        }

        public void Get(string endpoint)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(CustomHttpClient), "Неможливо виконати запит: об'єкт вже звільнено.");
            }

            if (_isConnected)
            {
                Console.WriteLine($"[HttpClient] GET-запит до '{_baseUrl}/{endpoint}' успішно виконано.");
            }
            else
            {
                Console.WriteLine($"[HttpClient] Помилка: з'єднання з '{_baseUrl}' закрито.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Console.WriteLine("[HttpClient] Звільнення керованих ресурсів...");
                }

                if (_isConnected)
                {
                    Console.WriteLine($"[HttpClient] Закриття з'єднання з '{_baseUrl}' (звільнення ресурсу).");
                    _isConnected = false;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        ~CustomHttpClient()
        {
            Console.WriteLine("[HttpClient] Викликано деструктор (фіналізатор) Garbage Collector'ом!");
            Dispose(false);
        }
    }
}