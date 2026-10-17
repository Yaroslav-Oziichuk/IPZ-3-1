using System;

namespace Lab8v14
{
    public class CreditCardPayment : Payment
    {
        public string CardNumber { get; set; }

        public CreditCardPayment(decimal amount, string cardNumber) 
            : base(amount)
        {
            CardNumber = cardNumber;
        }

        public override void Process()
        {
            string maskedCard = CardNumber.Length >= 4 
                ? "**** **** **** " + CardNumber.Substring(CardNumber.Length - 4) 
                : CardNumber;

            Console.WriteLine($"[CreditCard] Оплачено {Amount:C} за допомогою картки ({maskedCard}).");
        }
    }
}
