namespace Problems.Y2017.D06;

public struct MemoryBankState : IEquatable<MemoryBankState>
{
    private const int BitsPerMask = 4;
    private const ulong BankMask = 0xFUL;
    
    private readonly int _numBanks;
    private ulong _store = 0UL;

    private ulong this[int index]
    {
        get => ReadBank(index);
        init => SetBank(index, value);
    }
    
    public MemoryBankState(IList<ulong> banks)
    {
        _numBanks = banks.Count;
        for (var i = 0; i < banks.Count; i++)
        {
            this[i] = banks[i];
        }
    }

    public MemoryBankState Reallocate()
    {
        var banks = 0UL;
        var index = 0;
        var next = new ulong[_numBanks];

        for (var i = 0; i < _numBanks; i++)
        {
            if (this[i] > banks)
            {
                banks = this[i];
                index = i;
            }

            next[i] = this[i];
        }

        next[index] = 0;
        while (banks-- > 0)
        {
            next[++index % _numBanks]++;
        }

        return new MemoryBankState(banks: next);
    }

    private ulong ReadBank(int index)
    {
        return (_store >> (index * BitsPerMask)) & BankMask;
    }
    
    private void SetBank(int index, ulong value)
    {
        _store &= ~(BankMask << (index * BitsPerMask));
        _store |= value << (index * BitsPerMask);
    }

    public bool Equals(MemoryBankState other)
    {
        return _store == other._store && _numBanks == other._numBanks;
    }

    public override bool Equals(object? obj)
    {
        return obj is MemoryBankState other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_store, _numBanks);
    }

    public static bool operator ==(MemoryBankState left, MemoryBankState right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MemoryBankState left, MemoryBankState right)
    {
        return !(left == right);
    }
}