using Utilities.Collections;

namespace Solutions.Y2016.D19;

[PuzzleInfo("An Elephant Named Joseph", Topics.Math, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var count = int.Parse(input);
        
        return part switch
        {
            1 => FindWinnerAdjacent(count),
            2 => FindWinnerAcross(count),
            _ => PuzzleNotSolvedString
        };
    }

    private static int FindWinnerAdjacent(int count)
    {
        var members = new CircularLinkedList<int>(Enumerable.Range(1, count));
        var current = members.Head;
        
        while (members.Count > 1)
        {
            members.Remove(current!.Next!);
            current = current.Next;
        }

        return current!.Value;
    }
    
    private static int FindWinnerAcross(int count)
    {
        var members = new CircularLinkedList<int>(Enumerable.Range(1, count));
        var across = members.Head;

        for (var i = 0; i < count / 2; i++)
        {
            across = across!.Next;
        }
        
        while (members.Count > 1)
        {
            var tmp = members.Count % 2 == 1 
                ? across!.Next!.Next 
                : across!.Next;
            
            members.Remove(across);
            across = tmp;
        }

        return across!.Value;
    }
}