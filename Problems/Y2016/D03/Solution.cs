using Utilities.Extensions;

namespace Problems.Y2016.D03;

[PuzzleInfo("Square With Three Sides", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountValidHorizontal(),
            2 => CountValidVertical(),
            _ => ProblemNotSolvedString
        };
    }

    private int CountValidHorizontal()
    {
        var input = GetInputLines();
        return input
            .Select(StringExtensions.ParseInts)
            .Count(IsValidTriangle);
    }
    
    private int CountValidVertical()
    {
        var count = 0;
        var numbers = GetInputLines()
            .Select(line => line.ParseInts())
            .ToList();
        
        var lengths = new int[3];
        for (var i = 0; i < numbers.Count; i += 3)
        for (var c = 0; c < 3; c++)
        {
            lengths[0] = numbers[i + 0][c];
            lengths[1] = numbers[i + 1][c];
            lengths[2] = numbers[i + 2][c];

            if (IsValidTriangle(lengths))
            {
                count++;
            }
        }

        return count;
    }

    private static bool IsValidTriangle(IList<int> lengths)
    {
        return
            lengths[0] + lengths[1] > lengths[2] &&
            lengths[0] + lengths[2] > lengths[1] &&
            lengths[1] + lengths[2] > lengths[0];
    }
}