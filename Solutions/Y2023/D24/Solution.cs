using System.Runtime.CompilerServices;

namespace Solutions.Y2023.D24;

[PuzzleInfo("Never Tell Me The Odds", Topics.Vectors|Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const double EpsilonParallel = 1e-3;
    
    public override object Run(int part)
    {
        var rays = GetInputLines()
            .Select(Ray3.Parse)
            .ToArray();
        
        return part switch
        {
            1 => Intersect2D(rays, aabb: new Aabb2(Min: 2e14, Max: 4e14)),
            _ => ProblemNotSolvedString
        };
    }

    private static int Intersect2D(IReadOnlyList<Ray3> rays, Aabb2 aabb)
    {
        var n = 0;
        
        for (var i = 0; i < rays.Count - 1; i++)
        for (var j = i + 1; j < rays.Count; j++)
        {
            if (Intersect(a: rays[i], b: rays[j], out var p) && aabb.Contains(p))
            {
                n++;
            }   
        }

        return n;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool Intersect(Ray3 a, Ray3 b, out Vec3 p)
    {
        var dx = b.S.X - a.S.X;
        var dy = b.S.Y - a.S.Y;
        var cp = b.D.X * a.D.Y - b.D.Y * a.D.X;
        
        if (Math.Abs(cp) < EpsilonParallel)
        {
            p = Vec3.Zero;
            return false;
        }
        
        var u = (dy * b.D.X - dx * b.D.Y) / cp;
        var v = (dy * a.D.X - dx * a.D.Y) / cp;
        
        p = a.GetPoint(u);
        return u >= 0 && v >= 0;
    }
}