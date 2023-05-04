namespace Problems.Y2019.IntCode;

public partial class IntCodeVm
{
    private readonly Dictionary<long, long> _memory;
    private long _pc;
    private long _rb;

    public event EventHandler? AwaitingInput;
    public event EventHandler? OutputEmitted;

    public IReadOnlyDictionary<long, long> Memory => _memory;
    public Queue<long> InputBuffer { get; }
    public Queue<long> OutputBuffer { get; }

    private IntCodeVm(IList<long> program, IEnumerable<long> inputs)
    {
        _memory = InitMemoryFromProgram(program);
        _pc = 0;
        _rb = 0;
        
        InputBuffer = new Queue<long>(inputs);
        OutputBuffer = new Queue<long>();
    }

    public ExitCode Run()
    {
        while (_pc >= 0)
        {
            var instr = ParseInstr(ReadMem(_pc));
            switch (instr.OpCode)
            {
                case OpCode.Add:
                    Add(instr);
                    break;
                case OpCode.Mul:
                    Mul(instr);
                    break;
                case OpCode.Inp:
                    if (!InputBuffer.Any())
                    {
                        AwaitingInput?.Invoke(this, EventArgs.Empty);
                        return ExitCode.AwaitingInput;
                    }
                    Inp(instr);
                    break;
                case OpCode.Out:
                    Out(instr);
                    break;
                case OpCode.Jit:
                    Jit(instr);
                    break;
                case OpCode.Jif:
                    Jif(instr);
                    break;
                case OpCode.Lst:
                    Lst(instr);
                    break;
                case OpCode.Eql:
                    Eql(instr);
                    break;
                case OpCode.Rbo:
                    Rbo(instr);
                    break;
                case OpCode.Hlt:
                    return ExitCode.Halted;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        throw new AccessViolationException();
    }

    private long GetParamAdr(Instruction instr, int paramIdx)
    {
        var adr = _pc + paramIdx + 1;
        var mode = instr.GetParamMode(paramIdx);
        
        return mode switch
        {
            ParameterMode.Imm => adr,
            ParameterMode.Pos => ReadMem(adr),
            ParameterMode.Rel => ReadMem(adr) + _rb,
            _ => throw new ArgumentOutOfRangeException(nameof(instr.OpCode), instr.OpCode, "Invalid opcode")
        };
    }

    private long ReadMem(long adr)
    {
        return _memory.TryGetValue(adr, out var value) ? value : 0L;
    }

    private void WriteMem(long adr, long val)
    {
        _memory[adr] = val;
    }
    
    private static Instruction ParseInstr(long value)
    {
        var opCode = (OpCode)(value % 100L);
        var paramModes = new List<ParameterMode>();

        value /= 100L;
        while (value > 0L)
        {
            paramModes.Add((ParameterMode)(value % 10L));
            value /= 10L;
        }
        
        return new Instruction(
            opCode: opCode,
            paramModes: paramModes);
    }

    private static Dictionary<long, long> InitMemoryFromProgram(IList<long> program)
    {
        var memory = new Dictionary<long, long>();
        for (var i = 0; i < program.Count; i++)
        {
            memory[i] = program[i];
        }
        return memory;
    }

    public enum ExitCode
    {
        Halted,
        AwaitingInput
    }
}