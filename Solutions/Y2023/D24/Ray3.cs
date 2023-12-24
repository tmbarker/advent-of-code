using Utilities.Extensions;

namespace Solutions.Y2023.D24;

public readonly record struct Ray3(Vec3 S, Vec3 D)
{
    public Vec3 GetPoint(double t)
    {
        return new Vec3(
            X: S.X + t * D.X,
            Y: S.Y + t * D.Y,
            Z: S.Z + t * D.Z);
    }
        
    public static Ray3 Parse(string line)
    {
        var n = line.ParseLongs();
        var s = new Vec3(X: n[0], Y: n[1], Z: n[2]);
        var d = new Vec3(X: n[3], Y: n[4], Z: n[5]);
        
        return new Ray3(s, d);
    }
}