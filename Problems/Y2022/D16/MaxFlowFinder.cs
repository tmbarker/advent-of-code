using Utilities.Extensions;

namespace Problems.Y2022.D16;

public class MaxFlowFinder
{
    private readonly ValveData _valveData;

    public MaxFlowFinder(ValveData valveData)
    {
        _valveData = valveData;
    }

    public int Run(string start, int timeLimit, bool withHelp)
    {
        return FindMaxFlow(timeLimit, start, new HashSet<string>(_valveData.Valves), withHelp);
    }

    private int FindMaxFlow(int t, string pos, HashSet<string> unopened, bool helper = false)
    {
        var max = 0;
        foreach (var valve in unopened.Freeze())
        {
            var travelAndOpenCost = GetTravelTime(pos, valve) + 1;
            if (travelAndOpenCost >= t)
            {
                continue;
            }

            var timeAtNextValve = t - travelAndOpenCost;
            var flow = GetFlowRate(valve) * timeAtNextValve + FindMaxFlow(
                t: timeAtNextValve,
                pos: valve,
                unopened: FormUnopenedSet(unopened, valve));
            
            if (helper)
            {
                flow += FindMaxFlow(
                    t: t,
                    pos: pos,
                    // TODO: The unopened set passed here has to reflect the valves unopened after the above
                    // FindMaxFlow(...) call
                    unopened: unopened);
            }
            
            max = Math.Max(max, flow);
        }
        
        return max;
    }

    private static HashSet<string> FormUnopenedSet(IEnumerable<string> unopened, string next)
    {
        var set = new HashSet<string>(unopened);
        set.Remove(next);
        return set;
    }

    private int GetTravelTime(string from, string to)
    {
        return _valveData.TravelTimesLookup[from][to];
    }

    private int GetFlowRate(string valve)
    {
        return _valveData.FlowRates[valve];
    }
}