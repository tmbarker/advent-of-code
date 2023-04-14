using System.Text;
using Problems.Common;
using Utilities.Collections;
using Utilities.Extensions;

namespace Problems.Y2017.D16;

using LinkedList = CircularLinkedList<char>;
using NodeMap = IDictionary<char, CircularLinkedListNode<char>>;

/// <summary>
/// Permutation Promenade: https://adventofcode.com/2017/day/16
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var instructions = input.Split(',');

        return part switch
        {
            1 => Execute(instructions, findCycle: false, rounds: 1),
            2 => Execute(instructions, findCycle: true,  rounds: 1000000000),
            _ => ProblemNotSolvedString
        };
    }

    private static string Execute(IList<string> instructions, bool findCycle, int rounds)
    {
        var list = new CircularLinkedList<char>();
        var map = new Dictionary<char, CircularLinkedListNode<char>>();
        
        foreach (var c in @"abcdefghijklmnop".ToCharArray())
        {
            map[c] = list.AddLast(c);
        }

        if (findCycle)
        {
            var steps = 0;
            var seen = new Dictionary<string, int>();
            var key = Concat(list);

            while (seen.TryAdd(key, steps))
            {
                Dance(instructions, list, map);
                steps++;
                key = Concat(list);
            }

            rounds %= steps - seen[key];
        }
        
        for (var i = 0; i < rounds; i++)
        {
            Dance(instructions, list, map);
        }

        return Concat(list);
    }

    private static void Dance(IEnumerable<string> instructions, LinkedList list, NodeMap map)
    {
        foreach (var instr in instructions)
        {
            var args = instr.ParseInts();
            switch (instr)
            {
                case not null when instr.StartsWith('s'):
                    ShiftHead(list, offset: list.Count - args[0]);
                    break;
                case not null when instr.StartsWith('x'):
                    var a = GetElement(list, index: args[0]);
                    var b = GetElement(list, index: args[1]);
                    SwapNodes(a, b, map);
                    break;
                case not null when instr.StartsWith('p'):
                    SwapNodes(a: instr[1], b: instr[3], map);
                    break;
            }
        }
    }

    private static string Concat(LinkedList list)
    {
        var sb = new StringBuilder();
        var node = list.Head!;

        for (var i = 0; i < list.Count; i++)
        {
            sb.Append(node.Value);
            node = node.Next!;
        }

        return sb.ToString();
    }
    
    private static void ShiftHead(LinkedList list, int offset)
    {
        var newHead = list.Head!;
        for (var i = 0; i < offset; i++)
        {
            newHead = newHead.Next!;
        }
        list.MarkHead(newHead);
    }
    
    private static char GetElement(LinkedList list, int index)
    {
        var node = list.Head!;
        for (var i = 0; i < index; i++)
        {
            node = node.Next!;
        }
        return node.Value;
    }
    
    private static void SwapNodes(char a, char b, NodeMap map)
    {
        (map[a].Value, map[b].Value) = (map[b].Value, map[a].Value);
        (map[a],       map[b])       = (map[b],       map[a]);
    }
}