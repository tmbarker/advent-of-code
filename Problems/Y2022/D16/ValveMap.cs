using Utilities.Extensions;

namespace Problems.Y2022.D16;

public class ValveMap
{
    public Dictionary<string, int> FlowRates { get; }
    public Dictionary<string, int> NonZeroFlowRates { get; }
    public Dictionary<string, HashSet<string>> Adjacencies { get; }
    public Dictionary<string, Dictionary<string, int>> ValveTravelTimesLookup { get; }

    public ValveMap(Dictionary<string, int> flowRates, Dictionary<string, HashSet<string>> adjacencies)
    {
        FlowRates = flowRates;
        NonZeroFlowRates = FlowRates.WhereValues(r => r > 0);
        Adjacencies = adjacencies;
        ValveTravelTimesLookup = FormTravelTimesLookup();
    }
    
    private Dictionary<string, Dictionary<string, int>> FormTravelTimesLookup()
    {
        return Adjacencies.Keys.ToDictionary(valve => valve, FormShortestPathsMap);
    }
    
    private Dictionary<string, int> FormShortestPathsMap(string start)
    {
        var allValves = Adjacencies.Keys.ToHashSet();
        allValves.EnsureContains(start);
        
        var unvisited = new HashSet<string>(allValves);
        var distances = allValves.ToDictionary(n => n, _ => int.MaxValue);

        distances[start] = 0;

        for (var i = 0; i < allValves.Count; i++)
        {
            var current = GetClosestUnvisited(distances, unvisited);
            unvisited.Remove(current);
            
            foreach (var neighbor in Adjacencies[current])
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

    private static string GetClosestUnvisited(Dictionary<string, int> distances, IReadOnlySet<string> unvisited)
    {
        var min = int.MaxValue;
        var closest = string.Empty;

        foreach (var (position, distance) in distances)
        {
            if (!unvisited.Contains(position) || distance > min)
            {
                continue;
            }
            
            min = distance;
            closest = position;
        }

        return closest;
    }
}