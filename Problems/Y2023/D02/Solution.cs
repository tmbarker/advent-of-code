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
        var constraint = new Set(Red: 12, Green: 13, Blue: 14);
        var games = ParseInputLines(parseFunc: ParseGame);
        var possible = games.Where(game => IsPossible(game, constraint));

        return possible.Sum(game => game.Id);
    }
    
    private static bool IsPossible(Game game, Set constraint)
    {
        return game.Draws.All(draw => IsPossible(observed: draw, constraint));
    }

    private static bool IsPossible(Set observed, Set constraint)
    {
        return 
            observed.Red   <= constraint.Red && 
            observed.Blue  <= constraint.Blue && 
            observed.Green <= constraint.Green;
    }
    
    private int SumMinimumPowers()
    {
        return ParseInputLines(parseFunc: ParseGame).Sum(GetMinimumPower);
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
        var draws = new List<Set>();

        foreach (var setString in elements[1].Split(separator: ';'))
        {
            draws.Add(item: new Set(
                Red:   Regex.Match(input: setString, pattern: @"(\d+) red").ParseSingleIntOrDefault(group: 1),
                Blue:  Regex.Match(input: setString, pattern: @"(\d+) blue").ParseSingleIntOrDefault(group: 1),
                Green: Regex.Match(input: setString, pattern: @"(\d+) green").ParseSingleIntOrDefault(group: 1)));
        }

        return new Game(id, draws);
    }
}