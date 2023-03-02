using Problems.Y2022.Common;
using Utilities.Cartesian;

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
    
    public override object Run(int part)
    {
        return part switch
        {
            1 =>  GetFewestStepsFromStart(),
            2 =>  GetFewestStepsFromMinHeight(),
            _ => ProblemNotSolvedString
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
        
        bool MoveAllowed(char from, char to) => to >= from || from - to == 1;

        var minDistances = GetMinDistancesMap(grid, bestSignalPos, MoveAllowed);
        var allPositions = grid.GetAllPositions();
        
        return allPositions
            .Where(p => grid[p] == MinHeight)
            .Select(position => minDistances[position])
            .Min();
    }

    private static Dictionary<Vector2D, int> GetMinDistancesMap(Grid2D<char> grid, Vector2D start, Func<char, char, bool> moveAllowedFunc)
    {
        var allPositions = grid.GetAllPositions().ToList();
        var unvisited = new HashSet<Vector2D>(allPositions);
        var distances = allPositions.ToDictionary(n => n, _ => int.MaxValue);

        distances[start] = 0;

        for (var i = 0; i < allPositions.Count; i++)
        {
            var current = GetClosestUnvisited(distances, unvisited);
            unvisited.Remove(current);
            
            // Unreachable position from start
            if (distances[current] == int.MaxValue)
            {
                continue;
            }

            foreach (var move in MovesSet)
            {
                var next = current + move;
                if (!grid.IsInDomain(next) || !moveAllowedFunc(grid[current], grid[next]))
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
        grid = Grid2D<char>.MapChars(GetInputLines(), c => c);
        start = grid.Single(kvp => kvp.Value == StartMarker).Key;
        end = grid.Single(kvp => kvp.Value == EndMarker).Key;

        grid[start] = StartHeight;
        grid[end] = EndHeight;
    }
}