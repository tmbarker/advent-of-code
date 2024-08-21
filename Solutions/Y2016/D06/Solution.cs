using System.Text;
using Utilities.Collections;

namespace Solutions.Y2016.D06;

[PuzzleInfo("Signals and Noise", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => ParseMessage(encoding: Encoding.MostCommon),
            2 => ParseMessage(encoding: Encoding.LeastCommon),
            _ => PuzzleNotSolvedString
        };
    }

    private string ParseMessage(Encoding encoding)
    {
        var message = new StringBuilder();
        var messages = GetInputLines();
        var columnCounts = new DefaultDict<int, DefaultDict<char, int>>(
            defaultSelector: _ => new DefaultDict<char, int>(defaultValue: 0));

        for (var j = 0; j < messages[0].Length; j++)
        for (var i = 0; i < messages.Length; i++)
        {
            columnCounts[j][messages[i][j]]++;
        }

        for (var i = 0; i < messages[0].Length; i++)
        {
            var counts = columnCounts[i];
            var letter = encoding switch
            {
                Encoding.MostCommon =>  counts.Keys.MaxBy(c => counts[c]),
                Encoding.LeastCommon => counts.Keys.MinBy(c => counts[c]),
                _ => throw new NoSolutionException()
            };
            
            message.Append(letter);
        }
        
        return message.ToString();
    }
}