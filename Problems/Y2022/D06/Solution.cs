using Problems.Common;
using Problems.Y2022.Common;

namespace Problems.Y2022.D06;

/// <summary>
/// Tuning Trouble: https://adventofcode.com/2022/day/6
/// </summary>
public class Solution : SolutionBase2022
{
    private const int PacketMarkerLength = 4;
    private const int MessageMarkerLength = 14;
    
    public override int Day => 6;
    
    public override object Run(int part)
    {
        var datastream = GetInputText();
        return part switch
        {
            1 =>  ListenForStartMarker(datastream, PacketMarkerLength),
            2 =>  ListenForStartMarker(datastream, MessageMarkerLength),
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
                var outshifted = buffer.Dequeue();

                bufferContentMap[outshifted]--;
                if (bufferContentMap[outshifted] <= 0)
                {
                    bufferContentMap.Remove(outshifted);
                }
            }
            
            if (!bufferContentMap.ContainsKey(received))
            {
                bufferContentMap.Add(received, 0);
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