using Problems.Common;

namespace Problems.Y2019.IntCode;

public abstract class IntCodeSolution : SolutionBase
{
    protected IList<long> LoadIntCodeProgram()
    {
        return new List<long>(GetInputText().Split(',').Select(long.Parse));
    }
}