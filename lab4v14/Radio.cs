using System;

namespace lab4v14
{
    public class Radio : Electronic
    {
        public string FrequencyRange{get; set;}

        public Radio(string brand, int powerConsumption, string frequencyRange)
            : base(brand, powerConsumption)
        {
            FrequencyRange = frequencyRange;
        }

        public override void TurnOn()
        {
            Console.WriteLine($"[Radio] Радіоприймач {Brand} увімкнено. Діапазон частот: {FrequencyRange}. Відтворення динаміка... Енергоспоживання: {PowerConsumption} Вт.");
        }

        public void TuneStation(double frequency)
        {
            Console.WriteLine($"[Radio] Радіо {Brand}: налаштовано на частоту {frequency} МГц.");
        }
    }
}