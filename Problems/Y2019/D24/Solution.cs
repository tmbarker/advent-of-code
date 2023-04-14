using Problems.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2019.D24;

/// <summary>
/// Planet of Discord: https://adventofcode.com/2019/day/24
/// </summary>
public class Solution : SolutionBase
{
    private const int Size = 5;
    
    private static readonly Vector2D CenterTile = new(2, 2);
    private static readonly Aabb2D TileAabb = new(
        extents: new[] { Vector2D.Zero, new(Size - 1, Size - 1) },
        inclusive: true);

    public override object Run(int part)
    {
        var input = GetInputLines();
        var initialBugs = ParseInitialBugs(input);

        return part switch
        {
            1 => GetFirstRepeatedRating(initialBugs, GridType.Static),
            2 => CountBugsAfterSteps(steps: 200, initialBugs, GridType.Recursive),
            _ => ProblemNotSolvedString
        };
    }

    private static long GetFirstRepeatedRating(HashSet<Vector3D> bugs, GridType gridType)
    {
        var ratings = new HashSet<long>();
        var rating = GetRating(bugs);

        while (ratings.Add(rating))
        {
            bugs = StepBugs(bugs, gridType);
            rating = GetRating(bugs);
        }

        return rating;
    }

    private static int CountBugsAfterSteps(int steps, HashSet<Vector3D> bugs, GridType gridType)
    {
        for (var i = 0; i < steps; i++)
        {
            bugs = StepBugs(bugs, gridType);
        }
    
        return bugs.Count;
    }

    private static HashSet<Vector3D> StepBugs(IReadOnlySet<Vector3D> bugs, GridType gridType)
    {
        var nextBugs = new HashSet<Vector3D>();
        var candidateTiles = GetCandidateTiles(bugs, gridType);
        
        foreach (var tile in candidateTiles)
        {
            var adjacentTiles = gridType == GridType.Static
                ? GetStaticAdjacencies(tile)
                : GetRecursiveAdjacencies(tile);
            
            var adjacentBugs = adjacentTiles.Count(bugs.Contains);
            var isBug = bugs.Contains(tile);

            if ((isBug && adjacentBugs is 1) || (!isBug && adjacentBugs is 1 or 2))
            {
                nextBugs.Add(tile);
            }
        }

        return nextBugs;
    }

    private static IEnumerable<Vector3D> GetCandidateTiles(IEnumerable<Vector3D> bugs, GridType gridType)
    {
        var depths = bugs
            .Select(v => v.Z)
            .ToHashSet();
        
        if (gridType == GridType.Static)
        {
            return TileAabb.Select(xy => new Vector3D(xy, z: depths.Single()));
        }

        var candidates = new List<Vector3D>();
        var tileXyPositions = TileAabb.Except(CenterTile);

        var min = depths.Min() - 1;
        var max = depths.Max() + 1;

        for (var depth = min; depth <= max; depth++)
        {
            candidates.AddRange(tileXyPositions.Select(xy => new Vector3D(xy, depth)));
        }

        return candidates;
    }

    private static IEnumerable<Vector3D> GetStaticAdjacencies(Vector3D tile)
    {
        yield return tile + Vector3D.Up;
        yield return tile + Vector3D.Down;
        yield return tile + Vector3D.Left;
        yield return tile + Vector3D.Right;
    }

    private static IEnumerable<Vector3D> GetRecursiveAdjacencies(Vector3D tile)
    {
        var adjacent = new List<Vector3D>();
        var depth = tile.Z;
        var xy = (Vector2D)tile;
        
        foreach (var direction in Vector2D.Zero.GetAdjacentSet(Metric.Taxicab))
        {
            // "Regular" (same depth) adjacencies
            //
            var target = xy + direction;
            if (TileAabb.Contains(target) && target != CenterTile)
            {
                adjacent.Add(new Vector3D(target, depth));
                continue;
            }

            // Encapsulating "outer" (depth -1) adjacencies
            //
            if (!TileAabb.Contains(target))
            {
                adjacent.Add(new Vector3D(CenterTile + direction, depth - 1));
                continue;
            }
            
            // Nested "inner" (depth + 1) adjacencies
            //
            const int radius = Size / 2;
            var fixedOffset = -radius * direction;
            var variableOffset = (Vector2D)(Rotation3D.Positive90Z * direction);

            for (var i = -radius; i <= radius; i++)
            {
                adjacent.Add(new Vector3D(
                    xy: CenterTile + fixedOffset + i * variableOffset,
                    z: depth + 1));
            }
        }
        
        return adjacent;
    }

    private static HashSet<Vector3D> ParseInitialBugs(IList<string> input)
    {
        var bugs = new HashSet<Vector3D>();
        
        for (var y = 0; y < Size; y++)
        for (var x = 0; x < Size; x++)
        {
            if (input[y][x] == '#')
            {
                bugs.Add(new Vector3D(
                    x: x,
                    y: y,
                    z: 0));
            }
        }

        return bugs;
    }
    
    private static long GetRating(IEnumerable<Vector3D> bugs)
    {
        return (long)bugs.Aggregate(0UL, (rating, pos) => rating | 1UL << (pos.X + pos.Y * Size));
    }
}