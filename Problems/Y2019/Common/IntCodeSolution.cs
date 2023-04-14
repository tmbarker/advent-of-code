using Problems.Common;

namespace Problems.Y2019.Common;

public abstract class IntCodeSolution : SolutionBase
{
    protected IList<long> LoadIntCodeProgram()
    {
        return new List<long>(GetInputText().Split(',').Select(long.Parse));
    }
}