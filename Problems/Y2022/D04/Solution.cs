using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D04;

/// <summary>
/// Camp Cleanup: https://adventofcode.com/2022/day/4
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var assignments = ParseInputLines(parseFunc: ParseAssignment);
        return part switch
        {
            1 => CountEncapsulating(assignments),
            2 => CountIntersecting(assignments),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountEncapsulating(IEnumerable<(Range<int> R1, Range<int> R2)> assignments)
    {
        return assignments.Count(assignment => CheckContains(a: assignment.R1, b: assignment.R2));
    }
    
    private static int CountIntersecting(IEnumerable<(Range<int> R1, Range<int> R2)> assignments)
    {
        return assignments.Count(assignment => Range<int>.Overlap(a: assignment.R1, b: assignment.R2, out _));
    }

    private static bool CheckContains(Range<int> a, Range<int> b)
    {
        return a.Contains(b) || b.Contains(a);
    }

    private static (Range<int> R1, Range<int> R2) ParseAssignment(string line)
    {
        var assignments = line.Split(separator: ',');
        var r1 = Range<int>.Parse(assignments[0]);
        var r2 = Range<int>.Parse(assignments[1]);

        return (r1, r2);
    }
}