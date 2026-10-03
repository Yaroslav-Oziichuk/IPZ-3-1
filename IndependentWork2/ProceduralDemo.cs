using System;

namespace IndependentWork2
{
    public static class ProceduralDemo
    {
        public static void Run()
        {
            Console.WriteLine("Процедурний підхід");

            string[] productNames = { "Ноутбук", "Мишка", "Клавіатура", "Монітор" };
            decimal[] productPrices = { 25000m, 450m, 800m, 6000m };
            int[] productQuantities = { 1, 2, 1, 1 };

            decimal[] itemTotals = CalculateItemTotals(productPrices, productQuantities);
            decimal[] discounts = CalculateDiscounts(productPrices, productQuantities);

            decimal grandTotal = 0m;

            for (int i = 0; i < productNames.Length; i++)
            {
                decimal positionTotal = itemTotals[i] - discounts[i];
                grandTotal += positionTotal;

                Console.WriteLine($"Товар: {productNames[i]} | Ціна: {productPrices[i]} грн | Кількість: {productQuantities[i]} | " +
                                  $"Сума: {itemTotals[i]} грн | Знижка: {discounts[i]} грн | До сплати: {positionTotal} грн");
            }

            Console.WriteLine($"Загальна сума кошика (процедурно): {grandTotal} грн\n");
        }

        public static decimal[] CalculateItemTotals(decimal[] prices, int[] quantities)
        {
            decimal[] totals = new decimal[prices.Length];
            for (int i = 0; i < prices.Length; i++)
            {
                totals[i] = prices[i] * quantities[i];
            }
            return totals;
        }

        public static decimal[] CalculateDiscounts(decimal[] prices, int[] quantities)
        {
            decimal[] discounts = new decimal[prices.Length];
            for (int i = 0; i < prices.Length; i++)
            {
                if (prices[i] > 500m)
                {
                    discounts[i] = (prices[i] * quantities[i]) * 0.10m;
                }
                else
                {
                    discounts[i] = 0m;
                }
            }
            return discounts;
        }
    }
}
