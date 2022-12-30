using System.Text;
using Problems.Common;
using Problems.Y2021.Common;
using Utilities.Numerics;

namespace Problems.Y2021.D16;

/// <summary>
/// Packet Decoder: https://adventofcode.com/2021/day/16
/// </summary>
public class Solution : SolutionBase2021
{
    public override int Day => 16;
    
    public override object Run(int part)
    {
        var packet = ParsePacket(ReadToPacketBinaryBuffer());
        return part switch
        {
            0 => SumPacketVersionNumbers(packet),
            1 => packet.Evaluate(),
            _ => ProblemNotSolvedString,
        };
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

    private static Packet ParsePacket(Queue<char> buffer)
    {
        var version = (int)BaseConversion.BinaryToDecimal(ReadSection(buffer, Section.Version));
        var @operator = (Operator)BaseConversion.BinaryToDecimal(ReadSection(buffer, Section.TypeId));

        if (@operator == Operator.Identity)
        {
            return new LiteralPacket(version, ParseLiteralValue(buffer));
        }

        var subPackets = new List<Packet>(); 
        var lengthTypeId = (int)BaseConversion.BinaryToDecimal(ReadSection(buffer, Section.LengthTypeId));
        
        if (lengthTypeId == 0)
        {
            var numSubPacketBits = (int)BaseConversion.BinaryToDecimal(ReadMultiple(buffer, 15));
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
            var numSubPackets = (int)BaseConversion.BinaryToDecimal(ReadMultiple(buffer, 11));
            for (var n = 0; n < numSubPackets; n++)
            {
                subPackets.Add(ParsePacket(buffer));
            }
        }

        return new OperatorPacket(version, @operator, subPackets);
    }

    private Queue<char> ReadToPacketBinaryBuffer()
    {
        AssertInputExists();
        
        var hex = File.ReadAllText(GetInputFilePath());
        var binary = BaseConversion.HexToBinary(hex);
        
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

        return BaseConversion.BinaryToDecimal(sb.ToString());
    }

    private static string ReadSection(Queue<char> buffer, Section section)
    {
        return section switch
        {
            Section.Version => ReadMultiple(buffer, 3),
            Section.TypeId => ReadMultiple(buffer, 3),
            Section.LengthTypeId => ReadMultiple(buffer, 1),
            Section.LiteralChunk => ReadMultiple(buffer, 5),
            _ => throw new NoSolutionException(),
        };
    }

    private static string ReadMultiple(Queue<char> buffer, int n)
    {
        var sb = new StringBuilder();
        while (n-- > 0)
        {
            sb.Append(buffer.Dequeue());
        }
        return sb.ToString();
    }
}