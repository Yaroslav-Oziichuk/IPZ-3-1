namespace lab1v14;

public class Restaurant
{
    private string _name;
    private string _cuisine;
    private double _rating;

    public string Name
    {
        get 
        { 
            return _name; 
        }
        set 
        { 
            _name = value; 
        }
    }

    public string Cuisine
    {
        get 
        { 
            return _cuisine; 
        }
        set 
        { 
            _cuisine = value; 
        }
    }

    public double Rating
    {
        get 
        { 
            return _rating; 
        }
        set 
        { 
            if (value >= 0 && value <= 5)
                _rating = value;
            else
                _rating = 0;
        }
    }

    public Restaurant(string name, string cuisine, double rating)
    {
        _name = name;
        _cuisine = cuisine;
        Rating = rating; 
    }

    public void ServeDish()
    {
        Console.WriteLine($"Ресторан {Name}, {Cuisine} кухня, рейтинг: {Rating}/5");
    }
}