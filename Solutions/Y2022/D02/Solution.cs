namespace Solutions.Y2022.D02;

[PuzzleInfo("Rock Paper Scissors", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, RockPaperScissorsChoice> StrategyGuideChoiceMap = new()
    {
        {'A', RockPaperScissorsChoice.Rock},
        {'B', RockPaperScissorsChoice.Paper},
        {'C', RockPaperScissorsChoice.Scissors},
        {'X', RockPaperScissorsChoice.Rock},
        {'Y', RockPaperScissorsChoice.Paper},
        {'Z', RockPaperScissorsChoice.Scissors}
    };

    private static readonly Dictionary<char, RockPaperScissorsResult> StrategyGuideResultMap = new()
    {
        {'X', RockPaperScissorsResult.Loss},
        {'Y', RockPaperScissorsResult.Draw},
        {'Z', RockPaperScissorsResult.Win}
    };

    public override object Run(int part)
    {
        return part switch
        {
            1 => EvaluateStrategyGuide1(),
            2 => EvaluateStrategyGuide2(),
            _ => ProblemNotSolvedString
        };
    }

    private int EvaluateStrategyGuide1()
    {
        var lines = GetInputLines();
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
        var lines = GetInputLines();
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