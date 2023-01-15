namespace Utilities.Numerics;

public static class BitHelper
{
    public static ulong ForceBit(ulong value, int bit, bool set)
    {
        return set
            ? value | (1UL << bit)
            : value & ~(1UL << bit);
    }
    
    public static IList<bool> GetBitsLsbFirst(ulong value, int padToLength = 1)
    {
        var bits = new List<bool>();
        while (value > 0 || bits.Count < padToLength)
        {
            bits.Add(value % 2 > 0);
            value /= 2;
        }
        return bits;
    }
}