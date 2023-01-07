using Problems.Y2021.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2021.D09;

/// <summary>
/// Smoke Basin: https://adventofcode.com/2021/day/9
/// </summary>
public class Solution : SolutionBase2021
{
    private const int BasinBoundaryHeight = 9;
    private const int NumBasinsToMultiply = 3;

    public override int Day => 9;
    
    public override object Run(int part)
    {
        var grid = ParseGrid(GetInputLines());
        return part switch
        {
            0 => SumRiskLevel(grid),
            1 => MeasureLargestBasins(grid),
            _ => ProblemNotSolvedString,
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
            .OrderByDescending(s => s)
            .Take(NumBasinsToMultiply)
            .Aggregate((i, j) => i * j);
    }

    private static int MeasureBasinSize(Grid2D<int> grid, Vector2D lowPoint)
    {
        var queue = new Queue<Vector2D>();
        var visited = new HashSet<Vector2D>();
            
        queue.Enqueue(lowPoint);
        visited.EnsureContains(lowPoint);

        while (queue.Count > 0)
        {
            var currentPos = queue.Dequeue();
            var height = grid[currentPos];
            visited.EnsureContains(currentPos);

            foreach (var adj in currentPos.GetAdjacentSet(DistanceMetric.Taxicab))
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
                visited.EnsureContains(currentPos);
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
            var lowerThanNeighbors = true;
            
            foreach (var adj in pos.GetAdjacentSet(DistanceMetric.Taxicab))
            {
                if (!grid.IsInDomain(adj) || height < grid[adj])
                {
                    continue;
                }
                
                lowerThanNeighbors = false;
                break;
            }

            if (lowerThanNeighbors)
            {
                lowPoints.Add(pos);
            }
        }

        return lowPoints;
    }

    private static Grid2D<int> ParseGrid(IList<string> lines)
    {
        var rows = lines.Count;
        var cols = lines[0].Length;
        var grid = Grid2D<int>.WithDimensions(rows, cols);
        
        for (var x = 0; x < cols; x++)
        for (var y = 0; y < rows; y++)
        {
            grid[x, rows - 1 - y] = lines[y][x] - '0';
        }
        
        return grid;
    }
}