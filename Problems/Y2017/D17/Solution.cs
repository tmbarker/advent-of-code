using Problems.Y2017.Common;
using Utilities.Collections;
using Utilities.Extensions;

namespace Problems.Y2017.D17;

/// <summary>
/// Spinlock: https://adventofcode.com/2017/day/17
/// </summary>
public class Solution : SolutionBase2017
{
    public override int Day => 17;
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var steps = input.ParseInt();

        return part switch
        {
            1 => Spin(steps, insertions: 2017),
            2 => Watch(steps, insertions: 50000000),
            _ => ProblemNotSolvedString
        };
    }

    private static int Spin(int steps, int insertions)
    {
        var list = new CircularLinkedList<int>();
        var node = list.AddFirst(0);

        for (var i = 0; i < insertions; i++)
        {
            node = Step(node, steps: steps % list.Count);
            node = list.AddAfter(node, value: i + 1);
        }

        return node.Next!.Value;
    }

    private static int Watch(int steps, int insertions)
    {
        var count = 1;
        var index = 0;
        var watch = 0;

        for (var i = 0; i < insertions; i++)
        {
            index = (index + steps) % count++;
            index++;
            
            if (index == 1)
            {
                watch = i + 1;
            }
        }
        
        return watch;
    }
    
    private static CircularLinkedListNode<int> Step(CircularLinkedListNode<int> node, int steps)
    {
        for (var i = 0; i < steps; i++)
        {
            node = node.Next!;
        }
        return node;
    }
}