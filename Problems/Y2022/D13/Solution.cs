using Problems.Y2022.Common;

namespace Problems.Y2022.D13;

/// <summary>
/// Distress Signal: https://adventofcode.com/2022/day/13
/// </summary>
public class Solution : SolutionBase2022
{
    private const string DivisorPacket1 = "[[2]]";
    private const string DivisorPacket2 = "[[6]]";

    public override int Day => 13;
    
    public override string Run(int part)
    {
        return part switch
        {
            0 => SumOrderedPacketIndices(PacketParser.ParsePairs(GetInput())).ToString(),
            1 => CalculateDecoderKey(PacketParser.ParsePackets(GetInput())).ToString(),
            _ => ProblemNotSolvedString,
        };
    }

    private static int SumOrderedPacketIndices(IEnumerable<PacketPair> pairs)
    {
        return pairs
            .Where(pair => pair.First.CompareTo(pair.Second) < 0)
            .Sum(pair => pair.Index);
    }
    
    private static int CalculateDecoderKey(IEnumerable<PacketElement> packets)
    {
        var list = packets.ToList();
        var divisor1 = PacketParser.ParseElement(DivisorPacket1)!;
        var divisor2 = PacketParser.ParseElement(DivisorPacket2)!;
        
        list.Add(divisor1);
        list.Add(divisor2);
        list.Sort();

        var firstIndex = list.IndexOf(divisor1);
        var secondIndex = list.IndexOf(divisor2);

        return (firstIndex + 1) * (secondIndex + 1);
    }
}