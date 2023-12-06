using Problems.Common;

namespace Problems.Y2021.D04;

/// <summary>
/// Giant Squid: https://adventofcode.com/2021/day/4
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        BingoData.Parse(GetInputLines(), out var draw, out var cards);
        return part switch
        {
            1 => GetFirstWinningCardScore(draw!, cards!),
            2 => GetLastWinningCardScore(draw!, cards!),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetFirstWinningCardScore(Queue<int> draw, IList<BingoCard> cards)
    {
        while (draw.Count > 0)
        {
            var nextNumber = draw.Dequeue();
            foreach (var card in cards)
            {
                if (card.Evaluate(nextNumber, out var score))
                {
                    return score;
                }
            }
        }

        throw new NoSolutionException();
    }
    
    private static int GetLastWinningCardScore(Queue<int> draw, ICollection<BingoCard> cards)
    {
        var numWins = 0;
        while (draw.Count > 0)
        {
            var nextNumber = draw.Dequeue();
            foreach (var card in cards)
            {
                if (card.HasWon || !card.Evaluate(nextNumber, out var score))
                {
                    continue;
                }
                
                numWins++;

                if (numWins >= cards.Count)
                {
                    return score;
                }
            }
        }

        throw new NoSolutionException();
    }
}