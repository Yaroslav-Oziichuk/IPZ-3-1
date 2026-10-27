using System;
using System.Linq;

namespace lab12v14
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IRepository<Course> courseRepo = new InMemoryRepository<Course>();

            Console.WriteLine("Демонстрація операцій CRUD");

            // 1. Створення / Додавання (Create)
            courseRepo.Add(new Course { Id = 1, Title = "Об'єктно-орієнтоване програмування", Credits = 5, TeacherId = 101 });
            courseRepo.Add(new Course { Id = 2, Title = "Бази даних", Credits = 4, TeacherId = 102 });

            Console.WriteLine("\nУсі курси після додавання:");
            foreach (var course in courseRepo.GetAll())
            {
                Console.WriteLine($"[ID: {course.Id}] {course.Title} - {course.Credits} ECTS (ID викладача: {course.TeacherId})");
            }

            // 2. Читання / Отримання за ID (Read)
            var c1 = courseRepo.GetById(1);
            Console.WriteLine($"\nОтримано курс з ID 1: {c1?.Title}");

            // 3. Оновлення (Update)
            if (c1 != null)
            {
                c1.Title = "Поглиблене ООП";
                c1.Credits = 6;
                courseRepo.Update(c1);
            }

            Console.WriteLine($"\nКурс з ID 1 після оновлення: {courseRepo.GetById(1)?.Title}, Кредити: {courseRepo.GetById(1)?.Credits}");

            // 4. Видалення (Delete)
            courseRepo.Delete(2);
            Console.WriteLine($"\nЗагальна кількість курсів після видалення ID 2: {courseRepo.GetAll().Count()}");
        }
    }
}

