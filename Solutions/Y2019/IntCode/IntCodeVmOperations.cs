namespace Solutions.Y2019.IntCode;

public partial class IntCodeVm
{
    private void Add(Instruction instr)
    {
        var pLhs = GetParamAdr(instr, 0);
        var pRhs = GetParamAdr(instr, 1);
        var pDst = GetParamAdr(instr, 2);
        
        WriteMem(
            adr: pDst, 
            val: ReadMem(pLhs) + ReadMem(pRhs));
        
        _pc += 4L;
    }
    
    private void Mul(Instruction instr)
    {
        var pLhs = GetParamAdr(instr, 0);
        var pRhs = GetParamAdr(instr, 1);
        var pDst = GetParamAdr(instr, 2);

        WriteMem(
            adr: pDst,
            val: ReadMem(pLhs) * ReadMem(pRhs));
        
        _pc += 4L;
    }
    
    private void Inp(Instruction instr)
    {
        var inp = InputBuffer.Dequeue();
        var pDst = GetParamAdr(instr, 0);
        
        WriteMem(
            adr: pDst,
            val: inp);
        
        _pc += 2L;
    }
    
    private void Out(Instruction instr)
    {
        var pOut = GetParamAdr(instr, 0);
        var vOut = ReadMem(pOut);

        OutputBuffer.Enqueue(vOut);
        OutputEmitted?.Invoke(this, EventArgs.Empty);
        
        _pc += 2L;
    }
    
    private void Jit(Instruction instr)
    {
        var pVal = GetParamAdr(instr, 0);
        var pJto = GetParamAdr(instr, 1);

        if (ReadMem(pVal) != 0L)
        {
            _pc = ReadMem(pJto);
        }
        else
        {
            _pc += 3L;
        }
    }
    
    private void Jif(Instruction instr)
    {
        var pVal = GetParamAdr(instr, 0);
        var pJto = GetParamAdr(instr, 1);

        if (ReadMem(pVal) == 0L)
        {
            _pc = ReadMem(pJto);
        }
        else
        {
            _pc += 3L;
        }
    }
    
    private void Lst(Instruction instr)
    {
        var pLhs = GetParamAdr(instr, 0);
        var pRhs = GetParamAdr(instr, 1);
        var pDst = GetParamAdr(instr, 2);

        WriteMem(
            adr: pDst, 
            val: ReadMem(pLhs) < ReadMem(pRhs) ? 1L : 0L);
        
        _pc += 4L;
    }
    
    private void Eql(Instruction instr)
    {
        var pLhs = GetParamAdr(instr, 0);
        var pRhs = GetParamAdr(instr, 1);
        var pDst = GetParamAdr(instr, 2);

        WriteMem(
            adr: pDst, 
            val: ReadMem(pLhs) == ReadMem(pRhs) ? 1L : 0L);
        
        _pc += 4L;
    }
    
    private void Rbo(Instruction instr)
    {
        var pOff = GetParamAdr(instr, 0);

        _rb += ReadMem(pOff);
        _pc += 2L;
    }
}