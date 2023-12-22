using Utilities.Geometry.Euclidean;
using Utilities.Graph;

namespace Solutions.Y2023.D22;

[PuzzleInfo("Sand Slabs", Topics.Vectors|Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Vector3D Gravity = new(x: 0, y: 0, z: -1);
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var bricks = new Dictionary<int, Brick>();
        
        for (var i = 0; i < input.Length; i++)
        {
            bricks[i] = Brick.Parse(id: i, line: input[i]);
        }
        
        var supportGraph = BuildSupportGraph(bricks.Values);
        
        return part switch
        {
            1 => CountNonSupported(bricks.Keys, supportGraph),
            2 => CountAllSupported(bricks.Keys, supportGraph),
            _ => ProblemNotSolvedString
        };
    }

    private static DirectedGraph<int> BuildSupportGraph(IEnumerable<Brick> bricks)
    {
        var staticMap = new Dictionary<Vector3D, int>();
        var graph = new DirectedGraph<int>();
        var queue = new Queue<Brick>(collection: bricks.OrderBy(brick => brick.Extents.Min.Z));
        
        while (queue.Count > 0)
        {
            var brick = queue.Dequeue();
            var below = brick.Extents;
            
            while (below.Min.Z > 0 && !below.Any(staticMap.ContainsKey))
            {
                brick.Extents = below;
                below = below.Shift(amount: Gravity);
            }
            
            foreach (var pos in below)
            {
                if (staticMap.TryGetValue(pos, out var supportedBy))
                {
                    graph.AddEdge(from: brick.Id, to: supportedBy);
                }
            }
            
            foreach (var pos in brick.Extents)
            {
                staticMap[pos] = brick.Id;
            }
        }

        return graph;
    }
    
    private static int CountNonSupported(IEnumerable<int> bricks, DirectedGraph<int> graph)
    {
        return bricks.Count(id => graph.Incoming[id].All(supported => graph.Outgoing[supported].Count > 1));
    }
    
    private static int CountAllSupported(IEnumerable<int> bricks, DirectedGraph<int> graph)
    {
        var sum = 0;
        foreach (var id in bricks)
        {
            var queue = new Queue<int>(collection: [id]);
            var chain = new HashSet<int>(collection: [id]);

            while (queue.Count > 0)
            {
                foreach (var supported in graph.Incoming[queue.Dequeue()])
                {
                    if (graph.Outgoing[supported].All(chain.Contains))
                    {
                        queue.Enqueue(supported);
                        chain.Add(supported);
                    }
                }
            }

            sum += chain.Count - 1;
        }
        return sum;
    }
}