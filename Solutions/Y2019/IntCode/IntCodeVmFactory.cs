namespace Solutions.Y2019.IntCode;

public partial class IntCodeVm
{
    public static IntCodeVm Create(IList<long> program)
    {
        return new IntCodeVm(program, Enumerable.Empty<long>());
    }
    
    public static IntCodeVm Create(IList<long> program, IEnumerable<long> inputs)
    {
        return new IntCodeVm(program, inputs);
    }
    
    public static IntCodeVm Create(IList<long> program, long input)
    {
        return new IntCodeVm(program, Enumerable.Repeat(input, 1));
    }
}