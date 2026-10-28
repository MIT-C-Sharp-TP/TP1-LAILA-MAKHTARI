using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

List<int> wins = new List<int>(); // Nombre de tours de chaque victoire
int game = 0;

// Exercice 3 : Plusieurs parties
while (true)
{
    game++;
    Console.WriteLine($"\n=================== GAME {game} ===================");
    
    int turns = PlayGame(); // Exécute une partie complète

    if (turns > 0)
    {
        wins.Add(turns);
        SaveResult($"Game {game}: victory in {turns} turns");
    }
    else
    {
        SaveResult($"Game {game}: defeat");
    }

    Console.Write("\nPlay again? (y/n) > ");
    if (Console.ReadLine()?.Trim().ToLower() != "y")
    {
        break;
    }
}

// Bilan final avec LINQ
Console.WriteLine($"\nVictories: {wins.Count} of {game}");
foreach (int t in wins.OrderBy(t => t).Take(3))
{
    Console.WriteLine($"- {t} turns");
}

// --- MÉTHODE PRINCIPALE DE JEU ---

int PlayGame()
{
    Random rng = new Random(); // new Random(7) pour une graine fixe
    Player player = new Player("Aya", 100, 2);

    // Exercice 2 : Abonnement à l'événement Died via une lambda
    player.Died += name => Console.WriteLine($"Game over for {name}.");

    List<Enemy> enemies = new List<Enemy>
    {
        new Goblin(),
        new Orc(),
        new Goblin(),
        new Boss() // Exercice 1 : Ajout du Boss
    };

    int turn = 0;

    Console.WriteLine("=== Arena ===");

    while (player.IsAlive && enemies.Count > 0)
    {
        turn++;
        Enemy enemy = enemies[0];

        Console.WriteLine();
        Console.WriteLine($"Turn {turn}: {player.Name} {player.Health} HP, {player.Potions} potions | {enemy.Name} {enemy.Health} HP");
        Console.Write("1 attack, 2 drink a potion > ");

        string input = Console.ReadLine();
        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Type 1 or 2.");
            continue;
        }

        bool acted = false;

        switch (choice)
        {
            case 1:
                int damage = rng.Next(12, 25);
                enemy.TakeDamage(damage);
                acted = true;
                break;

            case 2:
                player.DrinkPotion();
                acted = true;
                break;

            default:
                Console.WriteLine("Type 1 or 2.");
                continue;
        }

        if (!acted) continue;

        if (!enemy.IsAlive)
        {
            Console.WriteLine($"{enemy.Name} is defeated!");
            enemies.RemoveAt(0);
            continue;
        }

        // Riposte de l'ennemi (polymorphisme : chaque type d'ennemi a sa propre attaque)
        enemy.Attack(player, rng);
    }

    if (player.IsAlive)
    {
        Console.WriteLine($"\nVictory in {turn} turns!");
        return turn;
    }
    else
    {
        Console.WriteLine("\nDefeat...");
        return 0;
    }
}

// Exercice 4 : Sauvegarde sécurisée dans un fichier texte avec try/catch
void SaveResult(string line)
{
    try
    {
        File.AppendAllText("results.txt", line + Environment.NewLine);
        Console.WriteLine($"Saved: {line}");
    }
    catch (IOException e)
    {
        Console.WriteLine($"Could not save the result: {e.Message}");
    }
    catch (UnauthorizedAccessException e)
    {
        Console.WriteLine($"Could not save the result: {e.Message}");
    }
}

// --- CLASSES ---

public class Player
{
    private int health;

    // Exercice 2 : Événement Died
    public event Action<string> Died;

    public string Name { get; }
    public int MaxHealth { get; }
    public int Potions { get; private set; }

    public int Health
    {
        get => health;
        private set => health = Math.Clamp(value, 0, MaxHealth);
    }

    public bool IsAlive => health > 0;

    public Player(string name, int maxHealth, int potions)
    {
        Name = name;
        MaxHealth = maxHealth;
        Potions = potions;
        Health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        bool wasAlive = IsAlive;
        Health -= amount;
        Console.WriteLine($"{Name} takes {amount} damage ({Health} HP)");

        // Exercice 2 : Déclenchement de l'événement uniquement au moment de la mort
        if (wasAlive && !IsAlive)
        {
            Died?.Invoke(Name);
        }
    }

    public void DrinkPotion()
    {
        if (Potions <= 0)
        {
            Console.WriteLine("No potion left!");
            return;
        }

        Potions--;
        Health += 30;
        Console.WriteLine($"{Name} drinks a potion ({Health} HP)");
    }
}

public abstract class Enemy
{
    public string Name { get; }
    public int Health { get; private set; }
    public bool IsAlive => Health > 0;

    protected Enemy(string name, int health)
    {
        Name = name;
        Health = health;
    }

    // Exercice 1 : virtual permet aux sous-classes de redéfinir la méthode
    public virtual void TakeDamage(int amount)
    {
        Health = Math.Max(Health - amount, 0);
        Console.WriteLine($"{Name} takes {amount} damage ({Health} HP)");
    }

    public abstract void Attack(Player target, Random rng);
}

public class Goblin : Enemy
{
    public Goblin() : base("Goblin", 30) { }

    public override void Attack(Player target, Random rng)
    {
        Console.WriteLine("Goblin stabs!");
        target.TakeDamage(rng.Next(4, 9));
    }
}

public class Orc : Enemy
{
    public Orc() : base("Orc", 60) { }

    public override void Attack(Player target, Random rng)
    {
        Console.WriteLine("Orc swings its club!");
        target.TakeDamage(rng.Next(8, 15));
    }
}

// Exercice 1 : La classe Boss avec armure
public class Boss : Enemy
{
    public int Armour { get; } = 6;

    public Boss() : base("Boss", 60) { }

    public override void TakeDamage(int amount)
    {
        int reduced = Math.Max(amount - Armour, 1);
        Console.WriteLine($"The armour absorbs {amount - reduced} damage.");
        base.TakeDamage(reduced); // Appelle la version de base d'Enemy
    }

    public override void Attack(Player target, Random rng)
    {
        Console.WriteLine("Boss slams the ground!");
        target.TakeDamage(rng.Next(9, 17));
    }
}