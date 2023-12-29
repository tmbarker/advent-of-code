namespace Solutions.Y2023.D24;

public readonly record struct Aabb2(decimal Min, decimal Max)
{
    public bool Contains(Vec3 p)
    {
        return p.X >= Min && p.X <= Max &&
               p.Y >= Min && p.Y <= Max;
    }
}