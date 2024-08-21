namespace Solutions.Y2020.D22;

public sealed class Deck(IEnumerable<int> initial)
{
    private readonly Queue<int> _cards = new(collection: initial);

    public bool HasCards => _cards.Count != 0;
    public int CardsRemaining => _cards.Count;
    public string State => string.Join(',', _cards);

    public Deck Copy(int numCards)
    {
        return new Deck(_cards.Take(numCards));
    }
    
    public int DrawFromTop()
    {
        return _cards.Dequeue();
    }

    public void AddToBottom(int card)
    {
        _cards.Enqueue(card);
    }

    public int Score()
    {
        var score = 0;
        while (_cards.Count != 0)
        {
            score += _cards.Count * _cards.Dequeue();
        }
        return score;
    }
}