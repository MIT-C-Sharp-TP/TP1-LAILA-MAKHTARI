using System;

int health = 45; // Testez aussi avec 100, 12, ou 0

string status;

if (health > 70)
{
    status = "healthy";
}
else if (health >= 31)
{
    status = "injured";
}
else if (health >= 1)
{
    status = "critical";
}
else
{
    status = "Aya is down";
}

if (health > 0)
{
    Console.WriteLine($"Aya has {health} HP: {status}");
}
else
{
    Console.WriteLine(status);
}