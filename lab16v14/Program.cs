using System.Diagnostics;
using System.Text;
using Lab16v14.Models;

namespace Lab16v14;

public class Program
{
    public static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        List<Department> departments = CreateDepartments();
        List<Employee> employees = CreateEmployees();

        Console.WriteLine("=== Лабораторна №10 | Варіант 14 | LINQ: співробітники та відділи ===");
        Console.WriteLine($"Відділів: {departments.Count}, співробітників: {employees.Count}\n");

        var t1 = Query1(employees, departments);
        var t2 = Query2(employees, departments);
        var t3 = Query3(employees, departments);
        var t4 = Query4(employees);
        DeferredVsImmediate(employees);

        Console.WriteLine("=== Підсумок часу виконання (мкс) ===");
        Console.WriteLine($"{"Запит",-34}{"Query syntax",14}{"Method syntax",16}");
        Console.WriteLine($"{"1. Фільтр (Sales, >50000)",-34}{Us(t1.Query),14}{Us(t1.Method),16}");
        Console.WriteLine($"{"2. Join",-34}{Us(t2.Query),14}{Us(t2.Method),16}");
        Console.WriteLine($"{"3. GroupBy + Sum + Join",-34}{Us(t3.Query),14}{Us(t3.Method),16}");
        Console.WriteLine($"{"4. Максимум (OrderBy vs MaxBy)",-34}{Us(t4.Query),14}{Us(t4.Method),16}");
    }

    // ---------- Тестові дані ----------
    private static List<Department> CreateDepartments() => new()
    {
        new Department(1,  "Sales",     500_000m),
        new Department(2,  "IT",        900_000m),
        new Department(3,  "HR",        250_000m),
        new Department(4,  "Finance",   400_000m),
        new Department(5,  "Marketing", 350_000m),
        new Department(6,  "Support",   300_000m),
        new Department(7,  "Legal",     280_000m),
        new Department(8,  "Logistics", 420_000m),
        new Department(9,  "Research",  650_000m),
        new Department(10, "Security",  330_000m)
    };

    private static List<Employee> CreateEmployees() => new()
    {
        new Employee(1,  "Іваненко Олег",     1,  62_000m),
        new Employee(2,  "Петренко Марія",    1,  48_000m),
        new Employee(3,  "Коваль Андрій",     1,  55_500m),
        new Employee(4,  "Шевченко Ірина",    1,  51_000m),
        new Employee(5,  "Бондаренко Денис",  1,  39_000m),
        new Employee(6,  "Мельник Софія",     2,  95_000m),
        new Employee(7,  "Ткаченко Максим",   2,  88_000m),
        new Employee(8,  "Кравченко Анна",    2,  72_000m),
        new Employee(9,  "Олійник Богдан",    2,  67_500m),
        new Employee(10, "Лисенко Віталій",   3,  35_000m),
        new Employee(11, "Гончар Наталя",     3,  41_000m),
        new Employee(12, "Савченко Юрій",     3,  38_500m),
        new Employee(13, "Руденко Оксана",    4,  58_000m),
        new Employee(14, "Мороз Тарас",       4,  64_000m),
        new Employee(15, "Павленко Катерина", 4,  52_000m),
        new Employee(16, "Яковенко Ігор",     5,  45_000m),
        new Employee(17, "Романюк Світлана",  5,  47_500m),
        new Employee(18, "Волошин Артем",     5,  43_000m),
        new Employee(19, "Захарчук Леся",     1,  70_000m),
        new Employee(20, "Дмитренко Павло",   2,  81_000m),
        new Employee(21, "Сидоренко Роман",   6,  33_000m),
        new Employee(22, "Бойко Валерія",     7,  57_000m),
        new Employee(23, "Кузьменко Дмитро",  8,  44_000m),
        new Employee(24, "Поліщук Алла",      9,  76_000m)
    };

    private static (TimeSpan Query, TimeSpan Method) Query1(
        List<Employee> employees, List<Department> departments)
    {
        Console.WriteLine("--- Запит 1: зарплата > 50000, відділ \"Sales\" ---");

        // Синтаксис запитів
        var (byQuery, tQuery) = Measure(() =>
            (from e in employees
             join d in departments on e.DepartmentId equals d.Id
             where d.Name == "Sales" && e.Salary > 50_000m
             select e).ToList());

        // Синтаксис методів
        var (byMethod, tMethod) = Measure(() =>
            employees
                .Where(e => e.Salary > 50_000m &&
                            departments.Any(d => d.Id == e.DepartmentId && d.Name == "Sales"))
                .ToList());

        Print("Синтаксис запитів", tQuery);
        foreach (var e in byQuery) Console.WriteLine($"   {e.Name,-20} {e.Salary,10:N0}");
        Print("Синтаксис методів", tMethod);
        Console.WriteLine($"   Результати збігаються: {byQuery.SequenceEqual(byMethod)}\n");
        return (tQuery, tMethod);
    }

    private static (TimeSpan Query, TimeSpan Method) Query2(
        List<Employee> employees, List<Department> departments)
    {
        Console.WriteLine("--- Запит 2: join співробітників з відділами ---");

        // Синтаксис запитів
        var (byQuery, tQuery) = Measure(() =>
            (from e in employees
             join d in departments on e.DepartmentId equals d.Id
             select new { Employee = e.Name, Department = d.Name }).ToList());

        // Синтаксис методів
        var (byMethod, tMethod) = Measure(() =>
            employees
                .Join(departments,
                      e => e.DepartmentId,
                      d => d.Id,
                      (e, d) => new { Employee = e.Name, Department = d.Name })
                .ToList());

        Print("Синтаксис запитів", tQuery);
        foreach (var r in byQuery) Console.WriteLine($"   {r.Employee,-20} -> {r.Department}");
        Print("Синтаксис методів", tMethod);
        Console.WriteLine($"   Кількість рядків: {byQuery.Count} / {byMethod.Count}\n");
        return (tQuery, tMethod);
    }

    private static (TimeSpan Query, TimeSpan Method) Query3(
        List<Employee> employees, List<Department> departments)
    {
        Console.WriteLine("--- Запит 3: загальна зарплата по відділах ---");

        // Синтаксис запитів
        var (byQuery, tQuery) = Measure(() =>
            (from e in employees
             group e by e.DepartmentId into g
             join d in departments on g.Key equals d.Id
             orderby d.Name
             select new { Department = d.Name, Total = g.Sum(x => x.Salary) }).ToList());

        // Синтаксис методів
        var (byMethod, tMethod) = Measure(() =>
            employees
                .GroupBy(e => e.DepartmentId)
                .Join(departments,
                      g => g.Key,
                      d => d.Id,
                      (g, d) => new { Department = d.Name, Total = g.Sum(x => x.Salary) })
                .OrderBy(x => x.Department)
                .ToList());

        Print("Синтаксис запитів", tQuery);
        foreach (var r in byQuery) Console.WriteLine($"   {r.Department,-10} {r.Total,12:N0}");
        Print("Синтаксис методів", tMethod);
        Console.WriteLine($"   Кількість груп: {byQuery.Count} / {byMethod.Count}\n");
        return (tQuery, tMethod);
    }

    private static (TimeSpan Query, TimeSpan Method) Query4(List<Employee> employees)
    {
        Console.WriteLine("--- Запит 4: найвища зарплата ---");

        var (byQuery, tQuery) = Measure(() =>
            (from e in employees
             orderby e.Salary descending
             select e).First());

        var (byMethod, tMethod) = Measure(() => employees.MaxBy(e => e.Salary)!);

        Print("Синтаксис запитів (OrderByDescending + First)", tQuery);
        Console.WriteLine($"   {byQuery}");
        Print("Синтаксис методів (MaxBy)", tMethod);
        Console.WriteLine($"   {byMethod}\n");
        return (tQuery, tMethod);
    }

    // ---------- Додатково: відкладене vs негайне виконання ----------
    private static void DeferredVsImmediate(List<Employee> employees)
    {
        Console.WriteLine("--- Додатково: відкладене та негайне виконання ---");
        var (_, tDeferred) = Measure(() => employees.Where(e => e.Salary > 50_000m)); // лише створення запиту
        var (_, tImmediate) = Measure(() => employees.Where(e => e.Salary > 50_000m).ToList());
        Print("Where без ToList (відкладене: запит лише створено)", tDeferred);
        Print("Where + ToList (негайне виконання)", tImmediate);
        Console.WriteLine();
    }

    // ---------- Допоміжні методи ----------
    private static (T Result, TimeSpan Elapsed) Measure<T>(Func<T> query)
    {
        query(); 
        var sw = Stopwatch.StartNew();
        T result = query();
        sw.Stop();
        return (result, sw.Elapsed);
    }

    private static string Us(TimeSpan t) => (t.TotalMilliseconds * 1000).ToString("F1");

    private static void Print(string label, TimeSpan t) =>
        Console.WriteLine($" [{label}] час: {t.TotalMilliseconds:F4} мс ({Us(t)} мкс)");
}

