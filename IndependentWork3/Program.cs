using System;

namespace IndependentWork3
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("Створення об'єктів");
            Product product1 = new Product(87, "Laptop", 28000.00m, "Electronics", 10);
            Console.WriteLine("Товар 1 (основний конструктор):");
            Console.WriteLine(product1);
            Console.WriteLine();

            Product product2 = new Product(88, "Mouse", 1000.00m);
            Console.WriteLine("Товар 2 (скорочений конструктор):");
            Console.WriteLine(product2);
            Console.WriteLine();

            Product product3 = new Product(product1);
            Console.WriteLine("Товар 3 (конструктор копіювання - дублікат Товару 1):");
            Console.WriteLine(product3);
            Console.WriteLine();
        }
    }
}
