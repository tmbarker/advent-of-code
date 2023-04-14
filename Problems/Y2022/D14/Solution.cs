using Problems.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

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
        var rockPaths = ParseRockPathVertices(GetInputLines());
        var occupiedPositions = FormRockPositionsSet(rockPaths);
        
        return part switch
        {
            1 => CountSandWithAbyss(occupiedPositions),
            2 => CountSandWithFloor(occupiedPositions),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int CountSandWithAbyss(ISet<Vector2D> rockPositions)
    {
        var sandAtRestCount = 0;
        
        while (DriveSandToRestWithAbyss(rockPositions))
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

    private static bool DriveSandToRestWithAbyss(ISet<Vector2D> occupiedPositions)
    {
        var abyssThreshold = occupiedPositions.Max(v => v.Y);
        var sandPos = SandOrigin;
        
        while (sandPos.Y < abyssThreshold)
        {
            var sandMoved = false;
            
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var movement in FallVectors)
            {
                if (occupiedPositions.Contains(sandPos + movement))
                {
                    continue;
                }
                
                sandPos += movement;
                sandMoved = true;
                break;
            }

            if (sandMoved)
            {
                continue;
            }
            
            occupiedPositions.Add(sandPos);
            return true;
        }

        return false;
    }

    private static bool DriveSandToRestWithFloor(ISet<Vector2D> occupiedPositions, int floorHeight)
    {
        var sandPos = SandOrigin;
        
        while (!occupiedPositions.Contains(SandOrigin))
        {
            var sandMoved = false;
            
            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var movement in FallVectors)
            {
                var targetPos = sandPos + movement;
                if (occupiedPositions.Contains(targetPos) || targetPos.Y == floorHeight)
                {
                    continue;
                }

                sandPos += movement;
                sandMoved = true;
                break;
            }

            if (sandMoved)
            {
                continue;
            }

            occupiedPositions.Add(sandPos);
            return true;
        }

        return false;
    }
    
    private static ISet<Vector2D> FormRockPositionsSet(IEnumerable<IList<Vector2D>> rockStructurePaths)
    {
        var set = new HashSet<Vector2D>();
        
        foreach (var rockStructurePath in rockStructurePaths)
        {
            for (var i = 0; i < rockStructurePath.Count - 1; i++)
            {
                var currentVertex = rockStructurePath[i];
                var nextVertex = rockStructurePath[i + 1];
                
                var pathSectionDelta = Vector2D.Normalize(nextVertex - currentVertex);
                var currentPos = currentVertex;

                while (currentPos != nextVertex)
                {
                    set.Add(currentPos);
                    currentPos += pathSectionDelta;
                }
            }
            
            //  Need to include the final vertex
            //
            set.Add(rockStructurePath.Last());
        }
        
        return set;
    }

    private static IEnumerable<IList<Vector2D>> ParseRockPathVertices(IEnumerable<string> input)
    {
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (var line in input)
        {
            var vertices = line.Split(separator: "->", StringSplitOptions.TrimEntries);
            var vectors = vertices.Select(ParseVertex).ToList();
            
            yield return vectors;
        }
    }

    private static Vector2D ParseVertex(string vertex)
    {
        var numbers = vertex.ParseInts();
        return new Vector2D(x: numbers[0], y: numbers[1]);
    }
}