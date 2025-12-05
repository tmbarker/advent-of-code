using Utilities.Numerics;

namespace Solutions.Y2025.D05;

[PuzzleInfo("Cafeteria", Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => Part1(),
            2 => Part2(),
            _ => PuzzleNotSolvedString
        };
    }

    private int Part1()
    {
        var input = ChunkInputByNonEmpty();
        var ranges = input[0]
            .Select(Range<long>.Parse)
            .ToList();

        return input[1]
            .Select(long.Parse)
            .Count(ingredient => ranges.Any(range => range.Contains(ingredient)));
    }
    
    private long Part2()
    {
        var ranges = ChunkInputByNonEmpty()[0]
            .Select(Range<long>.Parse)
            .OrderBy(range => range.Min)
            .ToList();
        
        var merged = new List<Range<long>>();
        var current = ranges[0];
        
        for (var i = 1; i < ranges.Count; i++)
        {
            if (Range<long>.Overlap(a: current, b: ranges[i], overlap: out _))
            {
                current = new Range<long>(
                    min: current.Min,
                    max: Math.Max(current.Max, ranges[i].Max));
            }
            else
            {
                merged.Add(current);
                current = ranges[i];
            }
        }
        
        return merged
            .Append(current)
            .Sum(range => range.Length);
    }
}