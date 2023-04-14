using Problems.Common;

namespace Problems.Y2020.D13;

/// <summary>
/// Shuttle Search: https://adventofcode.com/2020/day/13
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        ParseInput(GetInputLines(), out var timestamp, out var busIds);
        return part switch
        {
            1 => GetNextDeparture(timestamp, busIds),
            2 => GetFirstIncrementalDeparture(busIds),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetNextDeparture(int timestamp, IEnumerable<string> busIds)
    {
        var bestWait = int.MaxValue;
        var bestBus = 0;
        
        foreach (var entry in busIds)
        {
            if (!int.TryParse(entry, out var busId))
            {
                continue;
            }

            var wait = (int)Math.Ceiling((double)timestamp / busId) * busId - timestamp;
            if (wait < bestWait)
            {
                bestWait = wait;
                bestBus = busId;
            }
        }

        return bestWait * bestBus;
    }

    private static long GetFirstIncrementalDeparture(IList<string> busIds)
    {
        var time = 0L;
        var minPeriod = 1L;
        
        for (var i = 0; i < busIds.Count; i++)
        {
            if (!int.TryParse(busIds[i], out var busId))
            {
                continue;
            }
            
            while ((time + i) % busId != 0)
            {
                time += minPeriod;
            }
            
            minPeriod *= busId;
        }

        return time;
    }
    
    private static void ParseInput(IList<string> input, out int timestamp, out IList<string> busIds)
    {
        timestamp = int.Parse(input[0]);
        busIds = new List<string>(input[1].Split(','));
    }
}