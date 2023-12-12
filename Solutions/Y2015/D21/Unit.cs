namespace Solutions.Y2015.D21;

public readonly struct Unit
{
    public int Damage { get; }
    public int Armor { get; }
    public int GearCost { get; }
    public int Hp { get; }

    public bool Dead => Hp <= 0;

    private Unit(int damage, int armor, int gearCost, int hp)
    {
        Damage = damage;
        Armor = armor;
        GearCost = gearCost;
        Hp = hp;
    }

    public Unit InflictDamage(int damage)
    {
        return new Unit(
            damage: Damage,
            armor: Armor,
            gearCost: GearCost,
            hp: Math.Max(0, Hp - damage));
    }
    
    public static Unit Spawn(ICollection<Gear> gear, int hp)
    {
        return new Unit(
            damage: gear.Sum(g => g.Damage),
            armor: gear.Sum(g => g.Armor),
            gearCost: gear.Sum(g => g.Cost),
            hp: hp);
    }
    
    public static Unit Spawn(int damage, int armor, int hp)
    {
        return new Unit(damage, armor, gearCost: 0, hp);
    }
}