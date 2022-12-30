using System.Text;
using Problems.Y2021.Common;

namespace Problems.Y2021.D16;

/// <summary>
/// Packet Decoder: https://adventofcode.com/2021/day/16
/// </summary>
public class Solution : SolutionBase2021
{
    private static readonly Dictionary<char, string> HexCharToBinary = new() {
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
    
    public override int Day => 16;
    
    public override object Run(int part)
    {
        var packet = ParsePacket(ReadInputToBinaryBuffer());
        return part switch
        {
            0 => SumPacketVersionNumbers(packet),
            1 => packet.Evaluate(),
            _ => ProblemNotSolvedString,
        };
    }

    private static Packet ParsePacket(Queue<char> buffer)
    {
        var version = (int)BinToDec(ReadSection(buffer, Section.Version));
        var @operator = (Operator)BinToDec(ReadSection(buffer, Section.TypeId));

        if (@operator == Operator.Identity)
        {
            return new LiteralPacket(version, ParseLiteralValue(buffer));
        }

        var subPackets = new List<Packet>(); 
        var lengthTypeId = (int)BinToDec(ReadSection(buffer, Section.LengthTypeId));
        
        if (lengthTypeId == 0)
        {
            var numSubPacketBits = (int)BinToDec(Read(buffer, 15));
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
            var numSubPackets = (int)BinToDec(Read(buffer, 11));
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
        var stack = new Stack<Packet>(new[] { root });

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
        AssertInputExists();
        
        var hex = File.ReadAllText(GetInputFilePath());
        var binary = HexToBin(hex);
        
        return new Queue<char>(binary);
    }
    
    private static long ParseLiteralValue(Queue<char> binaryBuffer)
    {
        var sb = new StringBuilder();
        var end = false;

        while (!end)
        {
            var chunk = ReadSection(binaryBuffer, Section.LiteralChunk);
            end = chunk[0] == '0';
            sb.Append(chunk[1..]);
        }

        return BinToDec(sb.ToString());
    }

    private static string ReadSection(Queue<char> buffer, Section section)
    {
        return section switch
        {
            Section.Version => Read(buffer, 3),
            Section.TypeId => Read(buffer, 3),
            Section.LengthTypeId => Read(buffer, 1),
            Section.LiteralChunk => Read(buffer, 5),
            Section.SubPacketCount => Read(buffer, 11),
            Section.SubPacketBits => Read(buffer, 15),
            _ => throw new ArgumentOutOfRangeException(nameof(section), section.ToString()),
        };
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
            var d = binary[digits - i - 1] - '0';
            value += d * p;
        }
        
        return value;
    }
    
    private static string HexToBin(string hex)
    {
        var sb = new StringBuilder();
        foreach (var c in hex.ToUpperInvariant())
        {
            sb.Append(HexCharToBinary[c]);
        }
        return sb.ToString();
    }
}