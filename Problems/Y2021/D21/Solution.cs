using System.Text.RegularExpressions;
using Problems.Y2021.Common;

namespace Problems.Y2021.D21;

/// <summary>
/// Dirac Dice: https://adventofcode.com/2021/day/21
/// </summary>
public class Solution : SolutionBase2021
{
    private const string InputRegex = @": (\d+)";
    private const int BoardPlaces = 10;
    private const int DiceSides = 100;
    private const int WinningScore = 1000;
    
    public override int Day => 21;
    
    public override object Run(int part)
    {
        var initialPositions = ParseInitialPositions(GetInputLines());
        var die = new DeterministicDie(DiceSides);
        var players = (new Player(initialPositions.P1), new Player(initialPositions.P2));
        
        return part switch
        {
            0 => SimulateGame(players, die),
            _ => ProblemNotSolvedString,
        };
    }
    
    private static int SimulateGame((Player P1, Player P2) players, DeterministicDie die)
    {
        while (true)
        {
            if (TakeTurn(players.P1, die))
            {
                break;
            }
            
            if (TakeTurn(players.P2, die))
            {
                break;
            }
        }
        
        return die.NumRolls * Math.Min(players.P1.Score, players.P2.Score);
    }
    
    private static bool TakeTurn(Player player, DeterministicDie die)
    {
        player.Position = (player.Position + die.Roll() + die.Roll() + die.Roll() - 1) % BoardPlaces + 1;
        player.Score += player.Position;
        
        return player.Score >= WinningScore;
    }

    private static (int P1, int P2) ParseInitialPositions(IList<string> input)
    {
        var p1 = int.Parse(Regex.Match(input[0], InputRegex).Groups[1].Value);
        var p2 = int.Parse(Regex.Match(input[1], InputRegex).Groups[1].Value);

        return (p1, p2);
    }
}