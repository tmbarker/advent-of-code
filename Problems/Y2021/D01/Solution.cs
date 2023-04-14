using Problems.Common;

namespace Problems.Y2021.D01;

/// <summary>
/// Sonar Sweep: https://adventofcode.com/2021/day/1
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
       return part switch
        {
            1 => CountDepthIncreases(windowSize: 1),
            2 => CountDepthIncreases(windowSize: 3),
            _ => ProblemNotSolvedString
        };
    }

    private int CountDepthIncreases(int windowSize)
    {
        var numIncreases = 0;
        var window = new Queue<int>(windowSize);
        
        var depths = GetInputLines()
            .Select(int.Parse)
            .ToList();

        foreach (var depth in depths)
        {
            if (window.Count < windowSize)
            {
                window.Enqueue(depth);
                continue;
            }

            var prevSum = window.Sum();
            window.Dequeue();
            window.Enqueue(depth);
            var curSum = window.Sum();

            if (curSum > prevSum)
            {
                numIncreases++;
            }
        }

        return numIncreases;
    }
}