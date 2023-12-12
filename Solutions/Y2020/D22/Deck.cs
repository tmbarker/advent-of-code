namespace Solutions.Y2020.D22;

public sealed class Deck
{
    private readonly Queue<int> _cards;

    public bool HasCards => _cards.Any();
    public int CardsRemaining => _cards.Count;
    public string State => string.Join(',', _cards);
    
    public Deck(IEnumerable<int> initial)
    {
        _cards = new Queue<int>(initial);
    }

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
        while (_cards.Any())
        {
            score += _cards.Count * _cards.Dequeue();
        }
        return score;
    }
}