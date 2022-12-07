namespace Problems.Y2022.D02;

public static class RockPaperScissorsHelper
{
    private static readonly Dictionary<RockPaperScissorsChoice, int> ChoiceScoreMap = new()
    {
        { RockPaperScissorsChoice.Rock, 1 },
        { RockPaperScissorsChoice.Paper, 2 },
        { RockPaperScissorsChoice.Scissors, 3 },
    };
    
    private static readonly Dictionary<RockPaperScissorsResult, int> ResultScoreMap = new()
    {
        { RockPaperScissorsResult.Loss, 0 },
        { RockPaperScissorsResult.Draw, 3 },
        { RockPaperScissorsResult.Win, 6 },
    };

    public static RockPaperScissorsResult Evaluate(RockPaperScissorsChoice choice, RockPaperScissorsChoice against)
    {
        if (choice == against)
        {
            return RockPaperScissorsResult.Draw;
        }

        return choice switch
        {
            RockPaperScissorsChoice.Rock => against == RockPaperScissorsChoice.Scissors
                ? RockPaperScissorsResult.Win
                : RockPaperScissorsResult.Loss,
            RockPaperScissorsChoice.Paper => against == RockPaperScissorsChoice.Rock
                ? RockPaperScissorsResult.Win
                : RockPaperScissorsResult.Loss,
            RockPaperScissorsChoice.Scissors => against == RockPaperScissorsChoice.Paper
                ? RockPaperScissorsResult.Win
                : RockPaperScissorsResult.Loss,
            _ => throw new ArgumentOutOfRangeException(nameof(choice), choice, null)
        };
    }

    public static RockPaperScissorsChoice Evaluate(RockPaperScissorsChoice against, RockPaperScissorsResult result)
    {
        if (result == RockPaperScissorsResult.Draw)
        {
            return against;
        }

        return against switch
        {
            RockPaperScissorsChoice.Rock => result == RockPaperScissorsResult.Win
                ? RockPaperScissorsChoice.Paper
                : RockPaperScissorsChoice.Scissors,
            RockPaperScissorsChoice.Paper => result == RockPaperScissorsResult.Win
                ? RockPaperScissorsChoice.Scissors
                : RockPaperScissorsChoice.Rock,
            RockPaperScissorsChoice.Scissors => result == RockPaperScissorsResult.Win
                ? RockPaperScissorsChoice.Rock
                : RockPaperScissorsChoice.Paper,
            _ => throw new ArgumentOutOfRangeException(nameof(against), against, null)
        };
    }

    public static int Score(RockPaperScissorsChoice choice)
    {
        return ChoiceScoreMap[choice];
    }
    
    public static int Score(RockPaperScissorsResult result)
    {
        return ResultScoreMap[result];
    }
}