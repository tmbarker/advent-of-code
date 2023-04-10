namespace Problems.Y2017.D18;

public class Cpu
{
    private readonly Dictionary<string, long> _registers;
    private readonly IList<string[]> _instructions;
    private long _ip;

    public event Action<long>? DataTransmitted; 
    public event Action<long>? DataReceived;

    public Queue<long>? InputBuffer { get; set; }
    public Queue<long>? OutputBuffer { get; set; }

    public long this[string reg]
    {
        get => _registers.TryGetValue(reg, out var value) ? value : 0L;
        set => _registers[reg] = value;
    }

    public Cpu(IEnumerable<string> program)
    {
        _registers = new Dictionary<string, long>();
        _instructions = program
            .Select(line => line.Split(' '))
            .ToList();
    }
    
    public ExitCode Run(CancellationToken token)
    {
        while (!token.IsCancellationRequested && _ip >= 0 && _ip < _instructions.Count)
        {
            var tokens = _instructions[(int)_ip];
            var args = tokens[1..];
            var op = tokens[0];

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