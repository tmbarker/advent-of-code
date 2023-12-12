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
            _ => ProblemNotSolvedString
        };
    }

    private int CountScore()
    {
        return ParseInputLines(parseFunc: ParseCard).Sum(card => ScoreCard(n: card.Wins));
    }

    private int CountCards()
    {
        var cards = ParseInputLines(parseFunc: ParseCard).ToArray();
        var counts = Enumerable.Repeat(element: 1, count: cards.Length).ToArray();

        for (var n = 1; n <= cards.Length; n++)
        for (var w = 1; w <= cards[n - 1].Wins; w++)
        {
            counts[n - 1 + w] += counts[n - 1];
        }

        return counts.Sum();
    }

    private static int ScoreCard(int n)
    {
        return n != 0
            ? (int)Math.Pow(2, n - 1)
            : 0;
    }
    
    private static Card ParseCard(string line)
    {
        var match = Regex.Match(input: line, pattern: @"Card\s+(?:\d+):(?:\s+(?<Wins>\d+))+\s\|(?:\s+(?<Have>\d+))+");
        var wins = match.Groups["Wins"].ParseInts();
        var have = match.Groups["Have"].ParseInts();

        return new Card(Wins: have.Count(wins.Contains));
    }
}