using Problems.Attributes;
using Problems.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2016.D22;

/// <summary>
/// Grid Computing: https://adventofcode.com/2016/day/22
/// </summary>
[Favourite("Grid Computing", Topics.Graphs, Difficulty.Hard)]
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var nodes = input[2..].Select(ParseNode).ToList();
        
        return part switch
        {
            1 => CountViable(nodes),
            2 => MoveData(nodes),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountViable(IList<Node> nodes)
    {
        return nodes.Sum(n1 => nodes.Count(n2 => AreViable(a: n1, b: n2)));
    }

    private static int MoveData(IList<Node> nodes)
    {
        var map = nodes
            .ToDictionary(node => node.Pos)
            .WhereValues(node => nodes.All(other => other.Size >= node.Used));
        var initialEmpty = nodes
            .Where(node => node.Used == 0)
            .Select(node => node.Pos)
            .ToList();
        var initialState = new State(
            targetData: new Vector2D(x: nodes.Max(node => node.Pos.X), y: 0),
            emptyNodes: initialEmpty);

        var queue = new Queue<State>(new[] { initialState });
        var visited = new HashSet<State>(new[] { initialState });
        var depth = 0;
        
        while (queue.Any())
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var state = queue.Dequeue();
                if (state.TargetData == Vector2D.Zero)
                {
                    return depth;
                }

                var adjacent = GetAdjacentStates(state, map);
                var unvisited = adjacent.Where(adj => !visited.Contains(adj));

                foreach (var adj in unvisited)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        throw new NoSolutionException();
    }

    private static IEnumerable<State> GetAdjacentStates(State state, Dictionary<Vector2D, Node> map)
    {
        foreach (var empty in state.EmptyNodes)
        foreach (var adjacent in empty.GetAdjacentSet(Metric.Taxicab))
        {
            if (map.ContainsKey(adjacent) && !state.EmptyNodes.Contains(adjacent))
            {
                yield return state.AfterDataMove(from: adjacent, to: empty);
            }
        }
    }

    private static bool AreViable(Node a, Node b)
    {
        return a.Used > 0 && a.Pos != b.Pos && a.Used <= b.Avail;
    } 
    
    private static Node ParseNode(string line)
    {
        var numbers = line.ParseInts();
        var pos = new Vector2D(
            x: numbers[0], 
            y: numbers[1]);

        return new Node(
            Pos: pos,
            Size: numbers[2],
            Used: numbers[3],
            Avail: numbers[4]);
    }
    
    private readonly record struct Node(Vector2D Pos, int Size, int Used, int Avail);
}