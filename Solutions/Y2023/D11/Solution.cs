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
        var pois = new List<Vector2D>();
        var rows = Enumerable.Range(start: 0, count: grid.Length).ToHashSet();
        var cols = Enumerable.Range(start: 0, count: grid[0].Length).ToHashSet();

        for (var y = 0; y < grid.Length; y++)
        for (var x = 0; x < grid[0].Length; x++)
        {
            if (grid[y][x] == '#')
            {
                pois.Add(item: new Vector2D(x, y));
                cols.Remove(x);
                rows.Remove(y);
            }
        }

        var sum = 0L;

        for (var i = 0; i < pois.Count - 1; i++)
        for (var j = i + 1; j < pois.Count; j++)
        {
            var xMin = Math.Min(pois[i].X, pois[j].X);
            var xMax = Math.Max(pois[i].X, pois[j].X);
            var yMin = Math.Min(pois[i].Y, pois[j].Y);
            var yMax = Math.Max(pois[i].Y, pois[j].Y);
                
            var dx = (amount - 1) * cols.Count(x => x > xMin && x < xMax);
            var dy = (amount - 1) * rows.Count(y => y > yMin && y < yMax);

            sum += Vector2D.Distance(a: pois[i], b: pois[j], Metric.Taxicab) + dx + dy;
        }

        return sum;
    }
}