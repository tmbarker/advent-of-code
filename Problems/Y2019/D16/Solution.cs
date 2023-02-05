using Problems.Y2019.Common;

namespace Problems.Y2019.D16;

/// <summary>
/// Flawed Frequency Transmission: https://adventofcode.com/2019/day/16
/// </summary>
public class Solution : SolutionBase2019
{
    private const int OutputStringLength = 8;
    private const int Phases = 100;

    private static readonly int[] BasePattern = { 0, 1, 0, -1 };

    public override int Day => 16;
    
    public override object Run(int part)
    {
        var data = ParseNumbers(GetInputText());
        return part switch
        {
            0 => FormOutputString(Fft(data, Phases).Take(OutputStringLength)),
            _ => ProblemNotSolvedString,
        };
    }

    private static int[] Fft(int[] data, int phases)
    {
        for (var i = 0; i < phases; i++)
        {
            data = Phase(data);
        }

        return data;
    }
    
    private static int[] Phase(int[] data)
    {
        var length = data.Length;
        var dest = new int[length];
        
        for (var i = 0; i < length; i++)
        {
            var pattern = GeneratePattern(i);
            var element = 0;

            for (var j = 0; j < length; j++)
            {
                element += data[j] * pattern[(j + 1) % pattern.Length];
            }

            dest[i] = Math.Abs(element) % 10;
        }

        return dest;
    }

    private static int[] GeneratePattern(int index)
    {
        var repeats = index + 1;
        var pattern = new int[BasePattern.Length * repeats];

        for (var i = 0; i < BasePattern.Length; i++)
        for (var j = 0; j < repeats; j++)
        {
            pattern[i * repeats + j] = BasePattern[i];
        }

        return pattern;
    }

    private static string FormOutputString(IEnumerable<int> data)
    {
        return string.Join("", data);
    }
    
    private static int[] ParseNumbers(string input)
    {
        return input.Select(c => c - '0').ToArray();
    }
}