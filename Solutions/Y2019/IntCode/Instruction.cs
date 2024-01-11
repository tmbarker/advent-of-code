namespace Solutions.Y2019.IntCode;

internal readonly struct Instruction(OpCode opCode, IEnumerable<ParameterMode> paramModes)
{
    private IList<ParameterMode> ParamModes { get; } = new List<ParameterMode>(paramModes);
    public OpCode OpCode { get; } = opCode;

    public ParameterMode GetParamMode(int paramIndex)
    {
        return paramIndex < ParamModes.Count 
            ? ParamModes[paramIndex] 
            : ParameterMode.Pos;
    }
}