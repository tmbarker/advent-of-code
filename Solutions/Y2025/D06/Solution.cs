using Utilities.Extensions;

namespace Solutions.Y2025.D06;

[PuzzleInfo("Trash Compactor", Topics.StringParsing|Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        return part switch
        {
            1 => Part1(input),
            2 => Part2(input),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Part1(string[] input)
    {
        var rows = input[..^1]
            .Select(line => line.ParseLongs())
            .ToList();

        return input[^1]
            .Where(c => !char.IsWhiteSpace(c))
            .Select((op, i) => Solve(op, operands: rows.Select(row => row[i])))
            .Sum();
    }

    private static long Part2(string[] input)
    {
        var total = 0L;
        var rows = input.Length;
        var cols = input[0].Length;
        
        var operands = new List<long>();
        for (var c = cols - 1; c >= 0; c--)
        {
            operands.Add(0L);
            for (var r = 0; r < rows - 1; r++)
            {
                if (!char.IsWhiteSpace(input[r][c]))
                {
                    operands[^1] *= 10L;
                    operands[^1] += input[r][c].AsDigit();
                }
            }
            
            var op = input[rows - 1][c];
            if (!char.IsWhiteSpace(op))
            {
                total += Solve(op, operands);
                operands.Clear();
                c--;
            }
        }
        
        return total;
    }

    private static long Solve(char op, IEnumerable<long> operands)
    {
        return operands.Aggregate(
            seed: op == '+' ? 0L : 1L,
            func: (acc, n) => op == '+' ? acc + n : acc * n);
    }
}