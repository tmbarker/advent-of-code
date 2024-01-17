using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D03;

/// <summary>
/// No Matter How You Slice It: https://adventofcode.com/2018/day/3
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var claims = ParseInputLines(parseFunc: ParseClaim);
        return part switch
        {
            1 => CountClaimOverlaps(claims.Select(c => c.Aabb)),
            2 => GetNonOverlappedClaim(claims.ToList()),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountClaimOverlaps(IEnumerable<Aabb2D> claimAabbs)
    {
        var map = new DefaultDict<Vec2D, int>(defaultValue: 0);
        
        foreach (var aabb in claimAabbs)
        foreach (var position in aabb)
        {
            map[position]++;
        }

        return map.Keys.Count(c => map[c] > 1);
    }

    private static int GetNonOverlappedClaim(IList<(int Id, Aabb2D aabb2D)> claims)
    {
        return claims
            .Single(claim => claims.Count(other => Aabb2D.Overlap(a: claim.aabb2D, b: other.aabb2D, out _)) == 1).Id;
    }

    private static (int Id, Aabb2D Aabb) ParseClaim(string line)
    {
        var numbers = line.ParseInts();
        var id = numbers[0];
        var aabb = new Aabb2D(
            xMin: numbers[1],
            xMax: numbers[1] + numbers[3] - 1,
            yMin: numbers[2],
            yMax: numbers[2] + numbers[4] - 1);

        return (id, aabb);
    }
}