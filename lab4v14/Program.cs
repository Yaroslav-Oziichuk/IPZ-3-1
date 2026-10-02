using System;

namespace lab4v14
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("Лабораторна робота №4 | Варіант 14");
            
            Television tv = new Television("Samsung", "4K UHD (3840x2160)", 120);
            Radio radio = new Radio("Sony", 15, "FM 87.5-108 MHz");

            tv.ChangeChannel(5);
            radio.TuneStation(101.5);

            Console.WriteLine();

            Console.WriteLine("Демонстрація поліморфізму");

            List<Electronic> devices = new List<Electronic>
            {
                new Electronic("Generic Electronics", 50),
                tv,
                radio
            };

            foreach (Electronic device in devices)
            {
                device.TurnOn();
            }

            Console.WriteLine();

            Console.WriteLine("Демонстрація приховування членів");

            Electronic tvAsElectronic = tv;

            Console.WriteLine("\n[A] Виклик перевизначеного методу TurnOn():");
            Console.Write("Через посилання Television: ");
            tv.TurnOn();
            Console.Write("Через посилання Electronic: ");
            tvAsElectronic.TurnOn();
            Console.WriteLine("Висновок: В обох випадках викликається перевизначений метод похідного класу");

            Console.WriteLine("\n[B] Виклик прихованого методу GetElectronicType():");
            Console.WriteLine($"Через посилання Television: \"{tv.GetElectronicType()}\"");
            Console.WriteLine($"Через посилання Electronic: \"{tvAsElectronic.GetElectronicType()}\"");
            Console.WriteLine("Висновок: Виклик залежить від типу посилання");

            Console.ReadKey();
        }
    }
}
