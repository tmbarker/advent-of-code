namespace Problems.Y2022.D16;

public class StrategyFinder
{
    private readonly string _start;
    private readonly int _timeLimit;
    private readonly ValveMap _valveMap;

    public event Action<Strategy>? StrategyFound;
    
    public StrategyFinder(string start, int timeLimit, ValveMap valveMap)
    {
        _start = start;
        _timeLimit = timeLimit;
        _valveMap = valveMap;
    }

    public void Run()
    {
        var state = new StrategyState();
        state.Path.Push(_start);
        
        FindStrategies(state);
    }

    private void FindStrategies(StrategyState strategyState)
    {
        if (IsStrategyFinished(strategyState))
        {
            RaiseStrategyFound(new Strategy(strategyState.OpenedValves));
            return;
        }

        var currentValve = strategyState.CurrentValve;
        foreach (var nextValve in GetUnopenedNonZeroValves(strategyState))
        {
            var timeToValve = GetTimeToValve(currentValve, nextValve);
            strategyState.CurrentTime += timeToValve;
            strategyState.OpenedValves[nextValve] = strategyState.CurrentTime++;

            strategyState.Path.Push(nextValve);
            FindStrategies(strategyState);
            
            strategyState.Path.Pop();
            strategyState.OpenedValves.Remove(nextValve);
            strategyState.CurrentTime -= timeToValve + 1;
        }
    }

    private bool IsStrategyFinished(StrategyState state)
    {
        return state.CurrentTime >= _timeLimit || state.OpenedValves.Count == _valveMap.NonZeroFlowRates.Count;
    }

    private int GetTimeToValve(string from, string to)
    {
        return _valveMap.ValveTravelTimesLookup[from][to];
    } 
    
    private IEnumerable<string> GetUnopenedNonZeroValves(StrategyState state)
    {
        return _valveMap.NonZeroFlowRates.Keys.Where(v => !state.IsValveOpened(v));
    }

    private void RaiseStrategyFound(Strategy strategy)
    {
        StrategyFound?.Invoke(strategy);
    }
}