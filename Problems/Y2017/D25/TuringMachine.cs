namespace Problems.Y2017.D25;

public sealed class TuringMachine
{
    private readonly Dictionary<char, State> _ruleTable;
    private readonly Dictionary<int, bool> _tape = new();
    private int _cursor;
    
    public TuringMachine(Dictionary<char, State> ruleTable)
    {
        _ruleTable = ruleTable;
    }

    public int Run(char state, int steps)
    {
        _tape.Clear();
        _cursor = 0;
        
        for (var i = 0; i < steps; i++)
        {
            var rule = _ruleTable[state];
            var transition = _tape.TryGetValue(_cursor, out var value) && value
                ? rule.True
                : rule.False;

            _tape[_cursor] = transition.Write;
            _cursor += transition.Move;
            
            state = transition.Next;
        }

        return _tape.Values.Count(b => b);
    }
    
    public readonly record struct Transition(bool Write, int Move, char Next);
    public readonly record struct State(Transition False, Transition True);
}