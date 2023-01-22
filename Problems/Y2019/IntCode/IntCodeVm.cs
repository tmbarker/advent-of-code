using Problems.Common;

namespace Problems.Y2019.IntCode;

public partial class IntCodeVm
{
    private readonly IList<int> _program;
    private int _pc;

    public Queue<int> InputBuffer { get; }
    public Queue<int> OutputBuffer { get; }

    private IntCodeVm(IList<int> program, IEnumerable<int> inputs)
    {
        _program = program;
        _pc = 0;
        
        InputBuffer = new Queue<int>(inputs);
        OutputBuffer = new Queue<int>();
    }

    public ExitCode Run()
    {
        while (PcValid())
        {
            var instr = ParseInstr(_program[_pc]);
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
                case OpCode.Hlt:
                    return ExitCode.Halted;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        throw new NoSolutionException();
    }

    private bool PcValid()
    {
        return _pc >= 0 && _pc < _program.Count;
    }

    private int ResolveArg(ParameterMode mode, int arg)
    {
        return mode switch
        {
            ParameterMode.Pos => _program[arg],
            ParameterMode.Imm => arg,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
    
    private static Instruction ParseInstr(int value)
    {
        var chars = new List<char>(value.ToString());
        var length = chars.Count;

        var opCodeValue = 0;
        var paramModes = new List<ParameterMode>();

        for (var i = 0; i < length; i++)
        {
            var digit = chars[length - i - 1] - '0';
            switch (i)
            {
                case 0 or 1:
                    opCodeValue += (int)Math.Pow(10, i) * digit;
                    continue;
                case > 1:
                    paramModes.Add((ParameterMode)digit);
                    continue;
            }
        }

        return new Instruction(
            opCode: (OpCode)opCodeValue,
            paramModes: paramModes);
    }

    public enum ExitCode
    {
        Halted,
        AwaitingInput,
    }
}