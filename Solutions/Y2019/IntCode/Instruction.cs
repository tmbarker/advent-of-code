namespace Solutions.Y2019.IntCode;

internal readonly struct Instruction
{
    private IList<ParameterMode> ParamModes { get; }
    public OpCode OpCode { get; }

    public Instruction(OpCode opCode, IEnumerable<ParameterMode> paramModes)
    {
        OpCode = opCode;
        ParamModes = new List<ParameterMode>(paramModes);
    }
    
    public ParameterMode GetParamMode(int paramIndex)
    {
        return paramIndex < ParamModes.Count 
            ? ParamModes[paramIndex] 
            : ParameterMode.Pos;
    }
}