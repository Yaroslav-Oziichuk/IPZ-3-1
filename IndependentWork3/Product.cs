using System;

namespace IndependentWork3
{
    public class Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string Category { get; }
        public int StockCount { get; }

        public Product(int id, string name, decimal price, string category, int stockCount)
        {
            Id = id;
            Name = name;
            Price = price;
            Category = category;
            StockCount = stockCount;
        }
        public Product(int id, string name, decimal price) : this (id, name, price, "Uncategorized", 0)
        {
        }

        public Product(Product other) : 
            this(other.Id, other.Name, other.Price, other.Category, other.StockCount)
        {
        }
        public override string ToString()
        {
            return $"ID: {Id}, NAME: {Name}, PRICE: {Price}, CATEGORY: {Category}, STOCKCOUNT: {StockCount}";
        }

    }
}