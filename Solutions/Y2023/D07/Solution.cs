using Utilities.Extensions;

namespace Solutions.Y2023.D07;

[PuzzleInfo("Camel Cards", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => ScoreHands(jokers: false),
            2 => ScoreHands(jokers: true),
            _ => PuzzleNotSolvedString
        };
    }

    private int ScoreHands(bool jokers)
    {
        Deck.HasJokers = jokers;
        return ParseInputLines(parseFunc: ParseHand)
            .Order()
            .Select((hand, index) => hand.Bid * (index + 1))
            .Sum();
    }

    private static Hand ParseHand(string line)
    {
        var elements = line.Split(separator: ' ');
        return new Hand(cards: elements[0], bid: elements[1].ParseInt());
    }
}