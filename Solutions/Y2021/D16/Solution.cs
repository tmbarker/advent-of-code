using System.Text;
using Utilities.Extensions;

namespace Solutions.Y2021.D16;

[PuzzleInfo("Packet Decoder", Topics.StringParsing, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, string> HexToPaddedBinary = new() {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'B', "1011" },
        { 'C', "1100" },
        { 'D', "1101" },
        { 'E', "1110" },
        { 'F', "1111" }
    };

    public override object Run(int part)
    {
        var packet = ParsePacket(ReadInputToBinaryBuffer());
        return part switch
        {
            1 => SumPacketVersionNumbers(packet),
            2 => packet.Evaluate(),
            _ => PuzzleNotSolvedString
        };
    }

    private static Packet ParsePacket(Queue<char> buffer)
    {
        var version = (int)ReadSectionToDecimal(buffer, Section.Version);
        var @operator = (Operator)ReadSectionToDecimal(buffer, Section.TypeId);

        if (@operator == Operator.Identity)
        {
            return new LiteralPacket(version, ParseLiteralValue(buffer));
        }

        var subPackets = new List<Packet>(); 
        var lengthTypeId = (int)ReadSectionToDecimal(buffer, Section.LengthTypeId);
        
        if (lengthTypeId == 0)
        {
            var numSubPacketBits = (int)ReadSectionToDecimal(buffer, Section.SubPacketBits);
            var bitsParsed = 0;

            while (bitsParsed < numSubPacketBits)
            {
                var before = buffer.Count;
                subPackets.Add(ParsePacket(buffer));
                bitsParsed += before - buffer.Count;
            }
        }
        else
        {
            var numSubPackets = (int)ReadSectionToDecimal(buffer, Section.SubPacketCount);
            for (var n = 0; n < numSubPackets; n++)
            {
                subPackets.Add(ParsePacket(buffer));
            }
        }

        return new OperatorPacket(version, @operator, subPackets);
    }

    private static int SumPacketVersionNumbers(Packet root)
    {
        var sum = 0;
        var stack = new Stack<Packet>(collection: [root]);

        while (stack.Count > 0)
        {
            var p = stack.Pop();
            sum += p.Version;
            
            foreach (var s in p.SubPackets)
            {
                stack.Push(s);
            }
        }

        return sum;
    }
    
    private Queue<char> ReadInputToBinaryBuffer()
    {
        return new Queue<char>(HexToBin(GetInputText()));
    }
    
    private static long ParseLiteralValue(Queue<char> binaryBuffer)
    {
        var sb = new StringBuilder();
        var end = false;

        while (!end)
        {
            var chunk = Read(binaryBuffer, 5);
            end = chunk[0] == '0';
            sb.Append(chunk[1..]);
        }

        return BinToDec(sb.ToString());
    }

    private static long ReadSectionToDecimal(Queue<char> buffer, Section section)
    {
        var binary = section switch
        {
            Section.Version =>        Read(buffer, n: 3),
            Section.TypeId =>         Read(buffer, n: 3),
            Section.LengthTypeId =>   Read(buffer, n: 1),
            Section.SubPacketCount => Read(buffer, n: 11),
            Section.SubPacketBits =>  Read(buffer, n: 15),
            _ => throw new NoSolutionException()
        };

        return BinToDec(binary);
    }

    private static string Read(Queue<char> buffer, int n)
    {
        var sb = new StringBuilder();
        while (n-- > 0)
        {
            sb.Append(buffer.Dequeue());
        }
        return sb.ToString();
    }
    
    private static long BinToDec(string binary)
    {
        var value = 0L;
        var digits = binary.Length;
        
        for (var i = 0; i < digits; i++)
        {
            var p = (long)Math.Pow(2, i);
            var d = binary[digits - i - 1].AsDigit();
            value += d * p;
        }
        
        return value;
    }
    
    private static string HexToBin(string hex)
    {
        var sb = new StringBuilder();
        foreach (var c in hex.ToUpperInvariant())
        {
            sb.Append(HexToPaddedBinary[c]);
        }
        return sb.ToString();
    }
}