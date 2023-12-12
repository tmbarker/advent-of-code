namespace Solutions.Y2017.D25;

public sealed class TuringMachine(IReadOnlyDictionary<char, TuringMachine.State> ruleTable)
{
    public readonly record struct Transition(bool Write, int Move, char Next);
    public readonly record struct State(Transition False, Transition True);
    
    private readonly Dictionary<int, bool> _tape = new();
    private int _cursor;

    public int Run(char state, int steps)
    {
        _tape.Clear();
        _cursor = 0;
        
        for (var i = 0; i < steps; i++)
        {
            var rule = ruleTable[state];
            var transition = _tape.TryGetValue(_cursor, out var value) && value
                ? rule.True
                : rule.False;

            _tape[_cursor] = transition.Write;
            _cursor += transition.Move;
            
            state = transition.Next;
        }

        return _tape.Values.Count(b => b);
    }
}