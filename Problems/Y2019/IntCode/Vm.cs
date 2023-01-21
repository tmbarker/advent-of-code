namespace Problems.Y2019.IntCode;

public partial class Vm
{
    private const int ResultAdr = 0;
    private const int Halt = 99;
    
    private readonly Dictionary<OpCode, Operation> _opCodeTable;

    private int _pc;
    private IList<int> _program = new List<int>();

    public IInputSource? InputSource { get; init; }
    public IOutputSink? OutputSink { get; init; }
    
    public Vm()
    {
        _opCodeTable = BuildOpCodeTable();
    }
    
    public int Run(IList<int> program)
    {
        _pc = 0;
        _program = program;
        
        while (_program[_pc] != Halt)
        {
            ExecuteInstr(ParseInstr(_program[_pc]));
        }

        return program[ResultAdr];
    }

    private void ExecuteInstr(Instruction instr)
    {
        _opCodeTable[instr.OpCode].Invoke(instr);
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
    
    private int ResolveArg(ParameterMode mode, int arg)
    {
        return mode switch
        {
            ParameterMode.Pos => _program[arg],
            ParameterMode.Imm => arg,
            _ => throw new ArgumentOutOfRangeException(nameof(mode), mode, null)
        };
    }
}