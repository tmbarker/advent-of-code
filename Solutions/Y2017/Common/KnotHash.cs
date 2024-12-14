namespace Solutions.Y2017.Common;

public static class KnotHash
{
    private const int SparseBytes = 256;
    private const int DenseBytes = 16;
    private const int Rounds = 64;

    private static readonly byte[] LengthSuffixValues = [17, 31, 73, 47, 23];
    
    public static string ComputeHash(string input)
    {
        var dense = new byte[DenseBytes];
        var sparse = Enumerable.Range(0, SparseBytes)
            .Select(n => (byte)n)
            .ToArray();
        var ascii = input
            .Select(c => (byte)c)
            .Concat(LengthSuffixValues)
            .ToArray();
        
        sparse = TieKnots(
            buffer: sparse,
            lengths: ascii,
            rounds: Rounds);
        
        for (var i = 0; i < dense.Length; i++)
        for (var j = 0; j < dense.Length; j++)
        {
            dense[i] ^= sparse[dense.Length * i + j];
        }

        return Convert.ToHexString(dense).ToLower();
    }
    
    public static byte[] TieKnots(byte[] buffer, byte[] lengths, int rounds)
    {
        var currentPos = 0;
        var skipLength = 0;

        for (var i = 0; i < rounds; i++)
        {
            foreach (var length in lengths)
            {
                ReverseSegment(buffer, start: currentPos, count: length);
                currentPos = (currentPos + length + skipLength) % buffer.Length;
                skipLength++;
            }   
        }

        return buffer;
    }

    private static void ReverseSegment(byte[] buffer, int start, int count)
    {
        var stack = new Stack<byte>();
        for (var i = 0; i < count; i++)
        {
            stack.Push(buffer[(start + i) % buffer.Length]);
        }

        for (var i = 0; i < count; i++)
        {
            buffer[(start + i) % buffer.Length] = stack.Pop();
        }
    }
}