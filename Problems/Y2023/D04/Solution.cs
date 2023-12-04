using System.Text.RegularExpressions;
using Problems.Common;
using Utilities.Extensions;

namespace Problems.Y2023.D04;

/// <summary>
/// Scratchcards: https://adventofcode.com/2023/day/4
/// </summary>
public class Solution : SolutionBase
{
    private readonly record struct Card(int Id, int NumWins);
    
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
        return ParseInputLines(parseFunc: ParseCard).Sum(card => ScoreCard(n: card.NumWins));
    }

    private int CountCards()
    {
        var map = ParseInputLines(parseFunc: ParseCard).ToDictionary(card => card.Id);
        var queue = new Queue<Card>(collection: map.Values);
        var count = queue.Count;

        while (queue.Any())
        {
            var card = queue.Dequeue();
            for (var i = 1; i <= card.NumWins; i++)
            {
                queue.Enqueue(map[card.Id + i]);
                count++;
            }
        }

        return count;
    }

    private static int ScoreCard(int n)
    {
        return n != 0
            ? (int)Math.Pow(2, n - 1)
            : 0;
    }
    
    private static Card ParseCard(string line)
    {
        var match = Regex.Match(input: line, pattern: @"Card\s+(?<Id>\d+):(?:\s+(?<Wins>\d+))+\s\|(?:\s+(?<Have>\d+))+");
        var wins = match.Groups["Wins"].ParseInts();
        var have = match.Groups["Have"].ParseInts();

        return new Card(
            Id: match.Groups["Id"].ParseInt(),
            NumWins: have.Count(wins.Contains));
    }
}