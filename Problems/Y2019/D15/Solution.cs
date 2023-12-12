using Problems.Y2019.IntCode;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;
using Utilities.Graph;

namespace Problems.Y2019.D15;

using FieldMap = IDictionary<Vector2D, Tile>;
using CostsMap = IDictionary<Vector2D, int>;

[PuzzleInfo("Oxygen System", Topics.IntCode|Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : IntCodeSolution
{
    private static readonly Dictionary<Vector2D, long> Commands = new()
    {
        { Vector2D.Up,    1L },
        { Vector2D.Down,  2L },
        { Vector2D.Left,  3L },
        { Vector2D.Right, 4L }
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
            _ => ProblemNotSolvedString
        };
    }

    private static int ComputeCostToTarget(FieldMap fieldMap)
    {
        var targetPos = fieldMap.Single(kvp => kvp.Value == Tile.Target).Key;
        var costs = BuildCostsMap(fieldMap, Vector2D.Zero);
        
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
        var start = Vector2D.Zero;
        var map = new Dictionary<Vector2D, Tile>
        {
            [start] = Tile.Empty
        };

        Traverse(
            droid: droid,
            pos: start,
            moveHistory: new Stack<Vector2D>(),
            map: map);

        return map;
    }

    private static CostsMap BuildCostsMap(FieldMap fieldMap, Vector2D from)
    {
        var adjacency = fieldMap
            .WhereValues(t => t != Tile.Wall)
            .ToDictionary(kvp => kvp.Key, kvp =>
            {
                return new HashSet<Vector2D>(kvp.Key.GetAdjacentSet(Metric.Taxicab)
                    .Where(p => fieldMap.ContainsKey(p) && fieldMap[p] != Tile.Wall));
            });

        return GraphHelper.DijkstraUnweighted(from, adjacency);
    }

    private static void Traverse(IntCodeVm droid, Vector2D pos, Stack<Vector2D> moveHistory, FieldMap map)
    {
        var moves = Vector2D.Zero.GetAdjacentSet(Metric.Taxicab);
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