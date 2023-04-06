namespace Problems.Y2018.D24;

using GroupKey = ValueTuple<Team, int>;

public readonly struct Group
{
    public Team Team { get; }
    public int Id { get; }
    public int Size { get; }
    public Unit Units { get; }

    public GroupKey Ck => (Team, Id);
    public int EffectivePower => Size * Units.Attack.Damage;
    public bool Dead => Size <= 0;
    
    public Group(Team team, int id, Unit units, int size)
    {
        Team = team;
        Id = id;
        Size = size;
        Units = units;
    }

    public override string ToString()
    {
        return $"{Team} Group {Id}";
    }
}