using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D08;

[PuzzleInfo("Resonant Collinearity", Topics.Vectors, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var map = GetInputGrid();
        var antennas = map
            .Where(pos => map[pos] != '.')
            .GroupBy(pos => map[pos])
            .Select(group => group.ToArray());
        
        return part switch
        {
             1 => CountSimple(map, antennas),
             2 => CountResonant(map, antennas),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountSimple(Grid2D<char> map, IEnumerable<Vec2D[]> antennas)
    {
        var antinodes = new HashSet<Vec2D>();
        
        foreach (var set in antennas)
        foreach (var pos in set)
        foreach (var other in set)
        {
            if (pos != other) antinodes.Add(pos + pos - other);
        }

        return antinodes.Count(map.Contains);
    }
    
    private static int CountResonant(Grid2D<char> map, IEnumerable<Vec2D[]> antennas)
    {
        var antinodes = new HashSet<Vec2D>();
        
        foreach (var set in antennas)
        foreach (var pos in set)
        foreach (var other in set)
        {
            if (pos == other) continue;

            var freq = pos - other;
            var node = pos;

            while (map.Contains(node))
            {
                antinodes.Add(node);
                node += freq;
            }
        }

        return antinodes.Count;
    }
}