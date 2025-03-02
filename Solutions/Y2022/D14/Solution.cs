using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D14;

[PuzzleInfo("Regolith Reservoir", Topics.Vectors|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int FloorDelta = 2;
    private static readonly Vec2D SandOrigin = new(X: 500, Y: 0);
    private static readonly HashSet<Vec2D> FallVectors =
    [
        new Vec2D(X:  0, Y: 1),
        new Vec2D(X: -1, Y: 1),
        new Vec2D(X:  1, Y: 1)
    ];

    public override object Run(int part)
    {
        var input = GetInputLines();
        var rocks = FormRockPositionsSet(input);
        
        return part switch
        {
            1 => CountSandWithAbyss(rocks),
            2 => CountSandWithFloor(rocks),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int CountSandWithAbyss(ISet<Vec2D> rockPositions)
    {
        var sandAtRestCount = 0;
        var abyssThreshold = rockPositions.Max(v => v.Y);
        
        while (DriveSandToRestWithAbyss(rockPositions, abyssThreshold))
        {
            sandAtRestCount++;
        }
        
        return sandAtRestCount;
    }
    
    private static int CountSandWithFloor(ISet<Vec2D> rockPositions)
    {
        var sandAtRestCount = 0;
        var floorHeight = rockPositions.Max(v => v.Y) + FloorDelta;
        
        while (DriveSandToRestWithFloor(rockPositions, floorHeight))
        {
            sandAtRestCount++;
        }

        return sandAtRestCount;
    }

    private static bool DriveSandToRestWithAbyss(ISet<Vec2D> occupiedPositions, int abyssThreshold)
    {
        var sandPos = SandOrigin;
        while (sandPos.Y < abyssThreshold)
        {
            var moved = false;
            foreach (var movement in FallVectors)
            {
                if (!occupiedPositions.Contains(sandPos + movement))
                {
                    sandPos += movement;
                    moved = true;
                    break;
                }
            }

            if (!moved)
            {
                occupiedPositions.Add(sandPos);
                return true;
            }
        }

        return false;
    }

    private static bool DriveSandToRestWithFloor(ISet<Vec2D> occupiedPositions, int floorHeight)
    {
        var sandPos = SandOrigin;
        while (!occupiedPositions.Contains(SandOrigin))
        {
            var moved = false;
            foreach (var movement in FallVectors)
            {
                var targetPos = sandPos + movement;
                if (!occupiedPositions.Contains(targetPos) && targetPos.Y != floorHeight)
                {
                    sandPos += movement;
                    moved = true;
                    break;
                }
            }

            if (!moved)
            {
                occupiedPositions.Add(sandPos);
                return true;
            }
        }

        return false;
    }
    
    private static HashSet<Vec2D> FormRockPositionsSet(IEnumerable<string> input)
    {
        var set = new HashSet<Vec2D>();
        foreach (var line in input)
        {
            var numbers = line.ParseInts();
            var vertices = numbers.Length / 2;

            for (var i = 1; i < vertices; i++)
            {
                var from = new Vec2D(
                    X: numbers[2 * (i - 1)],
                    Y: numbers[2 * (i - 1) + 1]);
                var to = new Vec2D(
                    X: numbers[2 * i],
                    Y: numbers[2 * i + 1]);

                var pos = from;
                var dir = Vec2D.Normalize(to - from);

                while (pos != to + dir)
                {
                    set.Add(pos);
                    pos += dir;
                }
            }
        }
        return set;
    }
}