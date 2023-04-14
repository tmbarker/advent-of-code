using Problems.Common;

namespace Problems.Y2018.D01;

/// <summary>
/// Chronal Calibration: https://adventofcode.com/2018/day/1
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var numbers = ParseInputLines(int.Parse);
        var enumerated = numbers.ToList();
        
        return part switch
        {
            1 => enumerated.Sum(),
            2 => GetFirstRepeatedFrequency(enumerated),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetFirstRepeatedFrequency(IList<int> numbers)
    {
        var i = 0;
        var freq = 0;
        var seen = new HashSet<int>();

        while (seen.Add(freq))
        {
            freq += numbers[i];
            i = (i + 1) % numbers.Count;
        }

        return freq;
    }
}