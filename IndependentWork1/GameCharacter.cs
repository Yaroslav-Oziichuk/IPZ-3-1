using System;

namespace IndependentWork1;

public class GameCharacter
{
    // 1. Приватні поля
    private string _nickname = string.Empty;
    private int _health;
    private int _maxHealth;

    public string Nickname
    {
        get => _nickname;
        set => _nickname = string.IsNullOrWhiteSpace(value) ? "Безременний Воїн" : value;
    }

    public int Health => _health;

    public int MaxHealth
    {
        get => _maxHealth;
        private set => _maxHealth = value > 0 ? value : 100;
    }

    public bool IsAlive => _health > 0;

    // 3. Конструктор
    public GameCharacter(string nickname, int maxHealth)
    {
        Nickname = nickname;
        MaxHealth = maxHealth;
        _health = MaxHealth; 
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive)
        {
            Console.WriteLine($"Персонаж {Nickname} вже виведений з ладу!");
            return;
        }

        if (damage < 0) damage = 0; 

        _health -= damage;

        if (_health <= 0)
        {
            _health = 0;
            Console.WriteLine($"[Шкода: {damage}] Персонаж {Nickname} отримав смертельного удару і загинув!");
        }
        else
        {
            Console.WriteLine($"[Шкода: {damage}] {Nickname} отримав шкоду. Залишилося здоров'я: {_health}/{MaxHealth}");
        }
    }
}