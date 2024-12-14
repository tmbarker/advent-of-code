namespace Solutions.Y2020.D14;

public readonly struct MaskFloating
{
    private readonly ulong _setMask;
    private readonly List<int> _floatingBitIndices;

    public MaskFloating(string maskStr)
    {
        var setMask = 0UL;
        var floatingBitIndices = new List<int>();
        
        for (var i = 0; i < maskStr.Length; i++)
        {
            switch (maskStr[maskStr.Length - i - 1])
            {
                case 'X':
                    floatingBitIndices.Add(i);
                    continue;
                case '1':
                    setMask |= 1UL << i;
                    continue;
            }
        }

        _setMask = setMask;
        _floatingBitIndices = floatingBitIndices;
    }

    public IEnumerable<ulong> Apply(ulong value)
    {
        var numFloatingBits = _floatingBitIndices.Count;
        var numBitVariations = (ulong)Math.Pow(2, numFloatingBits);

        if (numFloatingBits == 0)
        {
            yield return value | _setMask;
            yield break;
        }

        for (var i = 0UL; i < numBitVariations; i++)
        {
            var floatingValue = value |= _setMask;
            var bitsLsbFirst = GetBitsLsbFirst(
                value: i,
                padToLength: numFloatingBits);
            
            for (var b = 0; b < numFloatingBits; b++)
            {
                floatingValue = ForceBit(
                    value: floatingValue,
                    bit: _floatingBitIndices[b],
                    set: bitsLsbFirst[b]);
            }
            
            yield return floatingValue;
        }
    }
    
    private static List<bool> GetBitsLsbFirst(ulong value, int padToLength)
    {
        var bits = new List<bool>();
        while (value > 0 || bits.Count < padToLength)
        {
            bits.Add(value % 2 > 0);
            value /= 2;
        }
        return bits;
    }
    
    private static ulong ForceBit(ulong value, int bit, bool set)
    {
        return set
            ? value | (1UL << bit)
            : value & ~(1UL << bit);
    }
}