using Utilities.Extensions;

namespace Solutions.Y2023.D24;

public readonly record struct Ray3(Vec3 S, Vec3 V)
{
    private const decimal EpsilonParallel = 1e-3m;
    
    public static bool Intersect2D(Ray3 a, Ray3 b, out Vec3 p)
    {
        var dx = b.S.X - a.S.X;
        var dy = b.S.Y - a.S.Y;
        var cp = b.V.X * a.V.Y - b.V.Y * a.V.X;
        
        if (Math.Abs(cp) < EpsilonParallel)
        {
            p = Vec3.Zero;
            return false;
        }
        
        var u = (dy * b.V.X - dx * b.V.Y) / cp;
        var v = (dy * a.V.X - dx * a.V.Y) / cp;
        
        p = a.GetPoint(u);
        return u >= 0 && v >= 0;
    }
    
    public static Ray3 Parse(string line)
    {
        var n = line.ParseLongs();
        var s = new Vec3(X: n[0], Y: n[1], Z: n[2]);
        var v = new Vec3(X: n[3], Y: n[4], Z: n[5]);
        
        return new Ray3(s, v);
    }
    
    private Vec3 GetPoint(decimal t)
    {
        return new Vec3(
            X: S.X + t * V.X,
            Y: S.Y + t * V.Y,
            Z: S.Z + t * V.Z);
    }
}