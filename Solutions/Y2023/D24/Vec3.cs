namespace Solutions.Y2023.D24;

public readonly record struct Vec3(decimal X, decimal Y, decimal Z)
{
    public static readonly Vec3 Zero = new(X: 0, Y: 0, Z: 0);
}