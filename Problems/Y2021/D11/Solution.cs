using Problems.Common;
using Utilities.Geometry;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D11;

/// <summary>
/// Dumbo Octopus: https://adventofcode.com/2021/day/11
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountFlashes(steps: 100),
            2 => WaitForAllFlashed().Result, 
            _ => ProblemNotSolvedString
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
        return Grid2D<int>.MapChars(GetInputLines(), c => c - '0');
    }
}