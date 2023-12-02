using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2023.D02;

/// <summary>
/// Cube Conundrum: https://adventofcode.com/2023/day/2
/// </summary>
public class Solution : SolutionBase
{
    private readonly record struct Game(int Id, List<Set> Draws);
    private readonly record struct Set(int Red, int Green, int Blue);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => SumPossible(),
            2 => SumMinimumPowers(),
            _ => ProblemNotSolvedString
        };
    }

    private int SumPossible()
    {
        var games = ParseInputLines(parseFunc: ParseGame);
        var constraint = new Set(Red: 12, Green: 13, Blue: 14);
        var possible = games.Where(game => IsPossible(constraint, game));

        return possible.Sum(game => game.Id);
    }
    
    private static bool IsPossible(Set constraint, Game game)
    {
        return game.Draws.All(draw => IsPossible(constraint, observed: draw));
    }

    private static bool IsPossible(Set constraint, Set observed)
    {
        return
            observed.Red <= constraint.Red &&
            observed.Blue <= constraint.Blue &&
            observed.Green <= constraint.Green;
    }
    
    private int SumMinimumPowers()
    {
        var games = ParseInputLines(parseFunc: ParseGame);
        var powers = games.Select(GetMinimumPower);

        return powers.Sum();
    }

    private static int GetMinimumPower(Game game)
    {
        var r = game.Draws.Max(draw => draw.Red);
        var g = game.Draws.Max(draw => draw.Green);
        var b = game.Draws.Max(draw => draw.Blue);

        return r * g * b;
    }
    
    private static Game ParseGame(string line)
    {
        var elements = line.Split(separator: ':');
        var id = elements[0].ParseInt();
        
        var setStrings = elements[1].Split(separator: ';');
        var setEntities = new List<Set>();

        foreach (var drawString in setStrings)
        {
            var r = Regex.Match(input: drawString, pattern: @"(\d+) red");
            var g = Regex.Match(input: drawString, pattern: @"(\d+) green");
            var b = Regex.Match(input: drawString, pattern: @"(\d+) blue");

            setEntities.Add(new Set(
                Red:   r.Success ? r.Groups[1].ParseInt() : 0,
                Blue:  b.Success ? b.Groups[1].ParseInt() : 0,
                Green: g.Success ? g.Groups[1].ParseInt() : 0));
        }

        return new Game(id, setEntities);
    }
}