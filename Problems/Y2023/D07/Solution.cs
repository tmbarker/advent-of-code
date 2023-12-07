using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2023.D07;

/// <summary>
/// Camel Cards: https://adventofcode.com/2023/day/7
/// </summary>
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => ScoreHands(jokers: false),
            2 => ScoreHands(jokers: true),
            _ => ProblemNotSolvedString
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