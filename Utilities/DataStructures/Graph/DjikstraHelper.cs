namespace Utilities.DataStructures.Graph;

public static class DjikstraHelper
{
    public static Dictionary<TNodeKey, int> Unweighted<TNodeKey>(TNodeKey start,
        Dictionary<TNodeKey, HashSet<TNodeKey>> adjacencyList) where TNodeKey : notnull
    {
        var unvisited = new HashSet<TNodeKey>(adjacencyList.Keys);
        var distances = unvisited.ToDictionary(n => n, _ => int.MaxValue);

        distances[start] = 0;

        for (var i = 0; i < distances.Count; i++)
        {
            var current = GetClosestUnvisited(distances, unvisited);
            unvisited.Remove(current);
            
            foreach (var neighbor in adjacencyList[current])
            {
                var distanceViaCurrent = distances[current] + 1; 
                if (distanceViaCurrent < distances[neighbor])
                {
                    distances[neighbor] = distanceViaCurrent;
                }
            }
        }

        return distances;
    }

    private static TNodeKey GetClosestUnvisited<TNodeKey>(Dictionary<TNodeKey, int> distances, IReadOnlySet<TNodeKey> unvisited) where TNodeKey : notnull
    {
        var min = int.MaxValue;
        var closest = default(TNodeKey);

        foreach (var (position, distance) in distances)
        {
            if (!unvisited.Contains(position) || distance > min)
            {
                continue;
            }
            
            min = distance;
            closest = position;
        }

        return closest!;
    }
}