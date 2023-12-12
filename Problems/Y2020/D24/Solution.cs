using System.Text.RegularExpressions;
using Utilities.Geometry.Hexagonal;

namespace Problems.Y2020.D24;

using Instructions = IList<string>;
using Floor = HashSet<Hex>;

[PuzzleInfo("Lobby Layout", Topics.Vectors|Topics.Math, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
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
        var input = GetInputLines();
        var instructions = ParseInstructions(input);
        var floor = AssembleFloor(instructions);
        
        return part switch
        {
            1 => floor.Count,
            2 => Simulate(floor, days: 100).Count,
            _ => ProblemNotSolvedString
        };
    }

    private static Floor Simulate(Floor floor, int days)
    {
        for (var d = 0; d < days; d++)
        {
            var consider = new Floor();
            var next = new Floor();
            
            foreach (var tile in floor)
            {
                consider.Add(tile);
                foreach (var adj in tile.GetAdjacentSet())
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
                if (!floor.Contains(tile) && adjBlackTiles is 2)
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
        foreach (var sequence in instructions)
        {
            var tile = sequence.Aggregate(
                seed: Hex.Zero,
                func: (current, step) => current + Adjacencies[step]);

            if (!floor.Add(tile))
            {
                floor.Remove(tile);
            }
        }
        return floor;
    }

    private static IEnumerable<Instructions> ParseInstructions(IEnumerable<string> input)
    {
        var regex = new Regex(pattern: @"(e|se|sw|w|nw|ne)+");
        foreach (var line in input)
        {
            var match = regex.Match(line);
            var captures = match.Groups[1].Captures.Select(c => c.Value);
            yield return new List<string>(captures);
        }
    }
}