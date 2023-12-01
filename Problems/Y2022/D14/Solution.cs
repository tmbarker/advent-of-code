using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D14;

/// <summary>
/// Regolith Reservoir: https://adventofcode.com/2022/day/14
/// </summary>
public class Solution : SolutionBase
{
    private const int FloorDelta = 2;
    private static readonly Vector2D SandOrigin = new(x: 500, y: 0);
    private static readonly HashSet<Vector2D> FallVectors = new()
    {
        new Vector2D(x:  0, y: 1),
        new Vector2D(x: -1, y: 1),
        new Vector2D(x:  1, y: 1)
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var rocks = FormRockPositionsSet(input);
        
        return part switch
        {
            1 => CountSandWithAbyss(rocks),
            2 => CountSandWithFloor(rocks),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int CountSandWithAbyss(ISet<Vector2D> rockPositions)
    {
        var sandAtRestCount = 0;
        var abyssThreshold = rockPositions.Max(v => v.Y);
        
        while (DriveSandToRestWithAbyss(rockPositions, abyssThreshold))
        {
            sandAtRestCount++;
        }
        
        return sandAtRestCount;
    }
    
    private static int CountSandWithFloor(ISet<Vector2D> rockPositions)
    {
        var sandAtRestCount = 0;
        var floorHeight = rockPositions.Max(v => v.Y) + FloorDelta;
        
        while (DriveSandToRestWithFloor(rockPositions, floorHeight))
        {
            sandAtRestCount++;
        }

        return sandAtRestCount;
    }

    private static bool DriveSandToRestWithAbyss(ISet<Vector2D> occupiedPositions, int abyssThreshold)
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

    private static bool DriveSandToRestWithFloor(ISet<Vector2D> occupiedPositions, int floorHeight)
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
    
    private static ISet<Vector2D> FormRockPositionsSet(IEnumerable<string> input)
    {
        var set = new HashSet<Vector2D>();
        foreach (var line in input)
        {
            var numbers = line.ParseInts();
            var vertices = numbers.Count / 2;

            for (var i = 1; i < vertices; i++)
            {
                var from = new Vector2D(
                    x: numbers[2 * (i - 1)],
                    y: numbers[2 * (i - 1) + 1]);
                var to = new Vector2D(
                    x: numbers[2 * i],
                    y: numbers[2 * i + 1]);

                var pos = from;
                var dir = Vector2D.Normalize(to - from);

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