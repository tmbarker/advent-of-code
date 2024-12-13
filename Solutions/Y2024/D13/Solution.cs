using Utilities.Extensions;
using Utilities.Numerics;

namespace Solutions.Y2024.D13;

[PuzzleInfo("Claw Contraption", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Button(long X, long Y, int Cost)
    {
        public static Button Parse(string s, int cost)
        {
            var numbers = s.ParseLongs();
            return new Button(X: numbers[0], Y: numbers[1], cost);
        }
    }
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Part1(delta: 0L),
            2 => Part1(delta: 10000000000000L),
            _ => PuzzleNotSolvedString
        };
    }

    private long Part1(long delta)
    {
        var chunks = GetInputLines().ChunkByNonEmpty();
        var tokens = 0L;

        foreach (var chunk in chunks)
        {
            var a = Button.Parse(chunk[0], cost: 3);
            var b = Button.Parse(chunk[1], cost: 1);
            var p = chunk[2].ParseLongs();

            if (TryComputeMin(a, b, px: p[0] + delta, py: p[1] + delta, out var min))
            {
                tokens += min;
            }
        }
        
        return tokens;
    }
    
    private static bool TryComputeMin(Button a, Button b, long px, long py, out long min)
    {
        //  Let 'Na' and 'Nb' denote how many times each respective button is pressed. We simply need to find a
        //  solution to the following system:
        //    Na * a.x + Nb * b.x = px
        //    Na * a.Y + Nb * b.Y = py
        //
        var ns = LinearSolver.Solve(epsilon: double.Epsilon, a: new double[,]
        {
            { a.X, b.X, px },
            { a.Y, b.Y, py }
        });

        //  Next, verify that the solution is integral
        //
        var na = (long)Math.Round(ns[0]);
        var nb = (long)Math.Round(ns[1]);

        if (na * a.X + nb * b.X == px &&
            na * a.Y + nb * b.Y == py)
        {
            min = na * a.Cost + nb * b.Cost;
            return true;
        }
        
        min = 0;
        return false;
    }
}