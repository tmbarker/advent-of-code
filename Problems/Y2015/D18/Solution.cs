using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2015.D18;

/// <summary>
/// Like a GIF For Your Yard: https://adventofcode.com/2015/day/18
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => Simulate(cornersAlwaysOn: false),
            2 => Simulate(cornersAlwaysOn: true),
            _ => ProblemNotSolvedString
        };
    }

    private int Simulate(bool cornersAlwaysOn)
    {
        var input = GetInputLines();
        var rows = input.Length;
        var cols = input[0].Length;
        
        var aabb = new Aabb2D(
            min: new Vector2D(x: 0, y: 0),
            max: new Vector2D(x: cols - 1, y: rows - 1));
        
        var on = new HashSet<Vector2D>();
        var next = new HashSet<Vector2D>();

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[rows - y - 1][x] == '#')
            {
                on.Add(new Vector2D(x, y));
            }
        }

        if (cornersAlwaysOn)
        {
            SetCorners(on, rows, cols);
        }

        for (var i = 0; i < 100; i++)
        {
            foreach (var pos in aabb)
            {
                var onAdj = pos.GetAdjacentSet(Metric.Chebyshev).Count(on.Contains);
                var onSelf = on.Contains(pos);

                if ((onSelf && onAdj is 2 or 3) || (!onSelf && onAdj is 3))
                {
                    next.Add(pos);
                }
            }
            
            (on, next) = (next, on);
            next.Clear();
            
            if (cornersAlwaysOn)
            {
                SetCorners(on, rows, cols);
            }
        }
        
        return on.Count;
    }

    private static void SetCorners(ISet<Vector2D> on, int rows, int cols)
    {
        on.Add(new Vector2D(x: 0, y: 0));
        on.Add(new Vector2D(x: 0, y: rows - 1));
        on.Add(new Vector2D(x: cols - 1, y: rows - 1));
        on.Add(new Vector2D(x: cols - 1, y: 0));
    }
}