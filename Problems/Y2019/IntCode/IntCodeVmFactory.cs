namespace Problems.Y2019.IntCode;

public partial class IntCodeVm
{
    public static IntCodeVm Create(IList<int> program, IEnumerable<int>? inputs = null)
    {
        return new IntCodeVm(program, inputs ?? Enumerable.Empty<int>());
    }

    public static IntCodeVm Create(IList<int> program, int input)
    {
        return new IntCodeVm(program, Enumerable.Repeat(input, 1));
    }
}