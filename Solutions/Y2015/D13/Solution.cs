using Utilities.Collections;
using Utilities.Extensions;

namespace Solutions.Y2015.D13;

[PuzzleInfo("Knights of the Dinner Table", Topics.Graphs, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const string Gain = "gain";
    private const string You = "you";
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => GetMax(you: false),
            2 => GetMax(you: true),
            _ => ProblemNotSolvedString
        };
    }

    private int GetMax(bool you)
    {
        var notes = ParseInputLines(parseFunc:l => l.Split(' '));
        var names = new HashSet<string>();
        var costs = new Dictionary<(string, string), int>();

        foreach (var tokens in notes)
        {
            names.Add(tokens[0]);
            costs[(tokens[0], tokens[^1][..^1])] = (tokens[2] == Gain ? 1 : -1) * int.Parse(tokens[3]);
        }

        if (you)
        {
            foreach (var name in names)
            {
                costs[(You, name)] = 0;
                costs[(name, You)] = 0;
            }
            names.Add(You);
        }

        return names
            .Permute()
            .Max(seating => Score(seating, costs));
    }

    private static int Score(IEnumerable<string> seats, IDictionary<(string, string), int> costs)
    {
        var score = 0;
        var circle = new CircularLinkedList<string>(seats);
        var current = circle.Head!;

        for (var i = 0; i < circle.Count; i++)
        {
            score += costs[(current.Value, current.Prev!.Value)];
            score += costs[(current.Value, current.Next!.Value)];

            current = current.Next;
        }
        
        return score;
    }
}

    