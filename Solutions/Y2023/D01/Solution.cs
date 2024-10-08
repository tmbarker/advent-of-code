using Utilities.Extensions;

namespace Solutions.Y2023.D01;

[PuzzleInfo("Trebuchet?!", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<string, int> SpelledDigits = new()
    {
        {"one",   1},
        {"two",   2},
        {"three", 3},
        {"four",  4},
        {"five",  5},
        {"six",   6},
        {"seven", 7},
        {"eight", 8},
        {"nine",  9}
    };
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Sum(ParseNaive),
            2 => Sum(ParseInterpreted),
            _ => PuzzleNotSolvedString
        };
    }

    private int Sum(Func<string, int> parseFunc) => ParseInputLines(parseFunc).Sum();
    
    private static int ParseNaive(string line)
    {
        return 10 * line.First(char.IsNumber).AsDigit() + line.Last(char.IsNumber).AsDigit();
    }
    
    private static int ParseInterpreted(string line)
    {
        var sum = 0;
        var digits = new List<int>();
        
        for (var i = 0; i < line.Length; i++)
        {
            if (char.IsNumber(line[i]))
            {
                digits.Add(item: line[i].AsDigit());
                continue;
            }

            foreach (var (spelled, digit) in SpelledDigits)
            {
                if (i + spelled.Length - 1 < line.Length && line[i..(i + spelled.Length)] == spelled)
                {
                    digits.Add(digit);
                    break;
                }
            }
        }

        sum += 10 * digits.First();
        sum += digits.Last();

        return sum;
    }
}