namespace Solutions.Y2018.D21;

public sealed class Cpu
{
    private readonly long _ipAdr;
    private readonly Dictionary<long, Action> _ipListeners = new();
    private readonly Dictionary<long, long> _registers = new()
    {
        { 0L, 0L },
        { 1L, 0L },
        { 2L, 0L },
        { 3L, 0L },
        { 4L, 0L },
        { 5L, 0L }
    };

    public long this[long addr]
    {
        get => _registers[addr];
        private set => _registers[addr] = value;
    }
    
    private long Ip
    {
        get => this[_ipAdr];
        set => this[_ipAdr] = value;
    }

    public Cpu(long ipAdr)
    {
        _ipAdr = ipAdr;
    }

    public void RegisterIpListener(long ipValue, Action listener)
    {
        _ipListeners[ipValue] = listener;
    }

    public void Run(IList<Instruction> program, CancellationToken token)
    {
        while (Ip < program.Count && !token.IsCancellationRequested)
        {
            CheckIpListeners();
            
            var instruction = program[(int)Ip];
            
            Execute(
                opcode: instruction.Opcode,
                a: instruction.A,
                b: instruction.B,
                c: instruction.C);

            Ip++;
        }
    }

    private void CheckIpListeners()
    {
        if (_ipListeners.TryGetValue(Ip, out var listener))
        {
            listener.Invoke();
        }
    }
    
    private void Execute(string opcode, long a, long b, long c)
    {
        this[c] = opcode switch
        {
            // ReSharper disable StringLiteralTypo
            "addr" => this[a] + this[b],
            "addi" => this[a] + b,
            "mulr" => this[a] * this[b],
            "muli" => this[a] * b,
            "banr" => this[a] & this[b],
            "bani" => this[a] & b,
            "borr" => this[a] | this[b],
            "bori" => this[a] | b,
            "setr" => this[a],
            "seti" => a,
            "gtir" => a > this[b] ? 1 : 0,
            "gtri" => this[a] > b ? 1 : 0,
            "gtrr" => this[a] > this[b] ? 1 : 0,
            "eqir" => a == this[b] ? 1 : 0,
            "eqri" => this[a] == b ? 1 : 0,
            "eqrr" => this[a] == this[b] ? 1 : 0,
            // ReSharper restore StringLiteralTypo
            _ => throw new ArgumentOutOfRangeException(nameof(opcode))
        };
    }

    public readonly record struct Instruction(string Opcode, long A, long B, long C);
}