using lab17v14.Models;

namespace lab17v14.Data;

public static class SampleData
{
    public static List<Recipe> CreateRecipes()
    {
        // Постачальники
        var greenField = new Supplier(1, "Ферма «Зелений лан»");
        var dairy      = new Supplier(2, "Молокозавод «Рівне-Молоко»");
        var meat       = new Supplier(3, "М'ясокомбінат «Волинь»");
        var grain      = new Supplier(4, "Агрохолдинг «Зерно України»");
        var fish       = new Supplier(5, "Рибгосп «Случ»");
        var vegetables = new Supplier(6, "Оптова база «Овочі+»");
        var spices     = new Supplier(7, "«Імпорт-Спайс»");
        var apiary     = new Supplier(8, "Пасіка «Медовий край»");
        var poultry    = new Supplier(9, "Птахофабрика «Золотий півень»");

        // Інгредієнти
        var flour     = new Ingredient(1,  "Борошно",            grain, greenField);
        var milk      = new Ingredient(2,  "Молоко",             dairy, greenField);
        var eggs      = new Ingredient(3,  "Яйця",               poultry, greenField);
        var sugar     = new Ingredient(4,  "Цукор",              grain, spices);
        var butter    = new Ingredient(5,  "Вершкове масло",     dairy);
        var cottage   = new Ingredient(6,  "Сир кисломолочний",  dairy, greenField);
        var chicken   = new Ingredient(7,  "Куряче філе",        poultry, meat);
        var beef      = new Ingredient(8,  "Яловичина",          meat);
        var pork      = new Ingredient(9,  "Свинина",            meat);
        var potato    = new Ingredient(10, "Картопля",           vegetables, greenField);
        var carrot    = new Ingredient(11, "Морква",             vegetables, greenField);
        var onion     = new Ingredient(12, "Цибуля",             vegetables, greenField);
        var tomato    = new Ingredient(13, "Помідори",           vegetables, greenField);
        var salmon    = new Ingredient(14, "Лосось",             fish);
        var salt      = new Ingredient(15, "Сіль",               spices, grain);
        var pepper    = new Ingredient(16, "Чорний перець",      spices);
        var honey     = new Ingredient(17, "Мед",                apiary);
        var buckwheat = new Ingredient(18, "Гречка",             grain);
        var rice      = new Ingredient(19, "Рис",                grain, spices);
        var oil       = new Ingredient(20, "Соняшникова олія",   greenField, grain);
        var sourCream = new Ingredient(21, "Сметана",            dairy);
        var cabbage   = new Ingredient(22, "Капуста",            vegetables, greenField);
        var beet      = new Ingredient(23, "Буряк",              vegetables, greenField);

        // Рецепти
        return new List<Recipe>
        {
            new(1,  "Борщ",                        beet, cabbage, potato, carrot, onion, beef, sourCream, salt),
            new(2,  "Деруни",                      potato, onion, eggs, flour, salt, oil),
            new(3,  "Сирники",                     cottage, eggs, flour, sugar, sourCream),
            new(4,  "Млинці",                      milk, eggs, flour, sugar, butter, oil),
            new(5,  "Гречана каша з куркою",       buckwheat, chicken, onion, carrot, butter, salt),
            new(6,  "Плов",                        rice, pork, carrot, onion, oil, pepper, salt),
            new(7,  "Печений лосось",              salmon, butter, pepper, salt),
            new(8,  "Овочеве рагу",                potato, carrot, onion, tomato, cabbage, oil, salt),
            new(9,  "Медовик",                     flour, honey, eggs, sugar, sourCream, butter),
            new(10, "Вареники з картоплею",        flour, potato, onion, eggs, salt, sourCream),
            new(11, "Курячий суп з рисом",         chicken, rice, carrot, onion, potato, salt),
            new(12, "Молочна рисова каша з медом", rice, milk, sugar, butter, honey),
        };
    }
}
