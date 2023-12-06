using Problems.Common;

namespace Problems.Y2018.D15;

/// <summary>
/// Beverage Bandits: https://adventofcode.com/2018/day/15
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        return part switch
        {
            1 => PlayDefaultGame(input, print: LogsEnabled),
            2 => PlayBuffedGame(input, print: LogsEnabled),
            _ => ProblemNotSolvedString
        };
    }

    private static int PlayDefaultGame(IList<string> input, bool print)
    {
        var emptyBuffs = new Dictionary<char, int>();
        var state = GameState.Create(input, teamDmgBuffs: emptyBuffs);
        var result = Sim.Run(state, print);

        return result.Score;
    }

    private static int PlayBuffedGame(IList<string> input, bool print)
    {
        var score = 0;
        var dmgBuff = 0;
        var desiredResult = false;

        while (!desiredResult)
        {
            var buffs = new Dictionary<char, int> { { GameData.Elf, dmgBuff++ } };
            var state = GameState.Create(input, buffs);
            var result = Sim.Run(state, print);

            score = result.Score;
            desiredResult = 
                result.WinningTeam == GameData.Elf && 
                result.Casualties[GameData.Elf] == 0;
        }

        return score;
    }
}