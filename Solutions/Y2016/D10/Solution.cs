using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2016.D10;

[PuzzleInfo("Balance Bots", Topics.RegularExpressions, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const string SourceMarker = "value";
    private static readonly Regex SourceRegex = new(@"^value (?<V>\d+) goes to (?<A>[a-z 0-9]+)$");
    private static readonly Regex ConnectionRegex =
        new(@"^(?<A>[a-z 0-9]+) gives.*to (?<B>[a-z 0-9]+) and.*to (?<C>[a-z 0-9]+)$");
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var map = BuildMap(input);

        return part switch
        {
            1 => FindNode(map, v1: 61, v2: 17),
            2 => ComputeOutputProduct(map, outputIds: [0, 1, 2]),
            _ => PuzzleNotSolvedString
        };
    }

    private static int FindNode(Dictionary<string, Node> map, int v1, int v2)
    {
        return map
            .Values.Single(n => n.Values.Contains(v1) && n.Values.Contains(v2))
            .Id.ParseInt();
    }

    private static int ComputeOutputProduct(Dictionary<string, Node> map, HashSet<int> outputIds)
    {
        return map
            .Values.Where(n => n.Values.Count == 1 && outputIds.Contains(n.Id.ParseInt()))
            .Aggregate(seed: 1, func: (product, node) => product * node.Values.Single());
    }
    
    private static Dictionary<string, Node> BuildMap(IList<string> input)
    {
        var map = new Dictionary<string, Node>();
        var queue = new Queue<string>();

        foreach (var line in input.Where(l => !l.Contains(SourceMarker)))
        {
            var match = ConnectionRegex.Match(line);
            var nodeId = match.Groups["A"].Value;
            var toLoId = match.Groups["B"].Value;
            var toHiId = match.Groups["C"].Value;

            map[nodeId] = new Node(nodeId) { Low = toLoId, High = toHiId };
            map.TryAdd(toLoId, new Node(toLoId));
            map.TryAdd(toHiId, new Node(toHiId));
        }

        foreach (var line in input.Where(l => l.Contains(SourceMarker)))
        {
            var match = SourceRegex.Match(line);
            var value = match.Groups["V"].ParseInt();
            var nodeId = match.Groups["A"].Value;
            var node = map[nodeId];

            node.Values.Add(value);
            if (node.Ready)
            {
                queue.Enqueue(nodeId);   
            }
        }

        while (queue.Count > 0)
        {
            var id = queue.Dequeue();
            var node = map[id];

            if (!node.Ready)
            {
                continue;
            }

            var low = map[node.Low!];
            low.Values.Add(node.Values.Min());
            if (low.Ready)
            {
                queue.Enqueue(low.Id);
            }
            
            var high = map[node.High!];
            high.Values.Add(node.Values.Max());
            if (high.Ready)
            {
                queue.Enqueue(high.Id);
            }
        }

        return map;
    }
}