using Problems.Y2020.Common;

namespace Problems.Y2020.D22;

using Memo = HashSet<(string P1State, string P2State)>;

/// <summary>
/// Crab Combat: https://adventofcode.com/2020/day/22
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 22;
    
    public override object Run(int part)
    {
        var decks = ParseDecks(GetInputLines());
        return part switch
        {
            0 => PlaySimpleGame(decks.P1, decks.P2),
            1 => PlayRecursiveGame(decks.P1, decks.P2).WinningScore,
            _ => ProblemNotSolvedString,
        };
    }

    private static int PlaySimpleGame(Deck p1, Deck p2)
    {
        while (p1.HasCards && p2.HasCards)
        {
            var (d1, d2) = (p1.DrawFromTop(), p2.DrawFromTop());
            if (d1 > d2)
            {
                p1.AddToBottom(d1);
                p1.AddToBottom(d2);
            }
            else
            {
                p2.AddToBottom(d2);
                p2.AddToBottom(d1);
            }
        }

        return p1.HasCards ? p1.Score() : p2.Score();
    }

    private static (bool P1Win, int WinningScore) PlayRecursiveGame(Deck p1, Deck p2)
    {
        var memo = new Memo();

        while (p1.HasCards && p2.HasCards)
        {
            if (!memo.Add((p1.State, p2.State)))
            {
                return (true, p1.Score());
            }

            var (d1, d2) = (p1.DrawFromTop(), p2.DrawFromTop());
            bool p1Win;
            
            if (p1.CardsRemaining >= d1 && p2.CardsRemaining >= d2)
            {
                (p1Win, _) =  PlayRecursiveGame(p1.Copy(d1), p2.Copy(d2));
            }
            else
            {
                p1Win = d1 > d2;
            }
            
            var wp = p1Win ? p1 : p2;
            wp.AddToBottom(p1Win ? d1 : d2);
            wp.AddToBottom(p1Win ? d2 : d1);
        }

        return (p1.HasCards, p1.HasCards ? p1.Score() : p2.Score());
    }
    
    private static (Deck P1, Deck P2) ParseDecks(IList<string> input)
    {
        var p2Idx = input.IndexOf("Player 2:");
        var p1 = new Deck(input.Skip(1).Take(p2Idx - 2).Select(int.Parse));
        var p2 = new Deck(input.Skip(p2Idx + 1).Select(int.Parse));

        return (p1, p2);
    }
}