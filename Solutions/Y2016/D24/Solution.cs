using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D24;

[PuzzleInfo("Air Duct Spelunking", Topics.Graphs|Topics.Recursion, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => Search(loop: false),
            2 => Search(loop: true),
            _ => PuzzleNotSolvedString
        };
    }

    private int Search(bool loop)
    {
        var map = GetInputLines();
        var pois = CollectPois(map);
        var costs = BuildCostLookup(map, pois);

        return GetMinCost(
            cost: 0,
            pos: 0,
            unvisited: pois.Keys.Except(single: 0).ToHashSet(),
            costs: costs,
            loop: loop);
    }

    private static int GetMinCost(int cost, int pos, HashSet<int> unvisited, IReadOnlyDictionary<(int, int), int> costs, bool loop)
    {
        if (unvisited.Count == 0)
        {
            return loop
                ? cost + costs[(pos, 0)]
                : cost;
        }

        var min = int.MaxValue;
        foreach (var poi in unvisited)
        {
            min = Math.Min(min, GetMinCost(
                cost: cost + costs[(pos, poi)],
                pos: poi,
                unvisited: unvisited.Except(poi).ToHashSet(),
                costs: costs,
                loop: loop));
        }

        return min;
    }
    
    private static Dictionary<int, Vec2D> CollectPois(string[] map)
    {
        var pois = new Dictionary<int, Vec2D>();
        var rows = map.Length;
        var cols = map[0].Length;

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (char.IsNumber(map[y][x]))
            {
                pois[map[y][x].AsDigit()] = new Vec2D(x, y);
            }
        }

        return pois;
    }

    private static Dictionary<(int, int), int> BuildCostLookup(IReadOnlyList<string> map, Dictionary<int, Vec2D> pois)
    {
        var lookup = new Dictionary<(int, int), int>();
        var poiKeys = pois.Keys;
        
        foreach (var key1 in poiKeys)
        foreach (var key2 in poiKeys)
        {
            if (key1 == key2)
            {
                lookup[(key1, key2)] = 0;
                continue;
            }

            var cost = ComputeCost(from: pois[key1], to: pois[key2], map);
            lookup[(key1, key2)] = cost;
            lookup[(key2, key1)] = cost;
        }

        return lookup;
    }

    private static int ComputeCost(Vec2D from, Vec2D to, IReadOnlyList<string> map)
    {
        var queue = new Queue<Vec2D>(collection: [from]);
        var visited = new HashSet<Vec2D>(collection: [from]);
        var depth = 0;
        
        while (queue.Count != 0)
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                if (pos == to)
                {
                    return depth;
                }

                var adjacent = pos
                    .GetAdjacentSet(Metric.Taxicab)
                    .Where(adj => PositionValid(adj, map))
                    .Where(adj => !visited.Contains(adj));

                foreach (var adj in adjacent)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        throw new NoSolutionException();
    }

    private static bool PositionValid(Vec2D pos, IReadOnlyList<string> map)
    {
        return
            pos.Y >= 0 && pos.Y < map.Count &&
            pos.X >= 0 && pos.X < map[0].Length &&
            map[pos.Y][pos.X] != '#';
    }
}