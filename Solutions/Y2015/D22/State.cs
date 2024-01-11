namespace Solutions.Y2015.D22;

public readonly struct State(Wizard player, Boss boss, int manaUsed, Dictionary<Spell, int> activeEffects)
    : IEquatable<State>
{
    private readonly string _key = $"[Player: Hp={player.Hp},M={player.Mana},U={manaUsed}]" +
                                   $"[Boss: Hp={boss.Hp}]" +
                                   $"[Effects: {string.Join(',', BuildActiveEffectsHash(activeEffects))}]";
    public Wizard Player { get; } = player;
    public Boss Boss { get; } = boss;
    public int ManaUsed { get; } = manaUsed;
    public Dictionary<Spell, int> ActiveEffects { get; } = activeEffects;

    public bool IsResolved => Player.Hp <= 0 || Boss.Hp <= 0;

    public static State Initial(Wizard player, Boss boss)
    {
        return new State(player, boss, manaUsed: 0, activeEffects: new Dictionary<Spell, int>());
    }

    private static string BuildActiveEffectsHash(Dictionary<Spell, int> effects)
    {
        return string.Concat(", ", effects
            .Select(kvp => $"{kvp.Key}={kvp.Value}")
            .Order());
    }

    public bool Equals(State other)
    {
        return _key == other._key;
    }

    public override bool Equals(object? obj)
    {
        return obj is State other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _key.GetHashCode();
    }

    public static bool operator ==(State left, State right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(State left, State right)
    {
        return !left.Equals(right);
    }
}