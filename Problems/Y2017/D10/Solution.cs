using Problems.Y2017.Common;
using Utilities.Extensions;

namespace Problems.Y2017.D10;

/// <summary>
/// Knot Hash: https://adventofcode.com/2017/day/10
/// </summary>
public class Solution : SolutionBase2017
{
    private static readonly byte[] LengthSuffixValues = { 17, 31, 73, 47, 23 };
    
    public override int Day => 10;
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var circle = Enumerable.Range(0, 256)
            .Select(n => (byte)n)
            .ToArray();
        
        return part switch
        {
            1 => HashSimple(input, circle),
            2 => HashFull(input, circle),
            _ => ProblemNotSolvedString
        };
    }

    private static int HashSimple(string input, byte[] circle)
    {
        var lengths = input.ParseInts().Select(n => (byte)n).ToArray();
        var sparse = Hash(
            circle: circle,
            lengths: lengths,
            rounds: 1);
        
        return sparse[0] * sparse[1];
    }

    private static string HashFull(string input, byte[] circle)
    {
        var ascii = input
            .Select(c => (byte)c)
            .Concat(LengthSuffixValues)
            .ToArray();
        
        var sparse = Hash(
            circle: circle,
            lengths: ascii,
            rounds: 64);
        
        var dense = new byte[16];
        for (var i = 0; i < 16; i++)
        for (var j = 0; j < 16; j++)
        {
            dense[i] ^= sparse[16 * i + j];
        }

        return Convert.ToHexString(dense).ToLower();
    }
    
    private static byte[] Hash(byte[] circle, byte[] lengths, int rounds)
    {
        var currentPos = 0;
        var skipLength = 0;

        for (var i = 0; i < rounds; i++)
        {
            foreach (var length in lengths)
            {
                ReverseSegment(circle, currentPos, length);
                currentPos = (currentPos + length + skipLength) % circle.Length;
                skipLength++;
            }   
        }

        return circle;
    }

    private static void ReverseSegment(IList<byte> circle, int start, int count)
    {
        var stack = new Stack<byte>();
        for (var i = 0; i < count; i++)
        {
            stack.Push(circle[(start + i) % circle.Count]);
        }

        for (var i = 0; i < count; i++)
        {
            circle[(start + i) % circle.Count] = stack.Pop();
        }
    }
}