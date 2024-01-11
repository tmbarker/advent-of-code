namespace Utilities.Graph;

public static class GraphHelper
{
    /// <summary>
    /// Execute the Floyd-Warshall algorithm to find the shortest path from each vertex to all other vertices
    /// </summary>
    public static Dictionary<(T, T), int> FloydWarshallUnweighted<T>(
        IDictionary<T, HashSet<T>> adjacencyList) where T : notnull
    {
        const int inf = int.MaxValue / 2 - 1;
        var vertices = adjacencyList.Keys;
        var costs = vertices.ToDictionary(vertex => (vertex, vertex), _ => 0);
        
        foreach (var i in vertices)
        {
            var adjacencies = adjacencyList[i];
            foreach (var j in vertices.Where(j => !EqualityComparer<T>.Default.Equals(i, j)))
            {
                costs.Add((i, j), adjacencies.Contains(j) ? 1 : inf);
            }
        }
        
        foreach (var k in vertices)
        foreach (var i in vertices)
        foreach (var j in vertices)
        {
            if (costs[(i, j)] > costs[(i, k)] + costs[(k, j)])
            {
                costs[(i, j)] = costs[(i, k)] + costs[(k, j)];
            }
        }

        return costs;
    }
    
    /// <summary>
    /// Execute Dijkstra's algorithm to find the shortest path from the <paramref name="start"/> vertex to all
    /// other vertices
    /// </summary>
    public static Dictionary<T, int> DijkstraUnweighted<T>(T start,
        IDictionary<T, HashSet<T>> adjacencyList) where T : notnull
    {
        return DijkstraUnweighted(
            start: start,
            adjacencyList: adjacencyList,
            stopPredicate: null);
    }
    
    /// <summary>
    /// Execute Dijkstra's algorithm to find the shortest path from the <paramref name="start"/> vertex to
    /// the <paramref name="end"/> vertex
    /// </summary>
    public static int DijkstraUnweighted<T>(T start, T end, 
        IDictionary<T, HashSet<T>> adjacencyList) where T : notnull
    {
        var costs = DijkstraUnweighted(
            start: start,
            adjacencyList: adjacencyList,
            stopPredicate: key => EqualityComparer<T>.Default.Equals(end, key));

        return costs.GetValueOrDefault(end, int.MaxValue);
    }

    private static Dictionary<T, int> DijkstraUnweighted<T>(T start,
        IDictionary<T, HashSet<T>> adjacencyList, Predicate<T>? stopPredicate)
        where T : notnull
    {
        var heap = new PriorityQueue<T, int>(items: [(start, 0)]);
        var costs = adjacencyList.Keys.ToDictionary(
            keySelector: n => n,
            elementSelector: n => EqualityComparer<T>.Default.Equals(n, start) ? 0 : int.MaxValue);

        while (heap.Count > 0)
        {
            var current = heap.Dequeue();
            if (stopPredicate?.Invoke(current) ?? false)
            {
                break;
            }
            
            foreach (var neighbor in adjacencyList[current])
            {
                if (costs[current] + 1 < costs[neighbor])
                {
                    costs[neighbor] = costs[current] + 1;
                    heap.Enqueue(neighbor, costs[neighbor]);
                }
            }
        }

        return costs;
    }
}