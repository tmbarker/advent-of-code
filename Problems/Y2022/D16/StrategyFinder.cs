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
        return FindMaxFlow(timeLimit, start, new HashSet<string>());
    }

    private int FindMaxFlow(int time, string currentValve, HashSet<string> opened)
    {
        var max = 0;
        foreach (var nextValve in GetUnopenedValves(opened))
        {
            var travelAndOpenCost = GetTravelTime(currentValve, nextValve) + 1;
            if (travelAndOpenCost >= time)
            {
                continue;
            }

            var timeAtNextValve = time - travelAndOpenCost;
            var openedValves = new HashSet<string>(opened) { nextValve };
            var relieved = GetFlowRate(nextValve) * timeAtNextValve +
                           FindMaxFlow(timeAtNextValve, nextValve, openedValves);

            max = Math.Max(max, relieved);
        }

        return max;
    }

    private int GetTravelTime(string from, string to)
    {
        return _valveMap.TravelTimesLookup[from][to];
    }

    private int GetFlowRate(string valve)
    {
        return _valveMap.FlowRates[valve];
    }

    private IEnumerable<string> GetUnopenedValves(HashSet<string> opened)
    {
        return _valveMap.Valves.Where(v => !opened.Contains(v));
    }
}