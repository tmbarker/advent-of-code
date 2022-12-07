using Problems.Y2022.Common;

namespace Problems.Y2022.D02;

/// <summary>
/// Rock Paper Scissors: https://adventofcode.com/2022/day/2
/// </summary>
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
        AssertInputExists();
        
        return part switch
        {
            0 => EvaluateStrategyGuide1().ToString(),
            1 => EvaluateStrategyGuide2().ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int EvaluateStrategyGuide1()
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

    private int EvaluateStrategyGuide2()
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