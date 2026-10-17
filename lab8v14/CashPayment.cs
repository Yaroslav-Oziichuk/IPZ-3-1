using System;

namespace Lab8v14
{
    public class CashPayment : Payment
    {
        public string Currency { get; set; }

        public CashPayment(decimal amount, string currency) 
            : base(amount)
        {
            Currency = currency;
        }

        public override void Process()
        {
            Console.WriteLine($"[Cash] Оплачено готівкою: {Amount} {Currency}.");
        }
    }
}
