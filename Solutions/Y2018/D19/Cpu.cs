namespace Solutions.Y2018.D19;

public sealed class Cpu
{
    private const int LoopAdr = 1;
    private const int ExitAdr = 16;
    
    private readonly int _ipAdr;
    private readonly Dictionary<int, int> _registers = new()
    {
        { 0, 0 },
        { 1, 0 },
        { 2, 0 },
        { 3, 0 },
        { 4, 0 },
        { 5, 0 }
    };

    public int this[int addr]
    {
        get => _registers[addr];
        set => _registers[addr] = value;
    }
    
    private int Ip
    {
        get => this[_ipAdr];
        set => this[_ipAdr] = value;
    }

    public Cpu(int ipAdr)
    {
        _ipAdr = ipAdr;
    }

    public int Run(IList<Instruction> program, bool enableOptimizations)
    {
        while (Ip < program.Count)
        {
            //  This optimization comes from disassembling the input instructions.
            //  See the adjacent commented asm.txt file for reference.
            //
            if (enableOptimizations && Ip == LoopAdr)
            {
                this[0] = SumOfDivisors(this[5]);
                Ip = ExitAdr;
                continue;
            }
            
            var instruction = program[Ip];
            Execute(
                opcode: instruction.Opcode,
                a: instruction.A,
                b: instruction.B,
                c: instruction.C);

            Ip++;
        }

        return this[0];
    }

    private static int SumOfDivisors(int n)
    {
        var sum = 0;
        for (var d = 1; d <= n + 1; d++)
        {
            if (n % d == 0)
            {
                sum += d;
            }
        }

        return sum;
    }

    private void Execute(string opcode, int a, int b, int c)
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

    public readonly record struct Instruction(string Opcode, int A, int B, int C);
}