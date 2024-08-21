using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D24;

[PuzzleInfo("Planet of Discord", Topics.Vectors|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private const int Size = 5;

    private static readonly Vec2D CenterTile = new(x: 2, y: 2);
    private static readonly Aabb2D TileAabb = new(
        xMin: 0, 
        yMin: 0, 
        xMax: Size - 1, 
        yMax: Size - 1);

    public override object Run(int part)
    {
        var input = GetInputLines();
        var initialBugs = ParseInitialBugs(input);

        return part switch
        {
            1 => GetFirstRepeatedRating(initialBugs, GridType.Static),
            2 => CountBugsAfterSteps(steps: 200, initialBugs, GridType.Recursive),
            _ => PuzzleNotSolvedString
        };
    }

    private static long GetFirstRepeatedRating(HashSet<Vec3D> bugs, GridType gridType)
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

    private static int CountBugsAfterSteps(int steps, HashSet<Vec3D> bugs, GridType gridType)
    {
        for (var i = 0; i < steps; i++)
        {
            bugs = StepBugs(bugs, gridType);
        }
    
        return bugs.Count;
    }

    private static HashSet<Vec3D> StepBugs(IReadOnlySet<Vec3D> bugs, GridType gridType)
    {
        var nextBugs = new HashSet<Vec3D>();
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

    private static IEnumerable<Vec3D> GetCandidateTiles(IEnumerable<Vec3D> bugs, GridType gridType)
    {
        var depths = bugs
            .Select(v => v.Z)
            .ToHashSet();
        
        if (gridType == GridType.Static)
        {
            return TileAabb.Select(xy => new Vec3D(xy, z: depths.Single()));
        }

        var candidates = new List<Vec3D>();
        var tileXyPositions = TileAabb.Except(CenterTile);

        var min = depths.Min() - 1;
        var max = depths.Max() + 1;

        for (var depth = min; depth <= max; depth++)
        {
            candidates.AddRange(tileXyPositions.Select(xy => new Vec3D(xy, depth)));
        }

        return candidates;
    }

    private static IEnumerable<Vec3D> GetStaticAdjacencies(Vec3D tile)
    {
        yield return tile + Vec3D.Up;
        yield return tile + Vec3D.Down;
        yield return tile + Vec3D.Left;
        yield return tile + Vec3D.Right;
    }

    private static IEnumerable<Vec3D> GetRecursiveAdjacencies(Vec3D tile)
    {
        var adjacent = new List<Vec3D>();
        var depth = tile.Z;
        var xy = (Vec2D)tile;
        
        foreach (var direction in Vec2D.Zero.GetAdjacentSet(Metric.Taxicab))
        {
            //  "Regular" (same depth) adjacencies
            //
            var target = xy + direction;
            if (TileAabb.Contains(target) && target != CenterTile)
            {
                adjacent.Add(new Vec3D(target, depth));
                continue;
            }

            //  Encapsulating "outer" (depth -1) adjacencies
            //
            if (!TileAabb.Contains(target))
            {
                adjacent.Add(new Vec3D(CenterTile + direction, depth - 1));
                continue;
            }
            
            //  Nested "inner" (depth + 1) adjacencies
            //
            const int radius = Size / 2;
            var fixedOffset = -radius * direction;
            var variableOffset = (Vec2D)(Rot3D.P90Z * direction);

            for (var i = -radius; i <= radius; i++)
            {
                adjacent.Add(new Vec3D(
                    xy: CenterTile + fixedOffset + i * variableOffset,
                    z: depth + 1));
            }
        }
        
        return adjacent;
    }

    private static HashSet<Vec3D> ParseInitialBugs(IList<string> input)
    {
        var bugs = new HashSet<Vec3D>();
        
        for (var y = 0; y < Size; y++)
        for (var x = 0; x < Size; x++)
        {
            if (input[y][x] == '#')
            {
                bugs.Add(new Vec3D(
                    x: x,
                    y: y,
                    z: 0));
            }
        }

        return bugs;
    }
    
    private static long GetRating(IEnumerable<Vec3D> bugs)
    {
        return (long)bugs.Aggregate(0UL, (rating, pos) => rating | 1UL << (pos.X + pos.Y * Size));
    }
}