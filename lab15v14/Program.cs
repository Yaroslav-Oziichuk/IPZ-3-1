using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== СИСТЕМА РЕЗЕРВНОГО КОПІЮВАННЯ (Варіант 14) ===\n");
        
        // 1. HashSet<T>: Унікальні файли для бекапу
        Console.WriteLine("--- 1. Робота з HashSet<T> (Унікальні файли) ---");
        
        // Список файлів, що надходять із дублікатами
        var incomingFiles = new List<string> 
        { 
            "document.docx", 
            "photo.png", 
            "document.docx", // дублікат 
            "database.mdf", 
            "photo.png",     // дублікат
            "config.json" 
        };

        Console.WriteLine("Вхідний список файлів (з дублікатами):");
        foreach (var file in incomingFiles)
        {
            Console.WriteLine($" - {file}");
        }

        // Ініціалізація HashSet для відсіювання дублікатів
        HashSet<string> uniqueBackupFiles = new HashSet<string>(incomingFiles);
        
        Console.WriteLine($"\nПісля додавання у HashSet (унікальних файлів: {uniqueBackupFiles.Count}):");
        foreach (var file in uniqueBackupFiles)
        {
            Console.WriteLine($" [Унікальний] -> {file}");
        }

        // Перевірка наявності елемента та додавання нового
        string testFile = "report.pdf";
        Console.WriteLine($"\nПеревірка наявності '{testFile}': {uniqueBackupFiles.Contains(testFile)}");
        
        uniqueBackupFiles.Add(testFile);
        Console.WriteLine($"Додано новий файл '{testFile}'. Поточна кількість: {uniqueBackupFiles.Count}");
        
        // Видалення елемента
        uniqueBackupFiles.Remove("config.json");
        Console.WriteLine("Файл 'config.json' видалено з набору для бекапу.\n");

        // 2. Queue<T>: Черга файлів на копіювання (FIFO)
        Console.WriteLine("--- 2. Робота з Queue<T> (Черга копіювання - FIFO) ---");
        
        Queue<string> backupQueue = new Queue<string>();

        // Додавання файлів у чергу (Enqueue)
        foreach (var file in uniqueBackupFiles)
        {
            backupQueue.Enqueue(file);
            Console.WriteLine($"У чергу на копіювання додано: {file}");
        }

        Console.WriteLine($"\nЗагалом у черзі файлів: {backupQueue.Count}");
        
        // Перегляд першого елемента в черзі без видалення (Peek)
        Console.WriteLine($"Найближчий до копіювання файл (Peek): {backupQueue.Peek()}");

        Console.WriteLine("\nПочинається процес резервного копіювання:");
        
        // Стек для збереження історії успішних бекапів
        Stack<string> backupHistory = new Stack<string>();

        while (backupQueue.Count > 0)
        {
            // Видаляємо файл з початку черги
            string currentFile = backupQueue.Dequeue();
            Console.WriteLine($"[КОПІЮВАННЯ] Файл '{currentFile}' успішно збережено на сервер. Залишилось у черзі: {backupQueue.Count}");
            
            // Зберігаємо успішну операцію в історію (Stack)
            backupHistory.Push(currentFile);
        }
        Console.WriteLine("Усі файли з черги успішно скопійовані!\n");

        // 3. Stack<T>
        Console.WriteLine("--- 3. Робота з Stack<T> (Історія бекапів - LIFO) ---");
        Console.WriteLine($"Кількість записів в історії бекапів: {backupHistory.Count}");

        Console.WriteLine($"Останній доданий до бекапу файл (Peek): {backupHistory.Peek()}");

        Console.WriteLine("\nПослідовність видачі історії бекапів (з кінця до початку — LIFO):");
        while (backupHistory.Count > 0)
        {
            string lastBackedUp = backupHistory.Pop();
            Console.WriteLine($" [Історія] Отримано запис про бекап файлу: {lastBackedUp}. Залишилось в історії: {backupHistory.Count}");
        }

        Console.WriteLine("\nРоботу програми завершено успішно.");
    }
}

