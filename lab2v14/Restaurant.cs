namespace lab2v14;
public class Restaurant
    {
        private string _name = string.Empty;
        private string _cuisine = string.Empty;
        private double _rating;

        public string Name
        {
            get => _name;
            set => _name = string.IsNullOrWhiteSpace(value) ? "Unnamed" : value;
        }

        public string Cuisine
        {
            get => _cuisine;
            set => _cuisine = string.IsNullOrWhiteSpace(value) ? "Mixed" : value;
        }

        public double Rating
        {
            get => _rating;
            set
            {
                if (value < 1.0 || value > 5.0)
                {
                    Console.WriteLine($"[Валідація] Помилка: Рейтинг {value} поза межами (1.0 - 5.0). Встановлено значення за замовчуванням 3.0.");
                    _rating = 3.0;
                }
                else
                {
                    _rating = value;
                }
            }
        }

        public Restaurant() : this("Unnamed", "Mixed", 3.0)
        {
        }

        public Restaurant(string name, string cuisine, double rating)
        {
            Name = name;
            Cuisine = cuisine;
            Rating = rating; 
        }

        public void ServeDish(string dishName)
        {
            Console.WriteLine($"Ресторан \"{Name}\" ({Cuisine}, Рейтинг: {Rating:F1}) подає страву: {dishName}");
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Назва: \"{Name}\" | Кухня: {Cuisine} | Рейтинг: {Rating:F1}");
        }

        ~Restaurant()
        {
            Console.WriteLine($"[Деструктор] Об'єкт ресторану \"{_name}\" знищено з пам'яті.");
        }
    }