using Utilities.Numerics;

namespace Problems.Y2020.D14;

public readonly struct MaskFloating
{
    private readonly ulong _setMask;
    private readonly IList<int> _floatingBitIndices;

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
            var bitsLsbFirst = BitHelper.GetBitsLsbFirst(i, numFloatingBits);
            
            for (var b = 0; b < numFloatingBits; b++)
            {
                floatingValue = BitHelper.ForceBit(floatingValue, _floatingBitIndices[b], bitsLsbFirst[b]);
            }
            yield return floatingValue;
        }
    }
}