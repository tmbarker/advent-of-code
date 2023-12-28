namespace Solutions.Y2023.D24;

[PuzzleInfo("Never Tell Me The Odds", Topics.Vectors|Topics.Math, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var rays = GetInputLines()
            .Select(Ray3.Parse)
            .ToArray();
        
        return part switch
        {
            1 => Intersect2D(rays, aabb: new Aabb2(Min: 2e14, Max: 4e14)),
            2 => Intersect3D(rays),
            _ => ProblemNotSolvedString
        };
    }

    private static long Intersect2D(IReadOnlyList<Ray3> rays, Aabb2 aabb)
    {
        var n = 0L;
        
        for (var i = 0; i < rays.Count - 1; i++)
        for (var j = i + 1; j < rays.Count; j++)
        {
            if (Ray3.Intersect2D(a: rays[i], b: rays[j], out var p) && aabb.Contains(p))
            {
                n++;
            }
        }

        return n;
    }

    private static long Intersect3D(IReadOnlyList<Ray3> rays)
    {
        throw new NoSolutionException();
    } 
}