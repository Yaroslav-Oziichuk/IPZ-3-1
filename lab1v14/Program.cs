using lab1v14;

class Program
{
    static void Main(string[] args)
    {
        Restaurant rest1 = new Restaurant("La Pizza", "Італійська", 4.3);
        Restaurant rest2 = new Restaurant("Sakura", "Японська", 3.5);
        Restaurant rest3 = new Restaurant("Затишок", "Українська", 4.9);

        rest1.ServeDish();
        rest2.ServeDish();
        rest3.ServeDish();
    }
}
