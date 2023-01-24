using Problems.Common;

namespace Problems.Y2019.Common;

public abstract class SolutionBase2019 : SolutionBase
{
    public override int Year => 2019;
    
    protected IList<long> LoadIntCodeProgram()
    {
        return new List<long>(GetInputText().Split(',').Select(long.Parse));
    }
}