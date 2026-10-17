using System;
using System.Collections.Generic;

namespace Lab8v14
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            List<Payment> payments = new List<Payment>
            {
                new CreditCardPayment(1250.50m, "4149490012345678"),
                new CashPayment(300.00m, "UAH"),
                new OnlinePayment(899.99m, "LiqPay"),
                new CreditCardPayment(450.00m, "5168750087654321"),
                new OnlinePayment(2100.00m, "PayPal")
            };

            Console.WriteLine("Процес обробки платежів\n");

            decimal totalAmount = 0m;

            foreach (var payment in payments)
            {
                payment.Process();
                totalAmount += payment.Amount; 
            }

            Console.WriteLine($"Загальна сума всіх проведених платежів: {totalAmount:C}");
            Console.WriteLine($"Всього оброблено транзакцій: {payments.Count}");

        }
    }
}

