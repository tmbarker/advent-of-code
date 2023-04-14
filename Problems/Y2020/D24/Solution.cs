using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Hexagonal;

namespace Problems.Y2020.D24;

using Instructions = IList<string>;
using Floor = HashSet<Hex>;

/// <summary>
/// Lobby Layout: https://adventofcode.com/2020/day/24
/// </summary>
public class Solution : SolutionBase
{
    private const int Days = 100;
    private static readonly Dictionary<string, Hex> Adjacencies = new()
    {
        { "e",  Hex.Directions[Pointy.E]  },
        { "w",  Hex.Directions[Pointy.W]  },
        { "ne", Hex.Directions[Pointy.Ne] },
        { "nw", Hex.Directions[Pointy.Nw] },
        { "se", Hex.Directions[Pointy.Se] },
        { "sw", Hex.Directions[Pointy.Sw] }
    };

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
            var consider = new HashSet<Hex>();
            var next = new Floor();
            
            foreach (var blackTile in floor)
            {
                consider.Add(blackTile);
                foreach (var adj in blackTile.GetAdjacentSet())
                {
                    consider.Add(adj);
                }
            }

            foreach (var tile in consider)
            {
                var adjBlackTiles = tile
                    .GetAdjacentSet()
                    .Count(floor.Contains);
                
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
                seed: Hex.Zero,
                func: (current, instr) => current + Adjacencies[instr]);

            if (!floor.Add(tile))
            {
                floor.Remove(tile);
            }
        }
        return floor;
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