using Problems.Y2022.Common;
using Utilities.DataStructures.Grid;

namespace Problems.Y2022.D12;

/// <summary>
/// Hill Climbing Algorithm: https://adventofcode.com/2022/day/12
/// </summary>
public class Solution : SolutionBase2022
{
    private const char MinHeight = 'a';
    private const char StartHeight = MinHeight;
    private const char StartMarker = 'S';
    private const char EndHeight = 'z';
    private const char EndMarker = 'E';

    private static readonly HashSet<Vector2D> MovesSet = new()
    {
        Vector2D.Up,
        Vector2D.Down,
        Vector2D.Left,
        Vector2D.Right,
    };

    public override int Day => 12;
    
    public override string Run(int part)
    {
        return part switch
        {
            0 => GetFewestStepsFromStart().ToString(),
            1 => GetFewestStepsFromMinHeight().ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int GetFewestStepsFromStart()
    {
        ParseGrid(out var grid, out var start, out var bestSignalPos);
        
        bool MoveAllowed(char from, char to) => !(to - from > 1);
        return GetMinDistancesMap(grid, start, MoveAllowed)[bestSignalPos];
    }

    private int GetFewestStepsFromMinHeight()
    {
        ParseGrid(out var grid, out _, out var bestSignalPos);

        bool MoveAllowed(char from, char to)
        {
            if (to >= from)
            {
                return true;
            }

            return from - to == 1;
        }
        
        var minDistances = GetMinDistancesMap(grid, bestSignalPos, MoveAllowed);
        var allPositions = grid.EnumerateAllPositions();
        var min = int.MaxValue;

        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var position in allPositions)
        {
            if (grid[position] == MinHeight)
            {
                min = Math.Min(min, minDistances[position]);
            }
        }

        return min;
    }

    private static Dictionary<Vector2D, int> GetMinDistancesMap(Grid2D<char> grid, Vector2D start, Func<char, char, bool> moveAllowedFunc)
    {
        var allPositions = grid.EnumerateAllPositions().ToList();
        var unvisited = new HashSet<Vector2D>(allPositions);
        var distances = allPositions.ToDictionary(n => n, _ => int.MaxValue);

        distances[start] = 0;

        for (var i = 0; i < allPositions.Count; i++)
        {
            var current = GetClosestUnvisited(distances, unvisited);
            unvisited.Remove(current);

            foreach (var move in MovesSet)
            {
                var next = current + move;
                if (!grid.IsInDomain(next))
                {
                    continue;
                }

                // Illegal move
                if (!moveAllowedFunc(grid[current], grid[next]))
                {
                    continue;
                }
                
                // Unreachable position from start
                if (distances[current] == int.MaxValue)
                {
                    continue;
                }
                
                var distanceViaCurrent = distances[current] + 1; 
                if (distanceViaCurrent < distances[next])
                {
                    distances[next] = distanceViaCurrent;
                }
            }
        }

        return distances;
    }

    private static Vector2D GetClosestUnvisited(Dictionary<Vector2D, int> distances, IReadOnlySet<Vector2D> unvisited)
    {
        var min = int.MaxValue;
        var closest = Vector2D.Zero;

        foreach (var (position, distance) in distances)
        {
            if (!unvisited.Contains(position) || distance > min)
            {
                continue;
            }
            
            min = distance;
            closest = position;
        }

        return closest;
    }

    private void ParseGrid(out Grid2D<char> grid, out Vector2D start, out Vector2D end)
    {
        var lines = GetInput();
        var rows = lines.Length;
        var cols = lines[0].Length;
        
        grid = Grid2D<char>.WithDimensions(rows, cols);
        start = Vector2D.Zero;
        end = Vector2D.Zero;
        
        for (var x = 0; x < cols; x++)
        for (var y = 0; y < rows; y++)
        {
            var height = lines[y][x];
            if (height == StartMarker)
            {
                height = StartHeight;
                start = new Vector2D(x, y);
            }
            
            if (height == EndMarker)
            {
                height = EndHeight;
                end = new Vector2D(x, y);
            }

            grid[x, y] = height;
        }
    }
}