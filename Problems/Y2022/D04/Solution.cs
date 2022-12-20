using Problems.Y2022.Common;

namespace Problems.Y2022.D04;

/// <summary>
/// Camp Cleanup: https://adventofcode.com/2022/day/4
/// </summary>
public class Solution : SolutionBase2022
{
    private const char PairDelimiter = ',';
    private const char SectionDelimiter = '-';

    public override int Day => 4;
    
    public override object Run(int part)
    { 
        return part switch
        {
            0 => GetNumInclusivePairs(),
            1 => GetNumIntersectingPairs(),
            _ => ProblemNotSolvedString,
        };
    }

    private int GetNumInclusivePairs()
    {
        var assignmentPairs = GetInput();
        return assignmentPairs
            .Select(ParseAssignmentPair)
            .Count(assignments => CheckSetsForContainment(assignments.Item1, assignments.Item2));
    }
    
    private int GetNumIntersectingPairs()
    {
        var assignmentPairs = GetInput();
        return assignmentPairs
            .Select(ParseAssignmentPair)
            .Count(assignments => CheckSetsForIntersection(assignments.Item1, assignments.Item2));
    }

    private static ((int, int), (int, int)) ParseAssignmentPair(string assignmentPair)
    {
        var assignments = assignmentPair.Split(PairDelimiter);
        return (ParseAssignment(assignments[0]), ParseAssignment(assignments[1]));
    }
    
    private static (int, int) ParseAssignment(string assignment)
    {
        var sections = assignment.Split(SectionDelimiter);
        return (int.Parse(sections[0]), int.Parse(sections[1]));
    }

    private static bool CheckSetsForContainment((int, int) a, (int, int) b)
    {
        return (a.Item1 <= b.Item1 && a.Item2 >= b.Item2) || (b.Item1 <= a.Item1 && b.Item2 >= a.Item2);
    }
    
    private static bool CheckSetsForIntersection((int, int) a, (int, int) b)
    {
        return a.Item1 <= b.Item2 && b.Item1 <= a.Item2;
    }
}