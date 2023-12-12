using System.Text.RegularExpressions;
using Utilities.Extensions;
using Utilities.Graph;

namespace Problems.Y2022.D16;

public sealed class ValveData
{
    private const string InputRegex = @".*([A-Z]{2}).*=(\d+);.*valves?(.*)";
    private const char Delimiter = ',';
    
    public IEnumerable<string> Valves => FlowRates.Keys;
    public IDictionary<string, int> FlowRates { get; }
    public IDictionary<(string, string), int> TravelTimesLookup { get; }

    public static ValveData Parse(IEnumerable<string> input)
    {
        var flowRates = new Dictionary<string, int>();
        var tunnelAdjacencies = new Dictionary<string, HashSet<string>>();

        foreach (var line in input)
        {
            var (id, flows, adjacencies) = ParseLine(line);
            flowRates.Add(id, flows);
            tunnelAdjacencies.Add(id, [..adjacencies]);
        }

        return new ValveData(flowRates, tunnelAdjacencies);
    }
    
    private ValveData(IDictionary<string, int> flowRates, IDictionary<string, HashSet<string>> adjacencies)
    {
        FlowRates = flowRates.WhereValues(r => r > 0);
        TravelTimesLookup = GraphHelper.FloydWarshallUnweighted(adjacencies);
    }

    private static (string id, int flowRate, string[] adjacent) ParseLine(string line)
    {
        var matches = Regex.Match(line, InputRegex);
        
        var valveId = matches.Groups[1].Value;
        var flowRate = matches.Groups[2].ParseInt();
        var adjacent = matches.Groups[3].Value;
        var adjacentIds = adjacent.Split(Delimiter, StringSplitOptions.TrimEntries);

        return (valveId, flowRate, adjacentIds);
    }
}