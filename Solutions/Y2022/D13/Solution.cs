namespace Solutions.Y2022.D13;

[PuzzleInfo("Distress Signal", Topics.StringParsing, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private const string DivisorPacket1 = "[[2]]";
    private const string DivisorPacket2 = "[[6]]";

    public override object Run(int part)
    {
        return part switch
        {
            1 => SumOrderedPacketIndices(PacketParser.ParsePairs(GetInputLines())),
            2 => CalculateDecoderKey(PacketParser.ParsePackets(GetInputLines())),
            _ => ProblemNotSolvedString
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
        var divisor1 = PacketParser.ParseElement(DivisorPacket1);
        var divisor2 = PacketParser.ParseElement(DivisorPacket2);
        
        list.Add(divisor1);
        list.Add(divisor2);
        list.Sort();

        var firstIndex = list.IndexOf(divisor1);
        var secondIndex = list.IndexOf(divisor2);

        return (firstIndex + 1) * (secondIndex + 1);
    }
}