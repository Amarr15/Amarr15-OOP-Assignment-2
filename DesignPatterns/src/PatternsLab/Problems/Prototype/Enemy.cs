namespace PatternsLab.Problems.Prototype;

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
}

public abstract class Enemy
{
    private string _modelData;

    public string Name { get; set; }
    public int Health { get; set; }
    public Weapon Weapon { get; set; }
    public List<string> Abilities { get; set; } = new();

    public string ModelId => _modelData;

    protected Enemy()
    {
        Console.WriteLine("   ...loading 3D model (slow)...");
        Thread.Sleep(500);

        _modelData = "MODEL_" +
                     Guid.NewGuid().ToString("N")[..6];
    }

    protected Enemy(Enemy other)
    {
        _modelData = other._modelData;

        Name = other.Name;
        Health = other.Health;

        Weapon = new Weapon
        {
            Name = other.Weapon.Name,
            Damage = other.Weapon.Damage
        };

        Abilities = new List<string>(other.Abilities);
    }

    public abstract Enemy Clone();
}


public class Orc : Enemy
{
    public Orc()
    {
        Name = "Orc";
        Health = 100;

        Weapon = new Weapon
        {
            Name = "Axe",
            Damage = 25
        };

        Abilities.Add("Rage");
    }

    private Orc(Orc other) : base(other)
    {
    }

    public override Enemy Clone()
    {
        return new Orc(this);
    }
}


public class Elf : Enemy
{
    public Elf()
    {
        Name = "Elf";
        Health = 70;

        Weapon = new Weapon
        {
            Name = "Bow",
            Damage = 18
        };

        Abilities.Add("Stealth");
    }

    private Elf(Elf other) : base(other)
    {
    }

    public override Enemy Clone()
    {
        return new Elf(this);
    }
}


public static class EnemyCopyHelper
{
    public static Enemy CopyEnemy(Enemy e)
    {
        return e.Clone();
    }
}