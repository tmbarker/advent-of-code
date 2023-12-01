using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D04;

/// <summary>
/// Camp Cleanup: https://adventofcode.com/2022/day/4
/// </summary>
public class Solution : SolutionBase
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

    private static int CountEncapsulating(IEnumerable<(Aabb1D R1, Aabb1D R2)> assignments)
    {
        return assignments.Count(assignment => CheckContains(assignment.R1, assignment.R2));
    }
    
    private static int CountIntersecting(IEnumerable<(Aabb1D R1, Aabb1D R2)> assignments)
    {
        return assignments.Count(assignment => Aabb1D.Overlap(assignment.R1, assignment.R2, out _));
    }

    private static bool CheckContains(Aabb1D a, Aabb1D b)
    {
        return (a.Min <= b.Min && a.Max >= b.Max) || (b.Min <= a.Min && b.Max >= a.Max);
    }

    private static (Aabb1D R1, Aabb1D R2) ParseAssignment(string line)
    {
        var assignments = line.Split(separator: ',');
        return (ParseRange(assignments[0]), ParseRange(assignments[1]));
    }
    
    private static Aabb1D ParseRange(string range)
    {
        var sections = range.Split(separator: '-');
        return new Aabb1D(
            min: int.Parse(sections[0]),
            max: int.Parse(sections[1]));
    }
}