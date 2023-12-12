namespace Solutions.Y2020.D14;

public readonly struct MaskSimple
{
    private readonly ulong _setMask;
    private readonly ulong _clearMask;
    
    public MaskSimple(string maskStr)
    {
        var setMask = 0UL;
        var clearMask = ~0UL;
        
        for (var i = 0; i < maskStr.Length; i++)
        {
            switch (maskStr[maskStr.Length - i - 1])
            {
                case '0':
                    clearMask &= ~(1UL << i);
                    continue;
                case '1':
                    setMask |= 1UL << i;
                    continue;
            }
        }

        _setMask = setMask;
        _clearMask = clearMask;
    }

    public ulong Apply(ulong value)
    {
        value |= _setMask;
        value &= _clearMask;
        
        return value;
    }
}