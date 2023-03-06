using Utilities.Cartesian;

namespace Problems.Y2018.D15;

public class Unit
{
    public int Id { get; }
    public char Team { get; }
    public int Dmg { get; }
    public int Hp { get; private set; }
    public Vector2D Pos { get; set; }

    public bool Dead => Hp <= 0;
    
    public Unit(int id, char team, int dmg, int hp, Vector2D pos)
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