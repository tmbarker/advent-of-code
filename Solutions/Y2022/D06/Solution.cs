using Utilities.Collections;

namespace Solutions.Y2022.D06;

[PuzzleInfo("Tuning Trouble", Topics.StringParsing, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var datastream = GetInputText();
        return part switch
        {
            1 => ListenForStartMarker(datastream, markerLength: 4),
            2 => ListenForStartMarker(datastream, markerLength: 14),
            _ => PuzzleNotSolvedString
        };
    }

    private static int ListenForStartMarker(string datastream, int markerLength)
    {
        var buffer = new Queue<char>(capacity: markerLength);
        var bufferContentMap = new DefaultDict<char, int>(defaultValue: 0);

        for (var i = 0; i < datastream.Length; i++)
        {
            var received = datastream[i];
            if (buffer.Count >= markerLength)
            {
                var token = buffer.Dequeue();
                if (--bufferContentMap[token] <= 0)
                {
                    bufferContentMap.Remove(token);
                }
            }
            
            bufferContentMap[received]++;
            buffer.Enqueue(received);

            if (bufferContentMap.Count == markerLength)
            {
                return i + 1;
            }
        }

        throw new NoSolutionException();
    }
}