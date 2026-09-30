using System;

namespace lab2v14;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        CreateObjects();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        Console.ReadLine();
    }

    static void CreateObjects()
    {
        Restaurant rest1 = new Restaurant();
        Restaurant rest2 = new Restaurant("Pasta Prima", "Італійська", 4.8);
        Restaurant rest3 = new Restaurant("Fast Food Center", "Американська", 7.5);

        Console.WriteLine("Інформація про ресторани");
        rest1.PrintInfo();
        rest2.PrintInfo();
        rest3.PrintInfo();

        Console.WriteLine("Подача страв");
        rest1.ServeDish("Піца Маргарита");
        rest2.ServeDish("Паста Карбонара");
        rest3.ServeDish("Бургер Меню");
    }
}
