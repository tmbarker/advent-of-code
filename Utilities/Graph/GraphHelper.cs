namespace Utilities.Graph;

public static class GraphHelper
{
    /// <summary>
    /// Execute Dijkstra's algorithm to find the shortest path from the <paramref name="start"/> node to all other nodes
    /// </summary>
    public static Dictionary<TNodeKey, int> DijkstraUnweighted<TNodeKey>(TNodeKey start,
        IDictionary<TNodeKey, HashSet<TNodeKey>> adjacencyList) where TNodeKey : notnull
    {
        var visited = new HashSet<TNodeKey> { start };
        var heap = new PriorityQueue<TNodeKey, int>(new[] { (start, 0) });
        var costs = adjacencyList.Keys.ToDictionary(
            keySelector: n => n,
            elementSelector: n => EqualityComparer<TNodeKey>.Default.Equals(n, start) ? 0 : int.MaxValue);

        while (heap.Count > 0)
        {
            var current = heap.Dequeue();
            foreach (var neighbor in adjacencyList[current])
            {
                if (visited.Contains(neighbor))
                {
                    continue;
                }

                var distanceViaCurrent = costs[current] + 1;
                if (distanceViaCurrent < costs[neighbor])
                {
                    costs[neighbor] = distanceViaCurrent;
                }

                visited.Add(neighbor);
                heap.Enqueue(neighbor, costs[neighbor]);
            }
        }

        return costs;
    }

    /// <summary>
    /// Execute the Floyd-Warshall algorithm to find the shortest path from each node to all other nodes
    /// </summary>
    public static Dictionary<(TNodeKey, TNodeKey), int> FloydWarshallUnweighted<TNodeKey>(
        IDictionary<TNodeKey, HashSet<TNodeKey>> adjacencyList) where TNodeKey : notnull
    {
        const int inf = int.MaxValue / 2 - 1;
        var nodes = adjacencyList.Keys;
        var costs = nodes.ToDictionary(node => (node, node), _ => 0);
        
        foreach (var i in nodes)
        {
            var adjacencies = adjacencyList[i];
            foreach (var j in nodes.Where(j => !EqualityComparer<TNodeKey>.Default.Equals(i, j)))
            {
                costs.Add((i, j), adjacencies.Contains(j) ? 1 : inf);
            }
        }
        
        foreach (var k in nodes)
        foreach (var i in nodes)
        foreach (var j in nodes)
        {
            if (costs[(i, j)] > costs[(i, k)] + costs[(k, j)])
            {
                costs[(i, j)] = costs[(i, k)] + costs[(k, j)];
            }
        }

        return costs;
    }
}