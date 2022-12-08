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
    public override int Parts => 2;
    
    public override string Run(int part)
    {
        AssertInputExists();

        return part switch
        {
            0 => ListenForStartMarker(PacketMarkerLength).ToString(),
            1 => ListenForStartMarker(MessageMarkerLength).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private int ListenForStartMarker(int markerLength)
    {
        var datastream = File.ReadAllText(GetInputFilePath());
        
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