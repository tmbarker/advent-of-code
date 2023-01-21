using Problems.Common;

namespace Problems.Y2019.Common;

public abstract class SolutionBase2019 : SolutionBase
{
    public override int Year => 2019;
    
    protected IList<int> LoadIntCodeProgram()
    {
        return new List<int>(GetInputText().Split(',').Select(int.Parse));
    }
}