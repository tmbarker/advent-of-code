using Problems.Y2018.Common;

namespace Problems.Y2018.D08;

/// <summary>
/// Memory Maneuver: https://adventofcode.com/2018/day/8
/// </summary>
public class Solution : SolutionBase2018
{
    public override int Day => 8;
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var buffer = ParseBuffer(input);
            
        return part switch
        {
            1 => SumMetadata(buffer),
            2 => GetNodeValue(buffer),
            _ => ProblemNotSolvedString
        };
    }

    private static int SumMetadata(Queue<int> buffer)
    {
        var sum = 0;
        var numChild = buffer.Dequeue();
        var numMetadata = buffer.Dequeue();

        for (var i = 0; i < numChild; i++)
        {
            sum += SumMetadata(buffer);
        }

        for (var i = 0; i < numMetadata; i++)
        {
            sum += buffer.Dequeue();
        }

        return sum;
    }

    private static int GetNodeValue(Queue<int> buffer)
    {
        var value = 0;
        var numChild = buffer.Dequeue();
        var numMetadata = buffer.Dequeue();

        if (numChild == 0)
        {
            for (var i = 0; i < numMetadata; i++)
            {
                value += buffer.Dequeue();
            }

            return value;
        }

        var childNodeValues = new Dictionary<int, int>();
        for (var i = 0; i < numChild; i++)
        {
            childNodeValues[i + 1] = GetNodeValue(buffer);
        }

        for (var i = 0; i < numMetadata; i++)
        {
            value += childNodeValues.TryGetValue(buffer.Dequeue(), out var metadata) 
                ? metadata 
                : 0;
        }

        return value;
    }
    
    private static Queue<int> ParseBuffer(string input)
    {
        return new Queue<int>(input.Split(' ').Select(int.Parse));
    }
}