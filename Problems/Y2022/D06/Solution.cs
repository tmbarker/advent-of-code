using Problems.Common;

namespace Problems.Y2022.D06;

/// <summary>
/// Tuning Trouble: https://adventofcode.com/2022/day/6
/// </summary>
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var datastream = GetInputText();
        return part switch
        {
            1 => ListenForStartMarker(datastream, markerLength: 4),
            2 => ListenForStartMarker(datastream, markerLength: 14),
            _ => ProblemNotSolvedString
        };
    }

    private static int ListenForStartMarker(string datastream, int markerLength)
    {
        var buffer = new Queue<char>(markerLength);
        var bufferContentMap = new Dictionary<char, int>(markerLength);

        for (var i = 0; i < datastream.Length; i++)
        {
            var received = datastream[i];
            if (buffer.Count >= markerLength)
            {
                var token = buffer.Dequeue();

                bufferContentMap[token]--;
                if (bufferContentMap[token] <= 0)
                {
                    bufferContentMap.Remove(token);
                }
            }

            bufferContentMap.TryAdd(received, 0);
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