using System.Text;
using Problems.Common;

namespace Problems.Y2016.D06;

/// <summary>
/// Signals and Noise: https://adventofcode.com/2016/day/6
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => ParseMessage(encoding: Encoding.MostCommon),
            2 => ParseMessage(encoding: Encoding.LeastCommon),
            _ => ProblemNotSolvedString
        };
    }

    private string ParseMessage(Encoding encoding)
    {
        var message = new StringBuilder();
        var messages = GetInputLines();
        var columnCounts = new Dictionary<int, Dictionary<char, int>>();

        for (var x = 0; x < messages[0].Length; x++)
        {
            columnCounts[x] = new Dictionary<char, int>();
            foreach (var sequence in messages)
            {
                columnCounts[x].TryAdd(sequence[x], 0);
                columnCounts[x][sequence[x]]++;
            }
        }

        for (var x = 0; x < messages[0].Length; x++)
        {
            var counts = columnCounts[x];
            var letter = encoding switch
            {
                Encoding.MostCommon => counts.Keys.MaxBy(c => counts[c]),
                Encoding.LeastCommon => counts.Keys.MinBy(c => counts[c]),
                _ => throw new ArgumentOutOfRangeException(nameof(encoding))
            };

            message.Append(letter);
        }
        
        return message.ToString();
    }
}