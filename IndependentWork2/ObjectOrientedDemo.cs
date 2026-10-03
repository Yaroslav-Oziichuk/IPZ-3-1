using System;
using System.Collections.Generic;

namespace IndependentWork2
{
    // Клас Товар
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
        }
    }

    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public decimal GetItemTotal()
        {
            return Product.Price * Quantity;
        }

        public decimal GetDiscount()
        {
            if (Product.Price > 500m)
            {
                return GetItemTotal() * 0.10m;
            }
            return 0m;
        }

        public decimal GetFinalPrice()
        {
            return GetItemTotal() - GetDiscount();
        }
    }

    public class Cart
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            _items.Add(new CartItem(product, quantity));
        }

        public decimal GetTotal()
        {
            decimal total = 0m;
            foreach (var item in _items)
            {
                total += item.GetFinalPrice();
            }
            return total;
        }

        public void PrintCartDetails()
        {
            Console.WriteLine("Об'єктно-орієнтований підхід");
            foreach (var item in _items)
            {
                Console.WriteLine($"Товар: {item.Product.Name} | Ціна: {item.Product.Price} грн | Кількість: {item.Quantity} | " +
                                  $"Сума: {item.GetItemTotal()} грн | Знижка: {item.GetDiscount()} грн | До сплати: {item.GetFinalPrice()} грн");
            }
            Console.WriteLine($"Загальна сума кошика (ООП): {GetTotal()} грн\n");
        }
    }

    public static class ObjectOrientedDemo
    {
        public static void Run()
        {
            Cart cart = new Cart();

            cart.AddItem(new Product("Ноутбук", 25000m), 1);
            cart.AddItem(new Product("Мишка", 450m), 2);
            cart.AddItem(new Product("Клавіатура", 800m), 1);
            cart.AddItem(new Product("Монітор", 6000m), 1);

            cart.PrintCartDetails();
        }
    }
}
