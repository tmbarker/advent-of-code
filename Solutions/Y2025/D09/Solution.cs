using Utilities.Geometry.Euclidean;

namespace Solutions.Y2025.D09;

[PuzzleInfo("Movie Theatre", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var vertices = ParseInputLines(Vec2D.Parse);
        return part switch
        {
            1 => Part1(vertices),
            2 => Part2(vertices),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Part1(Vec2D[] vertices)
    {
        var max = 0L;
        
        for (var i = 1; i < vertices.Length; i++)
        for (var j = 0; j < i;               j++)
        {
            var w = Math.Abs(vertices[j].X - vertices[i].X) + 1L;
            var h = Math.Abs(vertices[j].Y - vertices[i].Y) + 1L;
            max = Math.Max(max, w * h);
        }
        
        return max;
    }
    
    private static long Part2(Vec2D[] vertices)
    {
        var max = 0L;
        var polygon = new RectilinearPolygon2D(vertices);
        
        for (var i = 1; i < vertices.Length; i++)
        for (var j = 0; j < i;               j++)
        {
            var vi = vertices[i];
            var vj = vertices[j];
            var aabb = new Aabb2D(vi, vj);

            if (aabb.Area > max && polygon.Contains(aabb))
            {
                max = aabb.Area;
            }
        }

        return max;
    }
}

