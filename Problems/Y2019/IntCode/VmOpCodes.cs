namespace Problems.Y2019.IntCode;

public partial class Vm
{
    private void Add(Instruction instr)
    {
        var arg1 = _program[_pc + 1];
        var arg2 = _program[_pc + 2];
        var dst  = _program[_pc + 3];
        
        var lhs = ResolveArg(instr.GetParamMode(0), arg1);
        var rhs = ResolveArg(instr.GetParamMode(1), arg2);

        _program[dst] = lhs + rhs;
        _pc += 4;
    }
    
    private void Mul(Instruction instr)
    {
        var arg1 = _program[_pc + 1];
        var arg2 = _program[_pc + 2];
        var dst  = _program[_pc + 3];
        
        var lhs = ResolveArg(instr.GetParamMode(0), arg1);
        var rhs = ResolveArg(instr.GetParamMode(1), arg2);

        _program[dst] = lhs * rhs;
        _pc += 4;
    }
    
    private void Inp(Instruction instr)
    {
        if (InputSource == null)
        {
            throw new MissingInputSourceException();
        }

        var inp = InputSource.Read();
        var dst = _program[_pc + 1];

        _program[dst] = inp;
        _pc += 2;
    }
    
    private void Out(Instruction instr)
    {
        if (OutputSink == null)
        {
            throw new MissingOutputSinkException();
        }
        
        var arg = _program[_pc + 1];
        var val = ResolveArg(instr.GetParamMode(0), arg);

        OutputSink.Write(val);
        _pc += 2;
    }
    
    private void Jit(Instruction instr)
    {
        var arg1 = _program[_pc + 1];
        var arg2 = _program[_pc + 2];
        
        var val = ResolveArg(instr.GetParamMode(0), arg1);
        var jto = ResolveArg(instr.GetParamMode(1), arg2);

        if (val != 0)
        {
            _pc = jto;
        }
        else
        {
            _pc += 3;
        }
    }
    
    private void Jif(Instruction instr)
    {
        var arg1 = _program[_pc + 1];
        var arg2 = _program[_pc + 2];
        
        var val = ResolveArg(instr.GetParamMode(0), arg1);
        var jto = ResolveArg(instr.GetParamMode(1), arg2);

        if (val == 0)
        {
            _pc = jto;
        }
        else
        {
            _pc += 3;
        }
    }
    
    private void Lst(Instruction instr)
    {
        var arg1 = _program[_pc + 1];
        var arg2 = _program[_pc + 2];
        var dest = _program[_pc + 3];
        
        var val1 = ResolveArg(instr.GetParamMode(0), arg1);
        var val2 = ResolveArg(instr.GetParamMode(1), arg2);

        _program[dest] = val1 < val2 ? 1 : 0;
        _pc += 4;
    }
    
    private void Eql(Instruction instr)
    {
        var arg1 = _program[_pc + 1];
        var arg2 = _program[_pc + 2];
        var dest = _program[_pc + 3];
        
        var val1 = ResolveArg(instr.GetParamMode(0), arg1);
        var val2 = ResolveArg(instr.GetParamMode(1), arg2);

        _program[dest] = val1 == val2 ? 1 : 0;
        _pc += 4;
    }
    
    private Dictionary<OpCode, Operation> BuildOpCodeTable()
    {
        return new Dictionary<OpCode, Operation>
        {
            { OpCode.Add, Add },
            { OpCode.Mul, Mul },
            { OpCode.Inp, Inp },
            { OpCode.Out, Out },
            { OpCode.Jit, Jit },
            { OpCode.Jif, Jif },
            { OpCode.Lst, Lst },
            { OpCode.Eql, Eql },
        };
    }
}