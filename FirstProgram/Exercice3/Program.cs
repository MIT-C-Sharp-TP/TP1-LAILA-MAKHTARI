using System;

int health = 40;
int poisonDamage = 8;

for (int turn = 1; turn <= 6; turn++)
{
    health -= poisonDamage;
    if (health < 0) health = 0; // La vie ne descend jamais sous 0

    if (health == 0)
    {
        Console.WriteLine($"Turn {turn}: poison, {health} HP left");
        Console.WriteLine("Aya is down.");
        break; // Sortie de la boucle
    }

    Console.WriteLine($"Turn {turn}: poison, {health} HP left");
}