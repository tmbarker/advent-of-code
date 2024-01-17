using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D17;

[PuzzleInfo("Reservoir Research", Topics.Vectors, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Vec2D Gravity = new(x: 0, y: 1);
    private static readonly Vec2D[] Sides = [Vec2D.Left, Vec2D.Right];

    public override object Run(int part)
    {
        var lines = GetInputLines();
        var reservoir = Reservoir.Parse(lines);

        return part switch
        {
            1 => Flood(reservoir, print: LogsEnabled, materials: [Water.Flowing, Water.Settled]),
            2 => Flood(reservoir, print: LogsEnabled, materials: [Water.Settled]),
            _ => ProblemNotSolvedString
        };
    }

    private static int Flood(Reservoir reservoir, bool print, params char[] materials)
    {
        var outflowAt = Reservoir.SpringPos + Gravity;
        var queue = new Queue<Vec2D>(collection: [outflowAt]);
        
        while (queue.Count != 0)
        {
            var pos = queue.Dequeue();
            var below = pos + Gravity;

            if (pos.Y > reservoir.MaxDepth)
            {
                continue;
            }

            switch (reservoir[below])
            {
                // Water flowing into empty space
                //
                case Terrain.Empty:
                    reservoir[pos] = Water.Flowing;
                    queue.Enqueue(pos + Gravity);
                    continue;
                // Water flowing into an already overflowing concavity
                //
                case Water.Flowing:
                    reservoir[pos] = Water.Flowing;
                    continue;
                // Water flowing onto a concavity floor, or already settled water 
                //
                case Terrain.Clay:
                case Water.Settled:
                {
                    if (TrySettle(reservoir, pos, out var overflows))
                    {
                        queue.Enqueue(pos - Gravity);
                        continue;
                    }

                    foreach (var overflow in overflows)
                    {
                        queue.Enqueue(overflow);
                    }
                    continue;
                }
            }
        }

        if (print)
        {
            reservoir.Print();
        }
        
        return reservoir.GetMaterialsCount(materials);
    }

    private static bool TrySettle(Reservoir reservoir, Vec2D searchFrom, out HashSet<Vec2D> overflows)
    {
        overflows = [];
        var canSettle = true;
        var visited = new HashSet<Vec2D>(collection: [searchFrom]);
        var queue = new Queue<Vec2D>(collection: [searchFrom]);

        while (queue.Count != 0)
        {
            var pos = queue.Dequeue();
            var below = pos + Gravity;

            if (reservoir[below] == Terrain.Empty)
            {
                canSettle = false;
                overflows.Add(pos);
                continue;
            }

            foreach (var direction in Sides)
            {
                var adjacent = pos + direction;
                var content = reservoir[adjacent];
                
                if (content is Terrain.Empty or Water.Flowing && visited.Add(adjacent))
                {
                    queue.Enqueue(adjacent);
                }
            }
        }

        foreach (var pos in visited)
        {
            reservoir[pos] = canSettle ? Water.Settled : Water.Flowing;
        }

        return canSettle;
    }
}