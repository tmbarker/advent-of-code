namespace Solutions.Y2015.D21;

public static class Shop
{
    public static readonly HashSet<Gear> Weapons =
    [
        new Gear(Cost:  8, Damage: 4, Armor: 0),
        new Gear(Cost: 10, Damage: 5, Armor: 0),
        new Gear(Cost: 25, Damage: 6, Armor: 0),
        new Gear(Cost: 40, Damage: 7, Armor: 0),
        new Gear(Cost: 74, Damage: 8, Armor: 0)
    ];
    
    public static readonly HashSet<Gear> Armor =
    [
        new Gear(Cost:  13, Damage: 0, Armor: 1),
        new Gear(Cost:  31, Damage: 0, Armor: 2),
        new Gear(Cost:  53, Damage: 0, Armor: 3),
        new Gear(Cost:  75, Damage: 0, Armor: 4),
        new Gear(Cost: 102, Damage: 0, Armor: 5)
    ];
    
    public static readonly HashSet<Gear> Rings =
    [
        new Gear(Cost:  25, Damage: 1, Armor: 0),
        new Gear(Cost:  50, Damage: 2, Armor: 0),
        new Gear(Cost: 100, Damage: 3, Armor: 0),
        new Gear(Cost:  20, Damage: 0, Armor: 1),
        new Gear(Cost:  40, Damage: 0, Armor: 2),
        new Gear(Cost:  80, Damage: 0, Armor: 3)
    ];
}