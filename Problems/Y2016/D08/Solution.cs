using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2016.D08;

[PuzzleInfo("Two-Factor Authentication", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int Rows = 6;
    private const int Cols = 50;

    public override object Run(int part)
    {
        var operations = GetInputLines();
        var screen = Simulate(operations);
        
        return part switch
        {
            1 => CountOn(screen),
            2 => BuildRepresentativeString(screen),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountOn(Grid2D<bool> screen)
    {
        return screen.Count(pixel => pixel.Value);
    }

    private static string BuildRepresentativeString(Grid2D<bool> screen)
    {
        return screen.BuildRepresentativeString(prepend: "\n", elementFormatter: (_, on) => on ? "#" : ".");
    }

    private static Grid2D<bool> Simulate(IEnumerable<string> operations)
    {
        var screen = Grid2D<bool>.WithDimensions(Rows, Cols, Origin.Uv);
        foreach (var op in operations)
        {
            var args = op.ParseInts();
            switch (op)
            {
                case not null when op.Contains("rec"):
                    RectOn(screen, cols: args[0], rows: args[1]);
                    break;
                case not null when op.Contains("row"):
                    RotateRow(screen, row: args[0], amount: args[1]);
                    break;
                case not null when op.Contains("col"):
                    RotateCol(screen, col: args[0], amount: args[1]);
                    break;
            }
        }
        return screen;
    }

    private static void RectOn(Grid2D<bool> screen, int cols, int rows)
    {
        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            screen[x, y] = true;
        }
    }
    
    private static void RotateCol(Grid2D<bool> screen, int col, int amount)
    {
        var state = new bool[Rows];
        for (var y = 0; y < Rows; y++)
        {
            state[(y + amount) % Rows] = screen[col, y];
        }
        for (var y = 0; y < Rows; y++)
        {
            screen[col, y] = state[y];
        }
    }
    
    private static void RotateRow(Grid2D<bool> screen, int row, int amount)
    {
        var state = new bool[Cols];
        for (var x = 0; x < Cols; x++)
        {
            state[(x + amount) % Cols] = screen[x, row];
        }
        for (var x = 0; x < Cols; x++)
        {
            screen[x, row] = state[x];
        }
    }
}