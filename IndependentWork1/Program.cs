using System;

namespace IndependentWork1;

class Program
{
    static void Main(string[] args)
    {
        Student student = new Student("Ткаченко Максим", 75.5);
        double scholarshipThreshold = 85.0;

        Console.WriteLine($"Ім'я студента: {student.Name}");
        Console.WriteLine($"Середній бал: {student.AverageMark}");
        Console.WriteLine($"Чи буде стипендія(поріг {scholarshipThreshold}) : {(student.Scholarships(scholarshipThreshold) ? "так" : "Ні")} ");

        Console.WriteLine("Демонстрація класу GameCharacter\n");

        GameCharacter hero = new GameCharacter("ShadowKnight", 150);

        Console.WriteLine($"Створено персонажа: {hero.Nickname}");
        Console.WriteLine($"Здоров'я: {hero.Health}/{hero.MaxHealth} | Живий: {(hero.IsAlive ? "Так" : "Ні")}\n");

        hero.TakeDamage(40);  
        hero.TakeDamage(140); 
        hero.TakeDamage(10);  

        Console.ReadLine();
    }
}
