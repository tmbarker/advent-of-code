namespace Problems.Y2019.D22;

public class Dealer
{
    private readonly long _deckSize;
    
    public Dealer(long deckSize)
    {
        _deckSize = deckSize;
    }
    
    public long StackDeck(long index)
    {
        return _deckSize - 1 - index;
    }
    
    public long CutDeck(long index, long amount)
    {
        return (index - amount) % _deckSize;
    }
    
    public long IncrementDeck(long index, long amount)
    {
        return index * amount % _deckSize;
    }
}