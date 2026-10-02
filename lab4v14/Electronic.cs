using System;

namespace lab4v14
{
    public class Electronic
    {
        private string _brand = string.Empty;
        private int _powerConsumption;

        public string Brand
        {
            get => _brand;
            set => _brand = string.IsNullOrWhiteSpace(value) ? "Невідомий бренд" : value;
        }

        public int PowerConsumption
        {
            get => _powerConsumption;
            set => _powerConsumption = value >= 0 ? value : 0;
        }
        
        public Electronic(string brand, int powerConsumption)
        {
            Brand = brand;
            PowerConsumption = powerConsumption;
        }

        public virtual void TurnOn()
        {
            Console.Write($"[Electronic] Пристрій {Brand} ввімкнено. Енергоспоживання: {PowerConsumption} Вт.");
        }

        public string GetElectronicType()
        {
            return "Загальний електронний пристрій";
        }
    }
}