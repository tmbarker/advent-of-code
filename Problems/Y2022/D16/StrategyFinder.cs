using Utilities.Extensions;

namespace Problems.Y2022.D16;

public class StrategyFinder
{
    private readonly ValveData _valveData;

    public event Action<Strategy>? StrategyFound; 

    public StrategyFinder(ValveData valveData)
    {
        _valveData = valveData;
    }

    public void Run(string start, int timeLimit)
    {
        Search(
            t: timeLimit, 
            f: 0, 
            pos: start, 
            unopened: new HashSet<string>(_valveData.Valves));
    }

    private void Search(int t, int f, string pos, ICollection<string> unopened)
    {
        RaiseStrategyFound(f, unopened);
        
        foreach (var valve in unopened.Freeze())
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
                unopened: unopened.Except(valve));
        }
    }

    private int GetTravelTime(string from, string to)
    {
        return _valveData.TravelTimesLookup[(from, to)];
    }

    private int GetFlowRate(string valve)
    {
        return _valveData.FlowRates[valve];
    }

    private void RaiseStrategyFound(int flow, IEnumerable<string> unopened)
    {
        StrategyFound?.Invoke(new Strategy(
            flow: flow,
            opened: _valveData.Valves.Except(unopened)));
    }
}