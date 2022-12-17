namespace Problems.Y2022.D16;

public class StrategyFinder
{
    private readonly ValveMap _valveMap;

    public event Action<StrategyState>? StrategyFound;
    
    public StrategyFinder(ValveMap valveMap)
    {
        _valveMap = valveMap;
    }

    public void Run(string start, int timeLimit)
    {
        var initialState = new StrategyState(start, timeLimit, 0, Enumerable.Empty<string>());
        var stack = new Stack<StrategyState>(Enumerable.Repeat(initialState, 1));

        FindStrategies(stack);
    }

    private void FindStrategies(Stack<StrategyState> stateStack)
    {
        var currentState = stateStack.Peek();
        if (IsStrategyFinished(currentState))
        {
            RaiseStrategyFound(stateStack.Pop());
            return;
        }
            
        var currentValve = currentState.CurrentValve;
        foreach (var nextValve in GetUnopenedValves(currentState))
        {
            var timeRemaining = currentState.TimeRemaining - (GetTimeToValve(currentValve, nextValve) + 1);
            if (timeRemaining <= 0)
            {
                continue;
            }
            
            stateStack.Push(new StrategyState(
                currentValve: nextValve, 
                timeRemaining: timeRemaining, 
                pressureRelieved: currentState.PressureRelieved + _valveMap.FlowRates[nextValve] * timeRemaining, 
                openedValves:  new HashSet<string>(currentState.OpenedValves) { nextValve }));
            
            FindStrategies(stateStack);
        }
        
        RaiseStrategyFound(stateStack.Pop());
    }

    private bool IsStrategyFinished(StrategyState state)
    {
        return state.TimeRemaining <= 0 || state.OpenedValves.Count == _valveMap.Valves.Count;
    }

    private int GetTimeToValve(string from, string to)
    {
        return _valveMap.TravelTimesLookup[from][to];
    }

    private IEnumerable<string> GetUnopenedValves(StrategyState state)
    {
        return _valveMap.Valves.Where(v => !state.OpenedValves.Contains(v));
    }

    private void RaiseStrategyFound(StrategyState state)
    {
        StrategyFound?.Invoke(state);
    }
}