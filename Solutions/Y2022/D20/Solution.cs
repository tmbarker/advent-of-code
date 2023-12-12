using Utilities.Collections;
using Utilities.Extensions;

namespace Solutions.Y2022.D20;

[PuzzleInfo("Grove Positioning System", Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private static readonly HashSet<int> CoordinateOffsets =
    [
        1000,
        2000,
        3000
    ];

    public override object Run(int part)
    {
        var encryptedNumbers = ParseInputLines(parseFunc: int.Parse);
        return part switch
        {
            1 => Decode(encryptedNumbers, mixes:  1, key: 1L),
            2 => Decode(encryptedNumbers, mixes: 10, key: 811589153L),
            _ => ProblemNotSolvedString
        };
    }

    private static long Decode(IEnumerable<int> numbers, int mixes, long key)
    {
        var list = new CircularLinkedList<long>(numbers.Select(n => n * key));
        var order = new List<CircularLinkedListNode<long>>(capacity: list.Count);
        var count = list.Count;
        
        var zero = list.Head!;
        var current = list.Head!;

        for (var i = 0; i < count; i++)
        {
            if (current!.Value == 0)
            {
                zero = current;
            }
            
            order.Add(current);
            current = current.Next;
        }

        for (var m = 0; m < mixes; m++)
        for (var i = 0; i < count; i++)
        {
            var node = order[i];
            var prev = node.Prev!;
            var move = node.Value.Modulo(count - 1);

            list.Remove(node);
            for (var j = 0; j < move; j++)
            {
                prev = prev.Next!;
            }

            list.AddAfter(prev, node);
        }  
        
        var sum = 0L;
        foreach (var offset in CoordinateOffsets)
        {
            var node = zero!;
            var move = offset.Modulo(count);
            
            for (var i = 0; i < move; i++)
            {
                node = node.Next!;
            }
            
            sum += node.Value;
        }

        return sum;
    }
}