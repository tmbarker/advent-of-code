using Problems.Y2021.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2021.D05;

/// <summary>
/// Hydrothermal Venture: https://adventofcode.com/2021/day/5
/// </summary>
public class Solution : SolutionBase2021
{
    private const string VertexDelimiter = "->";
    private const string CoordinateDelimiter = ",";

    public override int Day => 5;
    
    public override object Run(int part)
    { 
        return part switch
        {
            0 => CountDangerousLocations(true),
            1 => CountDangerousLocations(false),
            _ => ProblemNotSolvedString,
        };
    }

    private int CountDangerousLocations(bool ignoreDiagonals)
    {
        var lines = ParseVentLineVertices(GetInputLines());
        var ventMap = BuildVentMap(lines, ignoreDiagonals);

        return ventMap.Values.Count(v => v > 1);
    }
    
    private static Dictionary<Vector2D, int> BuildVentMap(IEnumerable<(Vector2D v1, Vector2D v2)> lines, bool ignoreDiagonals)
    {
        var dictionary = new Dictionary<Vector2D, int>();

        foreach (var (v1, v2) in lines)
        {
            if (v1.IsDiagonalTo(v2) && ignoreDiagonals)
            {
                continue;
            }
            
            dictionary.EnsureContainsKey(v2);
            dictionary[v2]++;
            
            var current = v1;
            var step = Vector2D.Normalize(v2 - v1);

            while (current != v2)
            {
                dictionary.EnsureContainsKey(current);
                dictionary[current]++;
                
                current += step;
            }
        }

        return dictionary;
    }

    private static IEnumerable<(Vector2D v1, Vector2D v2)> ParseVentLineVertices(IEnumerable<string> input)
    {
        return input.Select(ParseVentLineVertices);
    }

    private static (Vector2D v1, Vector2D v2) ParseVentLineVertices(string line)
    {
        var parts = line.Split(VertexDelimiter, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
        return (ParseVertex(parts[0]), ParseVertex(parts[1]));
    }

    private static Vector2D ParseVertex(string coordinates)
    {
        var parts = coordinates.Split(CoordinateDelimiter);
        var x = int.Parse(parts[0]);
        var y = int.Parse(parts[1]);

        return new Vector2D(x, y);
    }
}