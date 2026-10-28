using System;

// Appels de la méthode Heal
int h1 = Heal(50, 30, 100);
Console.WriteLine($"50 + 30 -> {h1}");

int h2 = Heal(90, 30, 100);
Console.WriteLine($"90 + 30 -> {h2}");

int h3 = Heal(0, 25, 100);
Console.WriteLine($"0 + 25 -> {h3}");


// Déclaration de la méthode Heal
int Heal(int health, int amount, int maxHealth)
{
    return Math.Min(health + amount, maxHealth);
}