using Problems.Y2022.Common;

namespace Problems.Y2022.D02;

public class Solution : SolutionBase2022
{
    private static readonly Dictionary<char, RockPaperScissorsChoice> StrategyGuideChoiceMap = new()
    {
        {'A', RockPaperScissorsChoice.Rock},
        {'B', RockPaperScissorsChoice.Paper},
        {'C', RockPaperScissorsChoice.Scissors},
        {'X', RockPaperScissorsChoice.Rock},
        {'Y', RockPaperScissorsChoice.Paper},
        {'Z', RockPaperScissorsChoice.Scissors},
    };

    private static readonly Dictionary<char, RockPaperScissorsResult> StrategyGuideResultMap = new()
    {
        {'X', RockPaperScissorsResult.Loss},
        {'Y', RockPaperScissorsResult.Draw},
        {'Z', RockPaperScissorsResult.Win},
    };

    public override int Day => 2;
    public override int Parts => 2;
    
    public override string Run(int part)
    {
        return part switch
        {
            0 => SolvePart1().ToString(),
            1 => SolvePart2().ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int SolvePart1()
    {
        var lines = File.ReadAllLines(GetInputFilePath());
        var score = 0;

        foreach (var line in lines)
        {
            var choice = StrategyGuideChoiceMap[line.Last()];
            var against = StrategyGuideChoiceMap[line.First()];
            var result = RockPaperScissorsHelper.Evaluate(choice, against);

            score += RockPaperScissorsHelper.Score(choice);
            score += RockPaperScissorsHelper.Score(result);
        }

        return score;
    }

    private int SolvePart2()
    {
        var lines = File.ReadAllLines(GetInputFilePath());
        var score = 0;

        foreach (var line in lines)
        {
            var against = StrategyGuideChoiceMap[line.First()];
            var result = StrategyGuideResultMap[line.Last()];
            var choice = RockPaperScissorsHelper.Evaluate(against, result);

            score += RockPaperScissorsHelper.Score(choice);
            score += RockPaperScissorsHelper.Score(result);
        }

        return score;
    }
}