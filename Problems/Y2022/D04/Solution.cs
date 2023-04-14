using Problems.Common;

namespace Problems.Y2022.D04;

/// <summary>
/// Camp Cleanup: https://adventofcode.com/2022/day/4
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var pairs = ParseInputLines(parseFunc: ParseAssignmentPair);
        return part switch
        {
            1 => GetNumInclusivePairs(pairs),
            2 => GetNumIntersectingPairs(pairs),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetNumInclusivePairs(IEnumerable<(Pair P1, Pair P2)> pairs)
    {
        return pairs.Count(assignments => CheckContains(assignments.P1, assignments.P2));
    }
    
    private static int GetNumIntersectingPairs(IEnumerable<(Pair P1, Pair P2)> pairs)
    {
        return pairs.Count(assignments => CheckIntersects(assignments.P1, assignments.P2));
    }

    private static bool CheckContains(Pair a, Pair b)
    {
        return (a.N1 <= b.N1 && a.N2 >= b.N2) || (b.N1 <= a.N1 && b.N2 >= a.N2);
    }
    
    private static bool CheckIntersects(Pair a, Pair b)
    {
        return a.N1 <= b.N2 && b.N1 <= a.N2;
    }
    
    private static (Pair P1, Pair P2) ParseAssignmentPair(string assignmentPair)
    {
        var assignments = assignmentPair.Split(',');
        return (ParseAssignment(assignments[0]), ParseAssignment(assignments[1]));
    }
    
    private static Pair ParseAssignment(string assignment)
    {
        var sections = assignment.Split('-');
        return new Pair(int.Parse(sections[0]), int.Parse(sections[1]));
    }

    private readonly record struct Pair(int N1, int N2);
}