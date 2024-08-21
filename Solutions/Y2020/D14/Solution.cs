namespace Solutions.Y2020.D14;

[PuzzleInfo("Docking Data", Topics.Assembly|Topics.BitwiseOperations|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var program = GetInputLines();
        return part switch
        { 
            1 => Machine.RunV1(program),
            2 => Machine.RunV2(program),
            _ => PuzzleNotSolvedString
        };
    }
}