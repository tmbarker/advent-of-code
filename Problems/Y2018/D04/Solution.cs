using System.Text.RegularExpressions;
using Problems.Y2018.Common;
using Utilities.Extensions;

namespace Problems.Y2018.D04;

/// <summary>
/// Repose Record: https://adventofcode.com/2018/day/4
/// </summary>
public class Solution : SolutionBase2018
{
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    private const string Guard = "Guard";
    private const string Falls = "falls";
    private const string Wakes = "wakes";
        
    private static readonly Regex LogRegex = new(@"^\[(.+)\] (.+)$");
    
    public override int Day => 4;
    
    public override object Run(int part)
    {
        var logs = ParseInputLines(parseFunc: ParseLog);
        var map = BuildSleepMap(logs);
        
        return part switch
        {
            1 => EvaluateStrategy1(map),
            2 => EvaluateStrategy2(map),
            _ => ProblemNotSolvedString
        };
    }

    private static Dictionary<int, List<int>> BuildSleepMap(IEnumerable<Log> logs)
    {
        var sleepMap = new Dictionary<int, List<int>>();
        var ordered = logs
            .OrderBy(l => l.DateTime)
            .ToList();
     
        var onDutyId = 0;
        var asleepAt = 0;
        
        foreach (var (dateTime, observation) in ordered)
        {
            switch (observation)
            {
                case {} when observation.StartsWith(Guard):
                    onDutyId = observation.Numbers().Single();
                    break;
                case {} when observation.StartsWith(Falls):
                    asleepAt = dateTime.Minute;
                    break;
                case {} when observation.StartsWith(Wakes):
                    sleepMap.EnsureContainsKey(onDutyId, new List<int>());
                    sleepMap[onDutyId].AddRange(Enumerable.Range(
                        start: asleepAt,
                        count: dateTime.Minute - asleepAt));
                    break;
            }
        }

        return sleepMap;
    }

    private static int EvaluateStrategy1(Dictionary<int, List<int>> sleepMap)
    {
        var mostAsleep = sleepMap.MaxBy(kvp => kvp.Value.Count);
        var mostCommonlyAt = mostAsleep.Value.Mode();

        return mostAsleep.Key * mostCommonlyAt;
    }
    
    private static int EvaluateStrategy2(Dictionary<int, List<int>> sleepMap)
    {
        var maxAsleep = 0;
        var bestGuard = 0;
        var bestMinute = 0;

        foreach (var (id, minutes) in sleepMap)
        {
            var distinctMinutes = minutes.Distinct();
            var minuteCounts = distinctMinutes.ToDictionary(
                keySelector: minute => minute,
                elementSelector: minute => minutes.Count(m => m == minute));
            
            var max = minuteCounts.MaxBy(kvp => kvp.Value);
            if (max.Value <= maxAsleep)
            {
                continue;
            }

            maxAsleep = max.Value;
            bestGuard = id;
            bestMinute = max.Key;
        }

        return bestGuard * bestMinute;
    }

    private static Log ParseLog(string line)
    {
        var match = LogRegex.Match(line);
        var dateTime = DateTime.ParseExact(s: match.Groups[1].Value, format: DateTimeFormat, provider: null);
        var observation = match.Groups[2].Value;

        return new Log(dateTime, observation);
    }
}