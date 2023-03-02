using Problems.Y2022.Common;

namespace Problems.Y2022.D04;

/// <summary>
/// Camp Cleanup: https://adventofcode.com/2022/day/4
/// </summary>
public class Solution : SolutionBase2022
{
    public override int Day => 4;
    
    public override object Run(int part)
    { 
        return part switch
        {
            1 =>  GetNumInclusivePairs(),
            2 =>  GetNumIntersectingPairs(),
            _ => ProblemNotSolvedString
        };
    }

    private int GetNumInclusivePairs()
    {
        var assignmentPairs = GetInputLines();
        return assignmentPairs
            .Select(ParseAssignmentPair)
            .Count(assignments => CheckSetsForContainment(assignments.P1, assignments.P2));
    }
    
    private int GetNumIntersectingPairs()
    {
        var assignmentPairs = GetInputLines();
        return assignmentPairs
            .Select(ParseAssignmentPair)
            .Count(assignments => CheckSetsForIntersection(assignments.P1, assignments.P2));
    }

    private static bool CheckSetsForContainment(Pair a, Pair b)
    {
        return (a.N1 <= b.N1 && a.N2 >= b.N2) || (b.N1 <= a.N1 && b.N2 >= a.N2);
    }
    
    private static bool CheckSetsForIntersection(Pair a, Pair b)
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