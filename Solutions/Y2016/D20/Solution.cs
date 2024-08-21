using Utilities.Numerics;

namespace Solutions.Y2016.D20;

[PuzzleInfo("Firewall Rules", Topics.Math, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private readonly record struct IntervalEndpoint(long Value, EndpointType Type);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Search(target: Target.LowestValid),
            2 => Search(target: Target.TotalAllowed),
            _ => PuzzleNotSolvedString
        };
    }

    private long Search(Target target)
    {
        var depth = 0;
        var count = 0L;
        var endpoints = GetInputLines()
            .SelectMany(ParseEndpoints)
            .OrderBy(endpoint => endpoint.Value)
            .ToArray();

        for (var i = 0; i < endpoints.Length; i++)
        {
            var endpoint = endpoints[i];
            var isFirst = i == 0;
            var isLast = i >= endpoints.Length - 1;
            
            if (endpoint.Type == EndpointType.Start)
            {
                if (depth == 0 && !isFirst)
                {
                    count += endpoint.Value - endpoints[i - 1].Value - 1;
                }
                
                depth++;
                continue;
            }

            var blockEnded = --depth == 0;
            var gapExists = !isLast && endpoint.Value + 1L != endpoints[i + 1].Value;
            
            if (blockEnded && gapExists && target == Target.LowestValid)
            {
                return endpoint.Value + 1L;
            }
        }

        return count;
    }
    
    private static IEnumerable<IntervalEndpoint> ParseEndpoints(string line)
    {
        var interval = Range<long>.Parse(line);
        yield return new IntervalEndpoint(Value: interval.Min, Type: EndpointType.Start);
        yield return new IntervalEndpoint(Value: interval.Max, Type: EndpointType.End);
    }
}