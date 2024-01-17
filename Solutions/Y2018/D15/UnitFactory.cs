using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D15;

public static class UnitFactory
{
    public static Unit Build(int id, char team, int dmgBuff, Vec2D pos)
    {
        return new Unit(
            id: id,
            team: team,
            pos: pos,
            dmg: GameData.Dmg + dmgBuff,
            hp: GameData.Hp);
    }
}