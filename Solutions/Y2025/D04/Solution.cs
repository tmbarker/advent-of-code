using Utilities.Geometry.Euclidean;

namespace Solutions.Y2025.D04;

[PuzzleInfo("Printing Department", Topics.Simulation, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var paper = GetInputGrid()
            .FindAll('@')
            .ToHashSet();
        
        return part switch
        {
            1 => Part1(paper),
            2 => Part2(paper),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Part1(HashSet<Vec2D> paper)
    {
        return GetAccessible(paper).Count;
    }

    private static int Part2(HashSet<Vec2D> paper)
    {
        var set = 0;
        var add = 1;
        
        while (add > 0)
        {
            var @new = GetAccessible(paper);
            add = @new.Count;
            set += add;
            paper.ExceptWith(@new);
        }
        
        return set;
    }

    private static HashSet<Vec2D> GetAccessible(HashSet<Vec2D> paper)
    {
        return paper
            .Where(pos => pos
                .GetChebyshevAdjacent()
                .Count(paper.Contains) < 4)
            .ToHashSet();
    }
}