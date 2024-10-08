using Solutions.Y2016.Common;

namespace Solutions.Y2016.D12;

[PuzzleInfo("Leonardo's Monorail", Topics.Assembly|Topics.Simulation, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var tokens = input.Select(line => line.Split(' ')).ToList();

        return part switch
        {
            1 => RunProgram(tokens, c: 0),
            2 => RunProgram(tokens, c: 1),
            _ => PuzzleNotSolvedString
        };
    }

    private static long RunProgram(IList<string[]> program, long c)
    {
        var vm = new Vm { ["c"] = c };
        vm.Run(program);
        return vm["a"];
    }
}