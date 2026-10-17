using System;

namespace Lab8v14
{
    public class Payment
    {
        public decimal Amount { get; set; }

        public Payment(decimal amount)
        {
            Amount = amount;
        }

        public virtual void Process()
        {
            Console.WriteLine($"[Payment] Опрацювання базового платежу на суму: {Amount:C}");
        }
    }
}