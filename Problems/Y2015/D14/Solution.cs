using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2015.D14;

/// <summary>
/// Reindeer Olympics: https://adventofcode.com/2015/day/14
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var reindeer = ParseInputLines(parseFunc: ParseReindeer).ToList();
        return part switch
        {
            1 => GetMaxDistance(reindeer, time: 2503),
            2 => GetMaxScore(reindeer, time: 2503),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetMaxDistance(IEnumerable<Reindeer> reindeer, int time)
    {
        return reindeer.Max(deer => DistanceAtTime(deer, time));
    }
    
    private static int GetMaxScore(IList<Reindeer> reindeer, int time)
    {
        var scores = reindeer.ToDictionary(
            keySelector: deer => deer.Name,
            elementSelector: _ => 0);

        foreach (var tick in Enumerable.Range(1, time))
        {
            var winners = reindeer
                .GroupBy(deer => DistanceAtTime(deer, tick))
                .OrderByDescending(grouping =>  grouping.Key)
                .First();

            foreach (var winner in winners)
            {
                scores[winner.Name]++;
            }
        }

        return scores.Values.Max();
    }
    
    private static int DistanceAtTime(Reindeer reindeer, int time)
    {
        var cycleDuration = reindeer.FlyDuration + reindeer.RestDuration;
        var wholeCycles = time / cycleDuration;
        var remainder = Math.Min(reindeer.FlyDuration, time - cycleDuration * wholeCycles);

        return (wholeCycles * reindeer.FlyDuration + remainder) * reindeer.FlySpeed;
    }
    
    private static Reindeer ParseReindeer(string line)
    {
        var numbers = line.ParseInts();
        return new Reindeer(
            Name: line.Split(' ')[0],
            FlySpeed:     numbers[0],
            FlyDuration:  numbers[1],
            RestDuration: numbers[2]);
    }
    
    private readonly record struct Reindeer(string Name, int FlySpeed, int FlyDuration, int RestDuration);
}