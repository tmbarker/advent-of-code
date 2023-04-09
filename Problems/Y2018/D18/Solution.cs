using System.Text;
using Problems.Common;
using Problems.Y2018.Common;
using Utilities.Cartesian;

namespace Problems.Y2018.D18;

/// <summary>
/// Settlers of The North Pole: https://adventofcode.com/2018/day/18
/// </summary>
public class Solution : SolutionBase2018
{
    private const char Ground = '.';
    private const char Trees = '|';
    private const char Lumberyard = '#';
    
    public override int Day => 18;
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var map = ParseResourceMap(input);
        
        return part switch
        {
            1 => EvolveNaive(map, minutes: 10),
            2 => EvolveCycle(map, minutes: 1000000000),
            _ => ProblemNotSolvedString
        };
    }

    private static int EvolveNaive(Dictionary<Vector2D, char> map, int minutes)
    {
        for (var i = 0; i < minutes; i++)
        {
            map = Evolve(map);
        }

        return ComputeResourceValue(map);
    }
    
    private static int EvolveCycle(Dictionary<Vector2D, char> map, int minutes)
    {
        var copy = new Dictionary<Vector2D, char>(map);
        var (start, length) = FindCycle(copy);

        return EvolveNaive(map, minutes: start + (minutes - start) % length);
    }

    private static (int Start, int Length) FindCycle(Dictionary<Vector2D, char> map)
    {
        var time = 0;
        var stateTimestamps = new Dictionary<string, int>();

        while (true)
        {
            map = Evolve(map);

            var key = BuildKey(map);
            if (stateTimestamps.TryGetValue(key, out var start))
            {
                return (start, time - start);
            }

            stateTimestamps[key] = time++;
        }
    }

    private static Dictionary<Vector2D, char> Evolve(Dictionary<Vector2D, char> map)
    {
        var next = new Dictionary<Vector2D, char>(capacity: map.Count);
        var aabb = new Aabb2D(extents: map.Keys, inclusive: true);
        
        foreach (var pos in aabb)
        {
            var adjPositions = pos.GetAdjacentSet(DistanceMetric.Chebyshev);
            var adjCounts = new Dictionary<char, int>
            {
                [Ground]     = adjPositions.Count(p => map.ContainsKey(p) && map[p] == Ground),
                [Trees]      = adjPositions.Count(p => map.ContainsKey(p) && map[p] == Trees),
                [Lumberyard] = adjPositions.Count(p => map.ContainsKey(p) && map[p] == Lumberyard)
            };

            next[pos] = map[pos] switch
            {
                Ground     => adjCounts[Trees] >= 3 ? Trees : Ground,
                Trees      => adjCounts[Lumberyard] >= 3 ? Lumberyard : Trees,
                Lumberyard => adjCounts[Lumberyard] >= 1 && adjCounts[Trees] >= 1 ? Lumberyard : Ground,
                _ => throw new NoSolutionException()
            };
        }

        return next;
    }

    private static int ComputeResourceValue(Dictionary<Vector2D, char> map)
    {
        var trees = map.Keys.Count(p => map[p] == Trees);
        var lumberyards = map.Keys.Count(p => map[p] == Lumberyard);

        return trees * lumberyards;
    }

    private static string BuildKey(Dictionary<Vector2D, char> map)
    {
        var aabb = new Aabb2D(extents: map.Keys, inclusive: true);
        var sb = new StringBuilder();
        
        for (var y = aabb.YMin; y <= aabb.YMax; y++)
        for (var x = aabb.XMin; x <= aabb.XMax; x++)
        {
            sb.Append(map[new Vector2D(x, y)]);
        }

        return sb.ToString();
    }
    
    private static Dictionary<Vector2D, char> ParseResourceMap(IList<string> input)
    {
        var rows = input.Count;
        var cols = input[0].Length;
        var map = new Dictionary<Vector2D, char>(capacity: rows * cols);

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            map[new Vector2D(x, y)] = input[y][x];
        }

        return map;
    }
}