using Utilities.Collections;
using Utilities.Extensions;

namespace Problems.Y2016.D21;

[PuzzleInfo("Scrambled Letters and Hash", Topics.Math|Topics.Simulation, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var steps = GetInputLines();
        return part switch
        {
            1 => Scramble(seed: @"abcdefgh", steps),
            2 => Unscramble(result: @"fbgdceah", steps),
            _ => ProblemNotSolvedString
        };
    }

    private static string Scramble(string seed, IEnumerable<string> steps)
    {
        var password = new CircularLinkedList<char>(seed);
        
        foreach (var step in steps)
        {
            var tokens = step.Split(' ');
            var args = step.ParseInts();

            switch (step)
            {
                case not null when step.StartsWith("swap position"):
                    SwapPositions(password, x: args[0], y: args[1]);
                    break;
                case not null when step.StartsWith("swap letter"):
                    SwapPositions(password, x: tokens[2].Single(), y: tokens[5].Single());
                    break;
                case not null when step.StartsWith("rotate left"):
                    RotateLeft(password, n: args[0]);
                    break;
                case not null when step.StartsWith("rotate right"):
                    RotateRight(password, n: args[0]);
                    break;
                case not null when step.StartsWith("rotate based"):
                    RotateRelative(password, x: tokens[6].Single());
                    break;
                case not null when step.StartsWith("reverse"):
                    ReverseRange(password, x: args[0], y: args[1]);
                    break;
                case not null when step.StartsWith("move"):
                    MovePosition(password, x: args[0], y: args[1]);
                    break;
            }
        }
        
        return password.BuildRepresentativeString(separator: string.Empty);
    }
    
    private static string Unscramble(string result, IEnumerable<string> steps)
    {
        var password = new CircularLinkedList<char>(result);
        
        foreach (var step in steps.Reverse())
        {
            var tokens = step.Split(' ');
            var args = step.ParseInts();

            switch (step)
            {
                case not null when step.StartsWith("swap position"):
                    SwapPositions(password, x: args[0], y: args[1]);
                    break;
                case not null when step.StartsWith("swap letter"):
                    SwapPositions(password, x: tokens[2].Single(), y: tokens[5].Single());
                    break;
                case not null when step.StartsWith("rotate left"):
                    RotateRight(password, n: args[0]);
                    break;
                case not null when step.StartsWith("rotate right"):
                    RotateLeft(password, n: args[0]);
                    break;
                case not null when step.StartsWith("rotate based"):
                    InverseRotateRelative(password, x: tokens[6].Single());
                    break;
                case not null when step.StartsWith("reverse"):
                    ReverseRange(password, x: args[0], y: args[1]);
                    break;
                case not null when step.StartsWith("move"):
                    MovePosition(password, x: args[1], y: args[0]);
                    break;
            }
        }
        
        return password.BuildRepresentativeString(separator: string.Empty);
    }

    private static void SwapPositions(CircularLinkedList<char> pass, int x, int y)
    {
        var nx = pass.GetNode(x);
        var ny = pass.GetNode(y);

        (nx.Value, ny.Value) = (ny.Value, nx.Value);
    }
    
    private static void SwapPositions(CircularLinkedList<char> pass, char x, char y)
    {
        var nx = pass.FindNode(x, out _)!;
        var ny = pass.FindNode(y, out _)!;

        (nx.Value, ny.Value) = (ny.Value, nx.Value);
    }
    
    private static void RotateLeft(CircularLinkedList<char> pass, int n)
    {
        for (var i = 0; i < n; i++)
        {
            pass.MarkHead(pass.Head!.Next!);
        }
    }
    
    private static void RotateRight(CircularLinkedList<char> pass, int n)
    {
        for (var i = 0; i < n; i++)
        {
            pass.MarkHead(pass.Head!.Prev!);
        }
    }
    
    private static void RotateRelative(CircularLinkedList<char> pass, char x)
    {
        pass.FindNode(x, out var index);
        RotateRight(pass, n: index + 1);

        if (index >= 4)
        {
            RotateRight(pass, n: 1);
        }
    }
    
    private static void InverseRotateRelative(CircularLinkedList<char> pass, char x)
    {
        //  This is how the forward direction of this step works
        //
        //  Start   Shift   Final
        //      0       1       1
        //      1       2       3
        //      2       3       5
        //      3       4       7
        //      4       6       2
        //      5       7       4
        //      6       8       6
        //      7       9       0

        pass.FindNode(x, out var index);
        RotateLeft(pass, n: index / 2 + (index % 2 == 1 || index == 0 ? 1 : 5));
    }

    private static void ReverseRange(CircularLinkedList<char> pass, int x, int y)
    {
        var from = pass.GetNode(x);
        var to = pass.GetNode(y);
        var count = y - x + 1;
        
        pass.ReverseRange(from, count, preserveHead: false);

        if (x == 0)
        {
            pass.MarkHead(to);
        }
    }
    
    private static void MovePosition(CircularLinkedList<char> pass, int x, int y)
    {
        var nx = pass.GetNode(x);
        pass.Remove(nx);

        if (y == 0)
        {
            pass.AddFirst(nx.Value);
            return;
        }

        var ny = pass.GetNode(index: y - 1);
        pass.AddAfter(ny, nx.Value);
    }
}