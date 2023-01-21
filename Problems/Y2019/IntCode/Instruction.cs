namespace Problems.Y2019.IntCode;

internal readonly struct Instruction
{
    public Instruction(OpCode opCode, IEnumerable<ParameterMode> paramModes)
    {
        OpCode = opCode;
        ParamModes = new List<ParameterMode>(paramModes);
    }
    
    private IList<ParameterMode> ParamModes { get; }
    public OpCode OpCode { get; }

    public ParameterMode GetParamMode(int paramIndex)
    {
        return paramIndex < ParamModes.Count ? ParamModes[paramIndex] : ParameterMode.Pos;
    }
}