using Solutions.Y2019.IntCode;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;
using Utilities.Graph;

namespace Solutions.Y2019.D15;

using FieldMap = IDictionary<Vec2D, Tile>;
using CostsMap = IDictionary<Vec2D, int>;

[PuzzleInfo("Oxygen System", Topics.IntCode|Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : IntCodeSolution
{
    private static readonly Dictionary<Vec2D, long> Commands = new()
    {
        { Vec2D.Up,    1L },
        { Vec2D.Down,  2L },
        { Vec2D.Left,  3L },
        { Vec2D.Right, 4L }
    };

    private static readonly Dictionary<long, Tile> StatusCodes = new()
    {
        { 0L, Tile.Wall },
        { 1L, Tile.Empty },
        { 2L, Tile.Target }
    };

    public override object Run(int part)
    {
        var droid = IntCodeVm.Create(LoadIntCodeProgram());
        var map = BuildFieldMap(droid);
        
        return part switch
        {
            1 => ComputeCostToTarget(map),
            2 => ComputeMaxCostFromTarget(map),
            _ => PuzzleNotSolvedString
        };
    }

    private static int ComputeCostToTarget(FieldMap fieldMap)
    {
        var targetPos = fieldMap.Single(kvp => kvp.Value == Tile.Target).Key;
        var costs = BuildCostsMap(fieldMap, Vec2D.Zero);
        
        return costs[targetPos];
    }
    
    private static int ComputeMaxCostFromTarget(FieldMap fieldMap)
    {
        var targetPos = fieldMap.Single(kvp => kvp.Value == Tile.Target).Key;
        var costs = BuildCostsMap(fieldMap, targetPos);
        
        return costs.Values.Max();
    }
    
    private static FieldMap BuildFieldMap(IntCodeVm droid)
    {
        var start = Vec2D.Zero;
        var map = new Dictionary<Vec2D, Tile>
        {
            [start] = Tile.Empty
        };

        Traverse(
            droid: droid,
            pos: start,
            moveHistory: new Stack<Vec2D>(),
            map: map);

        return map;
    }

    private static CostsMap BuildCostsMap(FieldMap fieldMap, Vec2D from)
    {
        var adjacency = fieldMap
            .WhereValues(t => t != Tile.Wall)
            .ToDictionary(kvp => kvp.Key, kvp =>
            {
                return new HashSet<Vec2D>(kvp.Key.GetAdjacentSet(Metric.Taxicab)
                    .Where(p => fieldMap.ContainsKey(p) && fieldMap[p] != Tile.Wall));
            });

        return GraphHelper.DijkstraUnweighted(from, adjacency);
    }

    private static void Traverse(IntCodeVm droid, Vec2D pos, Stack<Vec2D> moveHistory, FieldMap map)
    {
        var moves = Vec2D.Zero.GetAdjacentSet(Metric.Taxicab);
        foreach (var move in moves)
        {
            if (map.ContainsKey(pos + move))
            {
                continue;
            }
            
            droid.InputBuffer.Enqueue(Commands[move]);
            droid.Run();
            
            map[pos + move] = StatusCodes[droid.OutputBuffer.Dequeue()];
            if (map[pos + move] == Tile.Wall)
            {
                continue;
            }
            
            moveHistory.Push(move);
            Traverse(droid, pos + move, moveHistory, map);
            moveHistory.Pop();

            droid.InputBuffer.Enqueue(Commands[-1 * move]);
            droid.Run();
            droid.OutputBuffer.Dequeue();
        }
    }
}