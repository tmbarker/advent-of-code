using Utilities.Collections;

namespace Problems.Y2017.Common;

public sealed class Vm
{
    private readonly DefaultDict<string, long> _registers;
    private readonly Dictionary<string, Action> _listeners;
    private readonly IList<string[]> _instructions;
    private long _ip;

    public event Action<long>? DataTransmitted; 
    public event Action<long>? DataReceived;

    public Queue<long>? InputBuffer { get; init; }
    public Queue<long>? OutputBuffer { get; init; }

    public long this[string reg]
    {
        get => _registers[reg];
        set => _registers[reg] = value;
    }

    public Vm(IEnumerable<string> program)
    {
        _registers = new DefaultDict<string, long>(defaultValue: 0L);
        _listeners = new Dictionary<string, Action>();
        _instructions = program
            .Select(line => line.Split(' '))
            .ToList();
    }

    public void RegisterListener(string op, Action listener)
    {
        _listeners[op] = listener;
    }
    
    public ExitCode Run(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _ip >= 0 && _ip < _instructions.Count)
        {
            var tokens = _instructions[(int)_ip];
            var args = tokens[1..];
            var op = tokens[0];

            if (_listeners.TryGetValue(op, out var listener))
            {
                listener.Invoke();
            }
            
            switch (op)
            {
                case "snd":
                    var transmit = GetValue(args[0]);
                    DataTransmitted?.Invoke(transmit);
                    OutputBuffer?.Enqueue(transmit);
                    break;
                case "set":
                    this[args[0]] = GetValue(args[1]);
                    break;
                case "add":
                    this[args[0]] += GetValue(args[1]);
                    break;
                case "sub":
                    this[args[0]] -= GetValue(args[1]);
                    break;
                case "mul":
                    this[args[0]] *= GetValue(args[1]);
                    break;
                case "mod":
                    this[args[0]] %= GetValue(args[1]);
                    break;
                case "rcv":
                    if (InputBuffer == null || !InputBuffer.Any())
                    {
                        return ExitCode.AwaitingInput;
                    }
                    var received = InputBuffer.Dequeue();
                    this[args[0]] = received;
                    DataReceived?.Invoke(received);
                    break;
                case "jgz":
                    if (GetValue(args[0]) > 0)
                    {
                        _ip += GetValue(args[1]);
                        continue;
                    }
                    break;
                case "jnz":
                    if (GetValue(args[0]) != 0)
                    {
                        _ip += GetValue(args[1]);
                        continue;
                    }
                    break;
            }
            
            _ip++;
        }

        return ExitCode.Halted;
    }

    private long GetValue(string s)
    {
        return long.TryParse(s, out var value)
            ? value
            : this[s];
    }

    public enum ExitCode
    {
        Halted = 0,
        AwaitingInput
    }
}