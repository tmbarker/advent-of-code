using Utilities.Extensions;

namespace Solutions.Y2015.D02;

[PuzzleInfo("I Was Told There Would Be No Math", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var boxes = input.Select(ParseBox);
        
        return part switch
        {
            1 => boxes.Sum(box => box.PaperReq),
            2 => boxes.Sum(box => box.RibbonReq),
            _ => ProblemNotSolvedString
        };
    }

    private static Box ParseBox(string line)
    {
        var dims = line.ParseLongs();
        return new Box(
            l: dims[0],
            w: dims[1],
            h: dims[2]);
    }
}