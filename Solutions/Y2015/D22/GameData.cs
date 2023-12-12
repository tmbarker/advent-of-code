namespace Solutions.Y2015.D22;

public static class GameData
{
    public static readonly HashSet<Spell> Spells =
    [
        Spell.MagicMissile,
        Spell.Drain,
        Spell.Shield,
        Spell.Poison,
        Spell.Recharge
    ];
    
    public static readonly Dictionary<Spell, int> SpellCosts = new()
    {
        { Spell.MagicMissile,  53 },
        { Spell.Drain,         73 },
        { Spell.Shield,       113 },
        { Spell.Poison,       173 },
        { Spell.Recharge,     229 }
    };
    
    public static readonly Dictionary<Spell, int> SpellDurations = new()
    {
        { Spell.MagicMissile, 0 },
        { Spell.Drain,        0 },
        { Spell.Shield,       6 },
        { Spell.Poison,       6 },
        { Spell.Recharge,     5 }
    };
}