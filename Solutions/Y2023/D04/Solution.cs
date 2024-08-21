using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2023.D04;

[PuzzleInfo("Scratchcards", Topics.None, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Card(int Wins);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountScore(),
            2 => CountCards(),
            _ => PuzzleNotSolvedString
        };
    }

    private int CountScore()
    {
        return ParseInputLines(parseFunc: ParseCard).Sum(card => (int)Math.Pow(2, card.Wins - 1));
    }

    private int CountCards()
    {
        var cards = ParseInputLines(parseFunc: ParseCard).ToArray();
        var counts = Enumerable.Repeat(element: 1, count: cards.Length).ToArray();

        for (var n = 0; n < cards.Length; n++)
        for (var w = 1; w <= cards[n].Wins; w++)
        {
            counts[n + w] += counts[n];
        }

        return counts.Sum();
    }
    
    private static Card ParseCard(string line)
    {
        var match = Regex.Match(input: line, pattern: @"Card\s+(?:\d+):(?:\s+(?<Wins>\d+))+\s\|(?:\s+(?<Have>\d+))+");
        var wins = match.Groups["Wins"].ParseInts();
        var have = match.Groups["Have"].ParseInts();

        return new Card(Wins: have.Count(wins.Contains));
    }
}