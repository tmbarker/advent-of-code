using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D24;

public readonly struct Blizzard(Vec2D pos, Vec2D vel, Vec2D respawnAt)
{
    private Vec2D Vel { get; } = vel;
    
    public Vec2D Pos { get; } = pos;
    public Vec2D RespawnAt { get; } = respawnAt;
    public Vec2D Ahead => Pos + Vel;
    
    public Blizzard Step()
    {
        return new Blizzard(pos: Ahead, vel: Vel, RespawnAt);
    }

    public Blizzard Respawn()
    {
        return new Blizzard(pos: RespawnAt, vel: Vel, RespawnAt);
    }
}