using System.Text;
using Utilities.Extensions;

namespace Solutions.Y2022.D25;

[PuzzleInfo("Full of Hot Air", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int SnafuRadix = 5;
    private const char MinusOne = '-';
    private const char MinusTwo = '=';

    private static readonly Dictionary<long, char> SnafuDigits = new()
    {
        { 2, '2' },
        { 1, '1' },
        { 0, '0' },
        { -1, MinusOne },
        { -2, MinusTwo }
    };
    
    public override int Parts => 1;

    public override object Run(int part)
    {
        return part switch
        {
            1 => DecimalToSnafu(SumSnafuNumbers(GetInputLines())),
            _ => ProblemNotSolvedString
        };
    }

    private static long SumSnafuNumbers(IEnumerable<string> snafuNumbers)
    {
        return snafuNumbers.Select(SnafuToDecimal).Sum();
    }
    
    private static string DecimalToSnafu(long snafuNumber)
    {
        var digits = (int)Math.Ceiling(Math.Log(snafuNumber, SnafuRadix));
        var snafuSb = new StringBuilder();
        
        for (var i = digits - 1; i >= 0; i--)
        {
            var v = Math.Pow(SnafuRadix, i);
            var d = (long)Math.Round(snafuNumber / v);

            snafuNumber -= (long)(d * v);
            snafuSb.Append(SnafuDigits[d]);
        }
        return snafuSb.ToString();
    }
    
    private static long SnafuToDecimal(string snafu)
    {
        var sum = 0L;
        for (var i = 0; i < snafu.Length; i++)
        {
            var c = snafu[snafu.Length - 1 - i];
            var d = ParseSnafuDigit(c);

            sum += (long)(d * Math.Pow(SnafuRadix, i));
        }
        return sum;
    }

    private static int ParseSnafuDigit(char digit)
    {
        return digit switch
        {
            MinusOne => -1,
            MinusTwo => -2,
            _ => digit.AsDigit()
        };
    }
}