using System.Text.RegularExpressions;
using Utilities.Collections;
using Utilities.Extensions;

namespace Solutions.Y2018.D04;

[PuzzleInfo("Repose Record", Topics.StringParsing|Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct LogEntry(DateTime DateTime, string Observation);
    
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm";
    private const string Guard = "Guard";
    private const string Falls = "falls";
    private const string Wakes = "wakes";

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

    private static IDictionary<int, List<int>> BuildSleepMap(IEnumerable<LogEntry> logs)
    {
        var sleepMap = new DefaultDict<int, List<int>>(defaultSelector: _ => []);
        var ordered = logs
            .OrderBy(l => l.DateTime)
            .ToList();
     
        var onDutyId = 0;
        var asleepAt = 0;
        
        foreach (var (dateTime, observation) in ordered)
        {
            switch (observation)
            {
                case not null when observation.StartsWith(Guard):
                    onDutyId = observation.ParseInts().Single();
                    break;
                case not null when observation.StartsWith(Falls):
                    asleepAt = dateTime.Minute;
                    break;
                case not null when observation.StartsWith(Wakes):
                    sleepMap[onDutyId].AddRange(Enumerable.Range(
                        start: asleepAt,
                        count: dateTime.Minute - asleepAt));
                    break;
            }
        }

        return sleepMap;
    }

    private static int EvaluateStrategy1(IDictionary<int, List<int>> sleepMap)
    {
        var mostAsleep = sleepMap.MaxBy(kvp => kvp.Value.Count);
        var mostCommonlyAt = mostAsleep.Value.Mode();

        return mostAsleep.Key * mostCommonlyAt;
    }
    
    private static int EvaluateStrategy2(IDictionary<int, List<int>> sleepMap)
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

    private static LogEntry ParseLog(string line)
    { 
        var match = Regex.Match(line, pattern: @"^\[(.+)\] (.+)$");
        var observation = match.Groups[2].Value;
        var dateTime = DateTime.ParseExact(
            s: match.Groups[1].Value,
            format: DateTimeFormat,
            provider: null);

        return new LogEntry(dateTime, observation);
    }
}