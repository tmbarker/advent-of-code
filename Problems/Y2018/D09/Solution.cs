using Problems.Y2018.Common;
using Utilities.Extensions;

namespace Problems.Y2018.D09;

/// <summary>
/// Marble Mania: https://adventofcode.com/2018/day/9
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 9;
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var gameParams = ParseGameParams(input);
        
        return part switch
        {
            1 => GetWinningScore(gameParams.NumPlayers, numMarbles: gameParams.LastMarbleWorth),
            2 => GetWinningScore(gameParams.NumPlayers, numMarbles: 100 *gameParams.LastMarbleWorth),
            _ => ProblemNotSolvedString
        };
    }

    private static long GetWinningScore(int numPlayers, long numMarbles)
    {
        var scores = Enumerable.Range(0, numPlayers).ToDictionary(
            keySelector: p => p,
            elementSelector: _ => 0L);

        var placed = new LinkedList<long>(new[] { 0L });
        var current = placed.First;
        
        var turnPlayer = 0;
        var nextMarble = 1L;

        while (nextMarble <= numMarbles)
        {
            if (nextMarble % 23L == 0L)
            {
                var toRemove = GetCcw(placed, from: current!, distance: 7);
                var worth = toRemove.Value;
                scores[turnPlayer] += worth + nextMarble;
                
                current = GetCw(placed, from: toRemove, distance: 1);
                placed.Remove(toRemove);
            }
            else
            {
                var placeAfter = GetCw(placed, from: current!, distance: 1);
            
                placed.AddAfter(placeAfter, new LinkedListNode<long>(nextMarble));
                current = placeAfter.Next;
            }

            turnPlayer = (turnPlayer + 1) % numPlayers;
            nextMarble++;
        }

        return scores.Values.Max();
    }

    private static LinkedListNode<T> GetCw<T>(LinkedList<T> placed, LinkedListNode<T> from, int distance)
    {
        var result = from;
        for (var i = 0; i < distance; i++)
        {
            result = result!.Next ?? placed.First!;
        }

        return result;
    }
    
    private static LinkedListNode<T> GetCcw<T>(LinkedList<T> marbles, LinkedListNode<T> from, int distance)
    {
        var result = from;
        for (var i = 0; i < distance; i++)
        {
            result = result!.Previous ?? marbles.Last!;
        }

        return result;
    }

    private static (int NumPlayers, long LastMarbleWorth) ParseGameParams(string input)
    {
        var numbers = input.ParseLongs();
        return ((int)numbers[0], numbers[1]);
    }
}