using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D09;

/// <summary>
/// Smoke Basin: https://adventofcode.com/2021/day/9
/// </summary>
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
            _ => ProblemNotSolvedString
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

    private static int MeasureBasinSize(Grid2D<int> grid, Vector2D lowPoint)
    {
        var queue = new Queue<Vector2D>();
        var visited = new HashSet<Vector2D>();
            
        queue.Enqueue(lowPoint);
        visited.Add(lowPoint);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            var height = grid[currentPos];
            visited.Add(currentPos);

            foreach (var adj in currentPos.GetAdjacentSet(Metric.Taxicab))
            {
                if (!grid.IsInDomain(adj) || visited.Contains(adj))
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
    
    private static IEnumerable<Vector2D> GetLowPoints(Grid2D<int> grid)
    {
        var lowPoints = new List<Vector2D>();
        
        for (var x = 0; x < grid.Width; x++)
        for (var y = 0; y < grid.Height; y++)
        {
            var pos = new Vector2D(x, y);
            var height = grid[pos];
            var lowerThanNeighbors = pos
                .GetAdjacentSet(Metric.Taxicab)
                .All(adj => !grid.IsInDomain(adj) || height < grid[adj]);

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