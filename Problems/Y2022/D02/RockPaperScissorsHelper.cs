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
        if ((int)choice == ((int)against + 1) % 3)
        {
            return RockPaperScissorsResult.Win;
        }

        return choice == against ? 
            RockPaperScissorsResult.Draw : 
            RockPaperScissorsResult.Loss;
    }

    public static RockPaperScissorsChoice Evaluate(RockPaperScissorsChoice against, RockPaperScissorsResult result)
    {
        if (result == RockPaperScissorsResult.Draw)
        {
            return against;
        }

        return result == RockPaperScissorsResult.Win ? 
            (RockPaperScissorsChoice)(((int)against + 1) % 3) :
            (RockPaperScissorsChoice)(((int)against + 2) % 3);
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