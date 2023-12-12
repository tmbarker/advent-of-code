using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D24;

public readonly struct Blizzard(Vector2D pos, Vector2D vel, Vector2D respawnAt)
{
    private Vector2D Vel { get; } = vel;
    
    public Vector2D Pos { get; } = pos;
    public Vector2D RespawnAt { get; } = respawnAt;
    public Vector2D Ahead => Pos + Vel;
    
    public Blizzard Step()
    {
        return new Blizzard(pos: Ahead, vel: Vel, RespawnAt);
    }

    public Blizzard Respawn()
    {
        return new Blizzard(pos: RespawnAt, vel: Vel, RespawnAt);
    }
}