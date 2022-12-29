namespace Utilities.DataStructures.Graph;

public static class DjikstraHelper
{
    public static Dictionary<TNodeKey, int> UnweightedFast<TNodeKey>(TNodeKey start,
        Dictionary<TNodeKey, HashSet<TNodeKey>> adjacencyList) where TNodeKey : notnull
    {
        var visited = new HashSet<TNodeKey>() { start };
        var heap = new PriorityQueue<TNodeKey, int>(Enumerable.Repeat((start, 0), 1));
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
}