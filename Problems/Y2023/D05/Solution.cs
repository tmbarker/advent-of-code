using Problems.Attributes;
using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2023.D05;

/// <summary>
/// If You Give A Seed A Fertilizer: https://adventofcode.com/2023/day/5
/// </summary>
[Favourite("If You Give A Seed A Fertilizer", Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var almanac = Almanac.Parse(input);
        
        return part switch
        {
            1 => FindMin(almanac, pairs: false),
            2 => FindMin(almanac, pairs: true),
            _ => ProblemNotSolvedString
        };
    }
    
    private static long FindMin(Almanac almanac, bool pairs)
    {
        var inputs = new Queue<Range<long>>(collection: MapSeeds(values: almanac.Seeds, pairs));
        var output = new Queue<Range<long>>();

        foreach (var map in almanac.Maps)
        {
            while (inputs.Any())
            {
                var range = inputs.Dequeue();
                foreach (var mapping in map.OrderedEntries)
                {
                    if (mapping.SourceRange.Contains(range))
                    {
                        output.Enqueue(item: mapping.Apply(range));
                        break;
                    }
                    
                    if (Range<long>.Overlap(a: mapping.SourceRange, b: range, out var overlap))
                    {
                        //  Because we are sweeping in ascending order, any partial overlap must
                        //  be on the upper bounds of the mapping source range
                        //
                        inputs.Enqueue(item: new Range<long>(min: mapping.SourceMax + 1, max: range.Max));
                        output.Enqueue(item: mapping.Apply(overlap));
                        break;
                    }
                }
            }

            (inputs, output) = (output, inputs);
        }

        return inputs.Min(range => range.Min);
    }
    
    private static IEnumerable<Range<long>> MapSeeds(IEnumerable<long> values, bool pairs)
    {
        if (!pairs)
        {
            return values.Select(Range<long>.Single);
        }

        return values
            .Chunk(size: 2)
            .Select(pair => new Range<long>(min: pair[0], max: pair[0] + pair[1] - 1));
    }
}