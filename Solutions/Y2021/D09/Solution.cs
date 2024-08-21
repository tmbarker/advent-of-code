using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D09;

[PuzzleInfo("Smoke Basin", Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int BasinBoundaryHeight = 9;
    private const int NumBasinsToMultiply = 3;

    public override object Run(int part)
    {
        var grid = ParseGrid(GetInputLines());
        return part switch
        {
            1 => SumRiskLevel(grid),
            2 => MeasureLargestBasins(grid),
            _ => PuzzleNotSolvedString
        };
    }

    private static int SumRiskLevel(Grid2D<int> grid)
    {
        return GetLowPoints(grid).Sum(lowPoint => grid[lowPoint] + 1);
    }
    
    private static int MeasureLargestBasins(Grid2D<int> grid)
    {
        return GetLowPoints(grid)
            .Select(lowPoint => MeasureBasinSize(grid, lowPoint))
            .OrderDescending()
            .Take(NumBasinsToMultiply)
            .Aggregate((i, j) => i * j);
    }

    private static int MeasureBasinSize(Grid2D<int> grid, Vec2D lowPoint)
    {
        var queue = new Queue<Vec2D>();
        var visited = new HashSet<Vec2D>();
            
        queue.Enqueue(lowPoint);
        visited.Add(lowPoint);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            var height = grid[currentPos];
            visited.Add(currentPos);

            foreach (var adj in currentPos.GetAdjacentSet(Metric.Taxicab))
            {
                if (!grid.Contains(adj) || visited.Contains(adj))
                {
                    continue;
                }

                var neighborHeight = grid[adj];
                if (neighborHeight >= BasinBoundaryHeight || neighborHeight <= height)
                {
                    continue;
                }
                    
                queue.Enqueue(adj);
                visited.Add(currentPos);
            }
        }

        return visited.Count;
    }
    
    private static IEnumerable<Vec2D> GetLowPoints(Grid2D<int> grid)
    {
        var lowPoints = new List<Vec2D>();
        
        for (var x = 0; x < grid.Width; x++)
        for (var y = 0; y < grid.Height; y++)
        {
            var pos = new Vec2D(x, y);
            var height = grid[pos];
            var lowerThanNeighbors = pos
                .GetAdjacentSet(Metric.Taxicab)
                .All(adj => !grid.Contains(adj) || height < grid[adj]);

            if (lowerThanNeighbors)
            {
                lowPoints.Add(pos);
            }
        }

        return lowPoints;
    }

    private static Grid2D<int> ParseGrid(IList<string> lines)
    {
        return Grid2D<int>.MapChars(lines, elementFunc: StringExtensions.AsDigit);
    }
}