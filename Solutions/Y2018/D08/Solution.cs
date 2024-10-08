namespace Solutions.Y2018.D08;

[PuzzleInfo("Memory Maneuver", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var buffer = ParseBuffer(input);
            
        return part switch
        {
            1 => SumMetadata(buffer),
            2 => GetNodeValue(buffer),
            _ => PuzzleNotSolvedString
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
            value += childNodeValues.GetValueOrDefault(buffer.Dequeue(), 0);
        }

        return value;
    }
    
    private static Queue<int> ParseBuffer(string input)
    {
        return new Queue<int>(input.Split(' ').Select(int.Parse));
    }
}