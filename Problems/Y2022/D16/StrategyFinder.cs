namespace Problems.Y2022.D16;

public class StrategyFinder
{
    private readonly ValveMap _valveMap;

    public StrategyFinder(ValveMap valveMap)
    {
        _valveMap = valveMap;
    }

    public int Run(string start, int timeLimit)
    {
        var initialState = new StrategyState(start, timeLimit, 0, Enumerable.Empty<string>());
        return Search(0, initialState);
    }

    private int Search(int max, StrategyState state)
    {
        foreach (var valve in GetUnopenedValves(state))
        {
            var timeRemaining = state.TimeRemaining - (GetTimeToValve(state.CurrentValve, valve) + 1);
            if (timeRemaining <= 0)
            {
                continue;
            }

            var nextState = new StrategyState(
                currentValve: valve,
                timeRemaining: timeRemaining,
                pressureRelieved: state.PressureRelieved + _valveMap.FlowRates[valve] * timeRemaining,
                openedValves: new HashSet<string>(state.OpenedValves) { valve });

            max = Math.Max(max, Search(max, nextState));
        }
        
        return Math.Max(max, state.PressureRelieved);
    }

    private int GetTimeToValve(string from, string to)
    {
        return _valveMap.TravelTimesLookup[from][to];
    }

    private IEnumerable<string> GetUnopenedValves(StrategyState state)
    {
        return _valveMap.Valves.Where(v => !state.OpenedValves.Contains(v));
    }
}