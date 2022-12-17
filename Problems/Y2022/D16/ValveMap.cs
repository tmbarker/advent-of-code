using Utilities.DataStructures.Graph;
using Utilities.Extensions;

namespace Problems.Y2022.D16;

public class ValveMap
{
    public ICollection<string> Valves => FlowRates.Keys;
    public Dictionary<string, int> FlowRates { get; }
    public Dictionary<string, Dictionary<string, int>> TravelTimesLookup { get; }

    public ValveMap(string start, IDictionary<string, int> allFlowRates, Dictionary<string, HashSet<string>> allAdjacencies)
    {
        FlowRates = allFlowRates.WhereValues(r => r > 0);
        TravelTimesLookup = FormTravelTimesLookup(allAdjacencies).WhereKeys(v => FlowRates.ContainsKey(v) || v == start);
    }
    
    private static Dictionary<string, Dictionary<string, int>> FormTravelTimesLookup(Dictionary<string, HashSet<string>> allAdjacencies)
    {
        return allAdjacencies.Keys.ToDictionary(valve => valve, valve => DjikstraHelper.Unweighted(valve, allAdjacencies));
    }
}