using Utilities.Collections;

namespace Solutions.Y2023.D07;

public readonly struct Hand(string cards, int bid) : IComparable<Hand>
{
    private readonly string _cards = cards;
    private readonly (int Most, int Next) _counts = Count(cards);

    public int Bid { get; } = bid;

    private static (int Most, int Next) Count(string cards)
    {
        var counts = new DefaultDict<char, int>(defaultValue: 0);
        var jokers = cards.Count(Deck.IsJoker);

        foreach (var c in cards.Where(c => !Deck.IsJoker(c)))
        {
            counts[c]++;
        }

        var ordered = counts.Values
            .OrderDescending()
            .ToArray();
        var most = ordered.FirstOrDefault() + jokers;
        var next = ordered.ElementAtOrDefault(1);

        return (most, next);
    }

    public int CompareTo(Hand other)
    {
        var mostComparison = _counts.Most.CompareTo(other._counts.Most);
        if (mostComparison != 0)
        {
            return mostComparison;
        }
        
        var nextComparison = _counts.Next.CompareTo(other._counts.Next);
        if (nextComparison != 0)
        {
            return nextComparison;
        }

        for (var i = 0; i < _cards.Length; i++)
        {
            var thisRank = Deck.GetRank(_cards[i]);
            var thatRank = Deck.GetRank(other._cards[i]);
            var rankComparison = thisRank.CompareTo(thatRank);

            if (rankComparison != 0)
            {
                return rankComparison;
            }
        }

        return 0;
    }
}