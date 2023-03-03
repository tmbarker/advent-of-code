using Problems.Y2018.Common;

namespace Problems.Y2018.D01;

/// <summary>
/// Chronal Calibration: https://adventofcode.com/2018/day/1
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 1;
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var numbers = ParseNumbers(input);
            
        return part switch
        {
            1 => numbers.Sum(),
            2 => GetFirstRepeatedFrequency(numbers),
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

    private static IList<int> ParseNumbers(IList<string> input)
    {
        return new List<int>(input.Select(int.Parse));
    }
}