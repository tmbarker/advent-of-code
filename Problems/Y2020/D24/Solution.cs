using System.Text.RegularExpressions;
using Problems.Y2020.Common;
using Utilities.Cartesian;

namespace Problems.Y2020.D24;

using Instructions = IList<string>;
using Floor = HashSet<Vector2D>;

/// <summary>
/// Lobby Layout: https://adventofcode.com/2020/day/24
/// </summary>
public class Solution : SolutionBase2020
{
    private const int Days = 100; 
    private static readonly Dictionary<string, Vector2D> TileAdjacencies = new()
    {
        { "e",  Vector2D.Right + Vector2D.Right },
        { "w",  Vector2D.Left + Vector2D.Left },
        { "ne", Vector2D.Up + Vector2D.Right },
        { "nw", Vector2D.Up + Vector2D.Left },
        { "se", Vector2D.Down + Vector2D.Right },
        { "sw", Vector2D.Down + Vector2D.Left },
    };

    public override int Day => 24;
    
    public override object Run(int part)
    {
        var instructions = ParseInstructions(GetInputLines());
        var floor = AssembleFloor(instructions);
        
        return part switch
        {
            1 => floor.Count,
            2 => Simulate(floor, Days).Count,
            _ => ProblemNotSolvedString
        };
    }

    private static Floor Simulate(Floor floor, int days)
    {
        for (var d = 0; d < days; d++)
        {
            var consider = new HashSet<Vector2D>();
            var next = new Floor();
            
            foreach (var blackTile in floor)
            {
                consider.Add(blackTile);
                foreach (var adj in GetAdjSet(blackTile))
                {
                    consider.Add(adj);
                }
            }

            foreach (var tile in consider)
            {
                var adjBlackTiles = GetAdjSet(tile).Count(floor.Contains);
                if (floor.Contains(tile) && adjBlackTiles is 1 or 2)
                {
                    next.Add(tile);
                }
                else if (!floor.Contains(tile) && adjBlackTiles is 2)
                {
                    next.Add(tile);
                }
            }

            floor = next;
        }

        return floor;
    }

    private static Floor AssembleFloor(IEnumerable<Instructions> instructions)
    {
        var floor = new Floor();
        foreach (var instructionSet in instructions)
        {
            var tile = instructionSet.Aggregate(
                seed: Vector2D.Zero,
                func: (current, instr) => current + TileAdjacencies[instr]);

            if (!floor.Add(tile))
            {
                floor.Remove(tile);
            }
        }
        return floor;
    }

    private static IEnumerable<Vector2D> GetAdjSet(Vector2D tile)
    {
        return TileAdjacencies.Values.Select(v => tile + v).ToHashSet();
    }
    
    private static IEnumerable<Instructions> ParseInstructions(IEnumerable<string> input)
    {
        var regex = new Regex(@"(e|se|sw|w|nw|ne)+");
        foreach (var line in input)
        {
            var match = regex.Match(line);
            var captures = match.Groups[1].Captures.Select(c => c.Value);
            yield return new List<string>(captures);
        }
    }
}