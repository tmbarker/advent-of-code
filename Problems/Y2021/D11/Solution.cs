using Problems.Y2021.Common;
using Utilities.DataStructures.Cartesian;

namespace Problems.Y2021.D11;

/// <summary>
/// Dumbo Octopus: https://adventofcode.com/2021/day/11
/// </summary>
public class Solution : SolutionBase2021
{
    private const int Steps = 100;
    
    public override int Day => 11;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => CountFlashes(Steps),
            1 => WaitForAllFlashed().Result, 
            _ => ProblemNotSolvedString,
        };
    }

    private int CountFlashes(int steps)
    {
        var flashes = 0;
        var octopusGrid = new OctopusGrid(GetInitialState());

        void OnSingleFlashed(Vector2D flashPos)
        {
            flashes++;
        }

        octopusGrid.SingleFlashed += OnSingleFlashed;
        octopusGrid.Observe(steps);

        return flashes;
    }
    
    private async Task<int> WaitForAllFlashed()
    {
        var firstAllFlashedStep = 0;
        var cts = new CancellationTokenSource();
        var octopusGrid = new OctopusGrid(GetInitialState());

        void OnAllFlashed(int stepNumber)
        {
            firstAllFlashedStep = stepNumber;
            cts.Cancel();
        }

        octopusGrid.AllFlashed += OnAllFlashed;
        await octopusGrid.ObserveContinuously(cts.Token);

        return firstAllFlashedStep;
    }

    private Grid2D<int> GetInitialState()
    {
        var lines = GetInputLines();
        var width = lines[0].Length;
        var height = lines.Length;
        var grid = Grid2D<int>.WithDimensions(height, width);
        
        for (var x = 0; x < width; x++)
        for (var y = 0; y < height; y++)
        {
            grid[x, y] = lines[height - 1 - y][x] - '0';
        }
        
        return grid;
    }
}