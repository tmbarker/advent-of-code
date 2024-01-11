namespace Solutions.Y2018.D16;

public sealed class Cpu
{
    private readonly Dictionary<int, int> _registers = new();

    public int this[int addr]
    {
        get => _registers[addr];
        private set => _registers[addr] = value;
    }
    
    public IEnumerable<int> Registers => _registers.Values;
    
    public Cpu()
    {
        ResetRegisters();
    }
    
    public void SetRegisters(IList<int> values)
    {
        _registers[0] = values[0];
        _registers[1] = values[1];
        _registers[2] = values[2];
        _registers[3] = values[3];
    }

    public void ResetRegisters()
    {
        SetRegisters([0, 0, 0, 0]);
    }

    public void Execute(Opcode opcode, int a, int b, int c)
    {
        this[c] = opcode switch
        {
            Opcode.Addr => this[a] + this[b],
            Opcode.Addi => this[a] + b,
            Opcode.Mulr => this[a] * this[b],
            Opcode.Muli => this[a] * b,
            Opcode.Banr => this[a] & this[b],
            Opcode.Bani => this[a] & b,
            Opcode.Borr => this[a] | this[b],
            Opcode.Bori => this[a] | b,
            Opcode.Setr => this[a],
            Opcode.Seti => a,
            Opcode.Gtir => a > this[b] ? 1 : 0,
            Opcode.Gtri => this[a] > b ? 1 : 0,
            Opcode.Gtrr => this[a] > this[b] ? 1 : 0,
            Opcode.Eqir => a == this[b] ? 1 : 0,
            Opcode.Eqri => this[a] == b ? 1 : 0,
            Opcode.Eqrr => this[a] == this[b] ? 1 : 0,
            _ => throw new ArgumentOutOfRangeException(nameof(opcode))
        };
    }

    public enum Opcode
    {
        // ReSharper disable IdentifierTypo
        Addr = 0,
        Addi,
        Mulr,
        Muli,
        Banr,
        Bani,
        Borr,
        Bori,
        Setr,
        Seti,
        Gtir,
        Gtri,
        Gtrr,
        Eqir,
        Eqri,
        Eqrr
        // ReSharper restore IdentifierTypo
    }
}