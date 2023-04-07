using Utilities.Graph;

namespace Problems.Y2017.D07;

public readonly struct Tower
{
    private DirectedGraph<string> Graph { get; }
    private Dictionary<string, int> NaiveWeights { get; }
    
    public string Root => Graph.Sources.Single();
    
    public Tower(DirectedGraph<string> graph, Dictionary<string, int> naiveWeights)
    {
        Graph = graph;
        NaiveWeights = naiveWeights;
    }
    
    public int Balance()
    {
        var unbalanced = string.Empty;
        var weightMemo = new Dictionary<string, int>();
        var queue = new Queue<string>(new[] { Root });

        while (queue.Any())
        {
            var current = queue.Dequeue();
            var balanced = IsBalanced(current, weightMemo);

            if (!balanced)
            {
                unbalanced = current;
            }
            
            foreach (var child in Graph.Outgoing[current])
            {
                queue.Enqueue(child);
            }
        }

        return GetWeightToBalance(unbalanced, weightMemo);
    }

    private int GetWeightToBalance(string unbalanced, IDictionary<string, int> weightMemo)
    {
        var weightCounts = new Dictionary<int, int>();
        var childWeights = new Dictionary<int, HashSet<string>>();

        foreach (var child in Graph.Outgoing[unbalanced])
        {
            var weight = GetWeight(child, weightMemo);
            
            weightCounts.TryAdd(weight, 0);
            weightCounts[weight]++;

            childWeights.TryAdd(weight, new HashSet<string>());
            childWeights[weight].Add(child);
        }
        
        var targetWeight = weightCounts.Keys.MaxBy(w => weightCounts[w]);
        var currentWeight = weightCounts.Keys.MinBy(w => weightCounts[w]);
        var delta = targetWeight - currentWeight;

        return NaiveWeights[childWeights[currentWeight].Single()] + delta;
    }

    private bool IsBalanced(string node, IDictionary<string, int> weightMemo)
    {
        if (!Graph.Outgoing[node].Any())
        {
            return true;
        }

        var childWeights = new HashSet<int>();
        foreach (var child in Graph.Outgoing[node])
        {
            childWeights.Add(GetWeight(child, weightMemo));
        }

        return childWeights.Count == 1;
    }

    private int GetWeight(string node, IDictionary<string, int> weightMemo)
    {
        if (weightMemo.TryGetValue(node, out var cachedWeight))
        {
            return cachedWeight;
        }

        var weight = NaiveWeights[node];
        foreach (var child in Graph.Outgoing[node])
        {
            weight += GetWeight(child, weightMemo);
        }

        weightMemo[node] = weight;
        return weight;
    }
}