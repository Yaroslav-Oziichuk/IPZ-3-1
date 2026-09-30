using System;

class Student
{
    private string _name;
    private double _averageMark;

    public string Name
    {
        get => _name;
        set => _name = string.IsNullOrWhiteSpace(value) ? "Студент" : value;
    }

    public double AverageMark
    {
        get => _averageMark;
        set => _averageMark = value;
    }

    public Student(string name, double averageMark)
    {
        _name = name;
        _averageMark = averageMark >= 0 && averageMark <= 100 ? averageMark : 60.0;
    }

    public bool Scholarships(double min)
    {
        return _averageMark >= min;
    }
}