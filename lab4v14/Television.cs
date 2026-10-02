using System;

namespace lab4v14
{
    public class Television : Electronic
    {
        public string ScreenResolution {get;set;}

        public Television(string brand, string screenResolution, int powerConsumption)
            : base(brand, powerConsumption)
        {
            ScreenResolution = screenResolution;
        }
        public override void TurnOn()
        {
            Console.WriteLine($"[Television] Телевізор {Brand} ({ScreenResolution}) увімкнено. Завантаження головного екрана... Енергоспоживання: {PowerConsumption} Вт.");
        }
        public void ChangeChannel(int channel)
        {
            Console.WriteLine($"[Television] Телевізор {Brand}: переключено на канал №{channel}.");
        }
        public new string GetElectronicType()
        {
            return "Телевізійний приймач";
        }
    }
}