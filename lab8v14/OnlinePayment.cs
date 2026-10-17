using System;

namespace Lab8v14
{
    public class OnlinePayment : Payment
    {
        public string Gateway { get; set; }

        public OnlinePayment(decimal amount, string gateway) 
            : base(amount)
        {
            Gateway = gateway;
        }

        public override void Process()
        {
            Console.WriteLine($"[Online] Оплачено {Amount:C} через платіжний шлюз: {Gateway}.");
        }
    }
}
