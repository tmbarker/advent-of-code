using Problems.Common;

namespace Problems.Y2017.D01;

/// <summary>
/// Inverse Captcha: https://adventofcode.com/2017/day/1
/// </summary>
public class Solution : SolutionBase
{
    
    public override object Run(int part)
    {
        var stream = GetInputText();
        return part switch
        {
            1 => SumDigits(stream, steps: 1),
            2 => SumDigits(stream, steps: stream.Length / 2),
            _ => ProblemNotSolvedString
        };
    }

    private static int SumDigits(string stream, int steps)
    {
        return stream
            .Where((chr, i) => chr == stream[(i + steps) % stream.Length])
            .Sum(t => t - '0');
    }
}