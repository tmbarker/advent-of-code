using Problems.Common;
using Problems.Y2021.Common;

namespace Problems.Y2021.D04;

/// <summary>
/// Giant Squid: https://adventofcode.com/2021/day/4
/// </summary>
public class Solution : SolutionBase2021
{
    public override int Day => 4;
    
    public override string Run(int part)
    {
        AssertInputExists();
        BingoData.Parse(GetInput(), out var draw, out var cards);

        return part switch
        {
            0 => GetFirstWinningCardScore(draw!, cards!).ToString(),
            1 => GetLastWinningCardScore(draw!, cards!).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private static int GetFirstWinningCardScore(Queue<int> draw, IList<BingoCard> cards)
    {
        var numDraws = 0;
        while (draw.Count > 0)
        {
            numDraws++;

            var next = draw.Dequeue();
            // Console.WriteLine($"Draw #{numDraws}: number => {next}");
            foreach (var card in cards)
            {
                if (card.Evaluate(next, out var score))
                {
                    // NOTE: Uncomment if you want to print each card and see the marked squares
                    // card.Print();
                    Console.WriteLine($"First Win: Score => {score} [On draw #{numDraws} with '{next}']");
                    return score;
                }
                
                // NOTE: Uncomment if you want to print each card and see the marked squares
                // card.Print();
            }
        }

        throw new NoSolutionException();
    }
    
    private static int GetLastWinningCardScore(Queue<int> draw, ICollection<BingoCard> cards)
    {
        var numWins = 0;
        var numDraws = 0;
        
        while (draw.Count > 0)
        {
            numDraws++;
            var next = draw.Dequeue();
            //Console.WriteLine($"Draw #{numDraws}: number => {next}");
            
            foreach (var card in cards)
            {
                if (!card.Evaluate(next, out var score))
                {
                    continue;
                }
                
                numWins++;
                //Console.WriteLine($"Win #{numWins}: Score => {score} [On draw #{numDraws} with '{next}']");
                
                if (numWins >= cards.Count)
                {
                    return score;
                }
            }
        }

        throw new NoSolutionException();
    }

    private string[] GetInput()
    {
        return File.ReadAllLines(GetInputFilePath());
    }
}