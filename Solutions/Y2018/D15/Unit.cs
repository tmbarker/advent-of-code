using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D15;

public sealed class Unit
{
    public int Id { get; }
    public char Team { get; }
    public int Dmg { get; }
    public int Hp { get; private set; }
    public Vec2D Pos { get; set; }

    public bool Dead => Hp <= 0;
    
    public Unit(int id, char team, int dmg, int hp, Vec2D pos)
    {
        Id = id;
        Team = team;
        Dmg = dmg;
        Hp = hp;
        Pos = pos;
    }

    public void InflictDamage(int dmg)
    {
        Hp = Math.Max(0, Hp - dmg);
    }
}