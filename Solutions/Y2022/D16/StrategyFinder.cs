using System.Collections.Immutable;

namespace Solutions.Y2022.D16;

public sealed class StrategyFinder(ValveData valveData)
{
    public event Action<Strategy>? StrategyFound;

    public void Run(string start, int timeLimit)
    {
        Search(
            t: timeLimit,
            f: 0,
            pos: start,
            unopened: [..valveData.Valves]);
    }

    private void Search(int t, int f, string pos, ImmutableHashSet<string> unopened)
    {
        RaiseStrategyFound(f, unopened);
        
        foreach (var valve in unopened)
        {
            var travelAndOpenTime = GetTravelTime(pos, valve) + 1;
            var timeAtNextValve = t - travelAndOpenTime;
            
            if (travelAndOpenTime >= t)
            {
                continue;
            }

            Search(
                t: timeAtNextValve,
                f: f + GetFlowRate(valve) * timeAtNextValve,
                pos: valve,
                unopened: unopened.Remove(valve));
        }
    }

    private int GetTravelTime(string from, string to)
    {
        return valveData.TravelTimesLookup[(from, to)];
    }

    private int GetFlowRate(string valve)
    {
        return valveData.FlowRates[valve];
    }

    private void RaiseStrategyFound(int flow, IEnumerable<string> unopened)
    {
        StrategyFound?.Invoke(new Strategy(
            flow: flow,
            opened: valveData.Valves.Except(unopened)));
    }
}