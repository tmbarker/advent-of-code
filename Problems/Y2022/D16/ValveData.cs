using System.Text.RegularExpressions;
using Utilities.DataStructures.Graph;
using Utilities.Extensions;

namespace Problems.Y2022.D16;

public class ValveData
{
    private const string InputRegex = @".*([A-Z]{2}).*=(\d+);.*valves?(.*)";
    private const char Delimiter = ',';
    
    public IEnumerable<string> Valves => FlowRates.Keys;
    public Dictionary<string, int> FlowRates { get; }
    public Dictionary<string, Dictionary<string, int>> TravelTimesLookup { get; }

    public static ValveData Parse(IEnumerable<string> input, string start)
    {
        var flowRates = new Dictionary<string, int>();
        var tunnelAdjacencies = new Dictionary<string, HashSet<string>>();

        foreach (var line in input)
        {
            var (id, flowRate, adjacent) = ParseLine(line);
            flowRates.Add(id, flowRate);
            tunnelAdjacencies.Add(id, new HashSet<string>(adjacent));
        }

        return new ValveData(start, flowRates, tunnelAdjacencies);
    }
    
    private ValveData(string start, IDictionary<string, int> flowRates, Dictionary<string, HashSet<string>> adjacencies)
    {
        FlowRates = flowRates.WhereValues(r => r > 0);
        TravelTimesLookup = FormTravelTimesLookup(adjacencies).WhereKeys(v => FlowRates.ContainsKey(v) || v == start);
    }
    
    private static Dictionary<string, Dictionary<string, int>> FormTravelTimesLookup(Dictionary<string, HashSet<string>> adjacencies)
    {
        return adjacencies.Keys.ToDictionary(valve => valve, valve => DjikstraHelper.Unweighted(valve, adjacencies));
    }
    
    private static (string id, int flowRate, string[] adjacent) ParseLine(string line)
    {
        var matches = Regex.Match(line, InputRegex);
        
        var valveId = matches.Groups[1].Value;
        var flowRate = int.Parse(matches.Groups[2].Value);
        var adjacent = matches.Groups[3].Value;
        var adjacentIds = adjacent.Split(Delimiter, StringSplitOptions.TrimEntries);

        return (valveId, flowRate, adjacentIds);
    }
}