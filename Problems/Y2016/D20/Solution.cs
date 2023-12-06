using Problems.Common;

namespace Problems.Y2016.D20;

/// <summary>
/// Firewall Rules: https://adventofcode.com/2016/day/20
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => Search(target: Target.LowestValid),
            2 => Search(target: Target.TotalAllowed),
            _ => ProblemNotSolvedString
        };
    }

    private long Search(Target target)
    {
        var depth = 0;
        var count = 0L;
        var endpoints = GetInputLines()
            .SelectMany(ParseEndpoints)
            .OrderBy(endpoint => endpoint.Value)
            .ToList();

        for (var i = 0; i < endpoints.Count; i++)
        {
            var endpoint = endpoints[i];
            var isFirst = i == 0;
            var isLast = i >= endpoints.Count - 1;
            
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
        var elements = line.Split(separator: '-');
        yield return new IntervalEndpoint(Value: long.Parse(elements[0]), Type: EndpointType.Start);
        yield return new IntervalEndpoint(Value: long.Parse(elements[1]), Type: EndpointType.End);
    }

    private readonly record struct IntervalEndpoint(long Value, EndpointType Type);
}