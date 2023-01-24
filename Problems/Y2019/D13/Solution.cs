using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D13;

/// <summary>
/// Care Package: https://adventofcode.com/2019/day/13
/// </summary>
public class Solution : SolutionBase2019
{
    private static readonly Dictionary<long, GameObject> GobCodes = new()
    {
        { 0L, GameObject.Empty },
        { 1L, GameObject.Wall },
        { 2L, GameObject.Block },
        { 3L, GameObject.Paddle },
        { 4L, GameObject.Ball }
    };

    public override int Day => 13;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => CountGameObjects(GameObject.Block),
            _ => ProblemNotSolvedString
        };
    }

    private int CountGameObjects(GameObject type)
    {
        var vm = IntCodeVm.Create(LoadIntCodeProgram());
        var gobCounts = Enum
            .GetValues<GameObject>()
            .ToDictionary(g => g, _ => 0);
        
        vm.Run();
        while (vm.OutputBuffer.Any())
        {
            vm.OutputBuffer.Dequeue();
            vm.OutputBuffer.Dequeue();
            gobCounts[GobCodes[vm.OutputBuffer.Dequeue()]]++;
        }

        return gobCounts[type];
    }
}