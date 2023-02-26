using System.Text.RegularExpressions;
using Problems.Common;
using Problems.Y2019.Common;

namespace Problems.Y2019.D22;

/// <summary>
/// Slam Shuffle: https://adventofcode.com/2019/day/22
/// </summary>
public class Solution : SolutionBase2019
{
    private const string Stack = "stack";
    private const string Cut = "cut";
    private const string Increment = "increment";

    private static readonly Regex NumberRegex = new(@"(-?\d+)");
    
    public override int Day => 22;
    
    public override object Run(int part)
    {
        return part switch
        {
            0 => FollowCard(card: 2019L, deckSize: 10007L),
            1 => TraceCard(index: 2020L, deckSize: 119315717514047L, numShuffles: 101741582076661L),
            _ => ProblemNotSolvedString
        };
    }

    private long FollowCard(long card, long deckSize)
    {
        var steps = GetInputLines();
        var dealer = new Dealer(deckSize);
        var index = card;
        
        foreach (var step in steps)
        {
            var amount = ParseArg(step);
            index = step switch
            {
                { } when step.Contains(Stack) => dealer.StackDeck(index),
                { } when step.Contains(Cut) => dealer.CutDeck(index, amount),
                { } when step.Contains(Increment) => dealer.IncrementDeck(index, amount),
                _ => throw new NoSolutionException()
            };
        }
        
        return index;
    }

    private long TraceCard(long index, long deckSize, long numShuffles)
    {
        var steps = GetInputLines();
        var reversedSteps = new List<string>(steps.Reverse());
        var inverseDealer = new InverseDealer(deckSize);
        
        foreach (var step in reversedSteps)
        {
            var amount = ParseArg(step);
            index = step switch
            {
                { } when step.Contains(Stack) => inverseDealer.UndoStack(index),
                { } when step.Contains(Cut) => inverseDealer.UndoCut(index, amount),
                { } when step.Contains(Increment) => inverseDealer.UndoIncrement(index, amount),
                _ => throw new NoSolutionException()
            };
        }

        throw new NotImplementedException($"TODO: Apply the shuffle repeatedly [N={numShuffles}]");
    }
    
    private static long ParseArg(string line)
    {
        var match = NumberRegex.Match(line);
        return match.Success
            ? long.Parse(match.Groups[0].Value)
            : 0L;
    }
}