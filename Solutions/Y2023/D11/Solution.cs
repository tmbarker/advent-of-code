using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D11;

[PuzzleInfo("Cosmic Expansion", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => Expand(amount: 2L),
            2 => Expand(amount: 1000000L),
            _ => ProblemNotSolvedString
        };
    }

    private long Expand(long amount)
    {
        var grid = GetInputLines();
        var map = new Dictionary<int, Vector2D>();
        var rows = Enumerable.Range(start: 0, count: grid.Length).ToHashSet();
        var cols = Enumerable.Range(start: 0, count: grid[0].Length).ToHashSet();

        for (var y = 0; y < grid.Length; y++)
        for (var x = 0; x < grid[0].Length; x++)
        {
            if (grid[y][x] == '#')
            {
                map[map.Count] = new Vector2D(x, y);
                cols.Remove(x);
                rows.Remove(y);
            }
        }

        var sum = 0L;
        var done = new HashSet<(int, int)>();

        foreach (var (id1, pos1) in map)
        foreach (var (id2, pos2) in map)
        {
            if (done.Add((id1, id2)) && done.Add((id2, id1)))
            {
                var xMin = Math.Min(pos1.X, pos2.X);
                var xMax = Math.Max(pos1.X, pos2.X);
                var yMin = Math.Min(pos1.Y, pos2.Y);
                var yMax = Math.Max(pos1.Y, pos2.Y);
                
                var dx = (amount - 1) * cols.Count(x => x > xMin && x < xMax);
                var dy = (amount - 1) * rows.Count(y => y > yMin && y < yMax);

                sum += Vector2D.Distance(a: pos1, b: pos2, Metric.Taxicab) + dx + dy;
            }
        }

        return sum;
    }
}