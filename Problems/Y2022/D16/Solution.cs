using System.Text.RegularExpressions;
using Problems.Y2022.Common;

namespace Problems.Y2022.D16;

/// <summary>
/// Proboscidea Volcanium: https://adventofcode.com/2022/day/16
/// </summary>
public class Solution : SolutionBase2022
{
    private const string InputRegex = @".*([A-Z]{2}).*=(\d+);.*valves?(.*)";
    private const char Delimiter = ',';
    
    private const string Start = "AA";
    private const int TimeLimitAlone = 30;

    public override int Day => 16;
    
    public override string Run(int part)
    {
        return part switch
        {
            0 => ComputeMaxPressureRelievedAlone(TimeLimitAlone).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int ComputeMaxPressureRelievedAlone(int timeLimit)
    {
        var valveMap = ParseValveMap(GetInput());
        var strategyFinder = new StrategyFinder(Start, timeLimit, valveMap);
        var maxFlowRelieved = 0;
        
        void OnStrategyFound(Strategy strategy)
        {
            var flowRelieved = EvaluateStrategy(strategy, timeLimit, valveMap.FlowRates);
            maxFlowRelieved = Math.Max(maxFlowRelieved, flowRelieved);
        }

        strategyFinder.StrategyFound += OnStrategyFound;
        strategyFinder.Run();

        return maxFlowRelieved;
    }

    private static int EvaluateStrategy(Strategy s, int timeLimit, IReadOnlyDictionary<string, int> flowRates)
    {
        var total = 0;
        foreach (var (valve, openedAt) in s.ValveOpeningTimings)
        {
            total += GetValveContribution(flowRates[valve], openedAt, timeLimit);
        }
        return total;;
    }

    private static int GetValveContribution(int flowRate, int openedAt, int timeLimit)
    {
        return flowRate * (timeLimit - openedAt);
    }
    
    private static ValveMap ParseValveMap(IEnumerable<string> input)
    {
        var flowRates = new Dictionary<string, int>();
        var tunnelAdjacencies = new Dictionary<string, HashSet<string>>();

        foreach (var line in input)
        {
            var (id, flowRate, adjacent) = ParseLine(line);
            flowRates.Add(id, flowRate);
            tunnelAdjacencies.Add(id, new HashSet<string>(adjacent));
        }

        return new ValveMap(flowRates, tunnelAdjacencies);
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