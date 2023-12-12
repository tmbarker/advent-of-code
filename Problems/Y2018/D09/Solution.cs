using Utilities.Collections;
using Utilities.Extensions;

namespace Problems.Y2018.D09;

[PuzzleInfo("Marble Mania", Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var gameParams = ParseGameParams(input);

        return part switch
        {
            1 => GetWinningScore(gameParams.NumPlayers, numMarbles: gameParams.LastMarbleWorth),
            2 => GetWinningScore(gameParams.NumPlayers, numMarbles: 100 * gameParams.LastMarbleWorth),
            _ => ProblemNotSolvedString
        };
    }

    private static long GetWinningScore(int numPlayers, long numMarbles)
    {
        var scores = Enumerable.Range(0, numPlayers).ToDictionary(
            keySelector: p => p,
            elementSelector: _ => 0L);

        var placed = new CircularLinkedList<long>(new[] { 0L });
        var current = placed.Head;
        
        var turnPlayer = 0;
        var nextMarble = 1L;

        while (nextMarble <= numMarbles)
        {
            if (nextMarble % 23L == 0L)
            {
                var toRemove = GetCcw(from: current!, distance: 7);
                var worth = toRemove.Value;
                
                scores[turnPlayer] += worth + nextMarble;
                current = GetCw(from: toRemove, distance: 1);
                placed.Remove(toRemove);
            }
            else
            {
                var placeAfter = GetCw(from: current!, distance: 1);

                placed.AddAfter(placeAfter, nextMarble);
                current = placeAfter.Next;
            }

            turnPlayer = (turnPlayer + 1) % numPlayers;
            nextMarble++;
        }

        return scores.Values.Max();
    }

    private static CircularLinkedListNode<T> GetCw<T>(CircularLinkedListNode<T> from, int distance)
    {
        var result = from;
        for (var i = 0; i < distance; i++)
        {
            result = result!.Next;
        }
        return result!;
    }
    
    private static CircularLinkedListNode<T> GetCcw<T>(CircularLinkedListNode<T> from, int distance)
    {
        var result = from;
        for (var i = 0; i < distance; i++)
        {
            result = result!.Prev;
        }
        return result!;
    }

    private static (int NumPlayers, long LastMarbleWorth) ParseGameParams(string input)
    {
        var numbers = input.ParseLongs();
        return ((int)numbers[0], numbers[1]);
    }
}