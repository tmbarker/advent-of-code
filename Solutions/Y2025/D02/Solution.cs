using Utilities.Numerics;

namespace Solutions.Y2025.D02;

[PuzzleInfo("Gift Shop", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => SumInvalid(Part1),
            2 => SumInvalid(Part2),
            _ => PuzzleNotSolvedString
        };
    }

    private long SumInvalid(Predicate<long> predicate)
    {
        return GetInputText()
            .Split(',')
            .Select(Range<long>.Parse)
            .AsParallel()
            .WithDegreeOfParallelism(Environment.ProcessorCount)
            .SelectMany(range => range)
            .Where(num => predicate(num))
            .Sum();
    }
    
    private static bool Part1(long num)
    {
        Span<char> span = stackalloc char[20];
        num.TryFormat(span, out var chars);
        var id = span[..chars];
        
        return IsInvalid(id, parts: 2);
    }

    private static bool Part2(long num)
    {
        Span<char> span = stackalloc char[20];
        num.TryFormat(span, out var chars);
        var id = span[..chars];
        
        for (var i = 2; i <= id.Length; i++)
        {
            if (IsInvalid(id, parts: i))
            {
                return true;
            }
        }
        
        return false;
    }

    private static bool IsInvalid(ReadOnlySpan<char> id, int parts)
    {
        if (id.Length % parts != 0)
        {
            return false;
        }

        var partLength = id.Length / parts;
        var partSequence = id[..partLength];
        
        for (var i = 1; i < parts; i++)
        {
            if (!partSequence.SequenceEqual(id.Slice(start: i * partLength, partLength)))
            {
                return false;
            }
        }
        
        return true;
    }
}