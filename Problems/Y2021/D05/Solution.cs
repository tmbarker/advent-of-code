using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D05;

[PuzzleInfo("Hydrothermal Venture", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    { 
        return part switch
        {
            1 => CountDangerousLocations(ignoreDiagonals: true),
            2 => CountDangerousLocations(ignoreDiagonals: false),
            _ => ProblemNotSolvedString
        };
    }

    private int CountDangerousLocations(bool ignoreDiagonals)
    {
        var lines = ParseInputLines(parseFunc: ParseVertices);
        var ventMap = BuildVentMap(lines, ignoreDiagonals);

        return ventMap.Values.Count(v => v > 1);
    }
    
    private static IDictionary<Vector2D, int> BuildVentMap(IEnumerable<(Vector2D V1, Vector2D V2)> lines, bool ignoreDiagonals)
    {
        var map = new DefaultDict<Vector2D, int>(defaultValue: 0);

        foreach (var (v1, v2) in lines)
        {
            if (ignoreDiagonals && v1.X != v2.X && v1.Y != v2.Y)
            {
                continue;
            }
            
            map[v2]++;
            
            var current = v1;
            var step = Vector2D.Normalize(v2 - v1);

            while (current != v2)
            {
                map[current]++;
                current += step;
            }
        }

        return map;
    }

    private static (Vector2D V1, Vector2D V2) ParseVertices(string line)
    {
        var parts = line.Split(separator: "->");
        return (V1: Vector2D.Parse(parts[0]), V2: Vector2D.Parse(parts[1]));
    }
}