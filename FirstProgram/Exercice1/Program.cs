using System;

class Program
{
    static void Main()
    {
        string name = "Aya";
        int level = 3;
        int health = 100;
        float speed = 5.5f;
        bool hasKey = false;

        Console.WriteLine($"Name:  {name}");
        Console.WriteLine($"Level: {level}");
        Console.WriteLine($"Health: {health} HP");
        Console.WriteLine($"Speed: {speed} m/s");
        Console.WriteLine($"Key:   {hasKey}");
        Console.WriteLine($"Next Level at {level * 100} XP");
    }
}