namespace Problems.Y2018.D24;

public readonly record struct Attack(string Type, int Damage);
public readonly record struct Unit(int Hp, IReadOnlySet<string> Weaknesses, IReadOnlySet<string> Immunities, Attack Attack, int Initiative);
public readonly record struct Result(Resolution Resolution, Team Winner, int Score);