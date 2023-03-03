﻿using Problems.Y2018.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2018.D03;

/// <summary>
/// No Matter How You Slice It: https://adventofcode.com/2018/day/3
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 3;
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var claims = ParseClaims(input);
        
        return part switch
        {
            1 => CountClaimOverlaps(claims.Select(c => c.Aabb)),
            2 => GetNonOverlappedClaim(claims.ToList()),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountClaimOverlaps(IEnumerable<Aabb2D> claimAabbs)
    {
        var map = new Dictionary<Vector2D, int>();
        
        foreach (var aabb in claimAabbs)
        foreach (var position in aabb)
        {
            map.EnsureContainsKey(position);
            map[position]++;
        }

        return map.WhereValues(c => c > 1).Count;
    }

    private static int GetNonOverlappedClaim(IList<(int Id, Aabb2D aabb2D)> claims)
    {
        return claims
            .Single(claim => claims.Count(other => claim.aabb2D.Intersects(other.aabb2D)) == 1).Id;
    }
    
    private static IEnumerable<(int Id, Aabb2D Aabb)> ParseClaims(IEnumerable<string> input)
    {
        return input.Select(ParseClaim);
    }

    private static (int Id, Aabb2D Aabb) ParseClaim(string line)
    {
        var numbers = line.Numbers();
        var id = numbers[0];
        var aabb = new Aabb2D(
            xMin: numbers[1],
            xMax: numbers[1] + numbers[3] - 1,
            yMin: numbers[2],
            yMax: numbers[2] + numbers[4] - 1);

        return (id, aabb);
    }
}