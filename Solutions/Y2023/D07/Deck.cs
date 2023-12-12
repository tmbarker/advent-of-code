namespace Solutions.Y2023.D07;

public static class Deck
{
    private const char Joker = 'J';
    private static readonly Dictionary<char, int> RankMap = new()
    {
        { '2',  2 },
        { '3',  3 },
        { '4',  4 },
        { '5',  5 },
        { '6',  6 },
        { '7',  7 },
        { '8',  8 },
        { '9',  9 },
        { 'T', 10 },
        { 'J', 11 },
        { 'Q', 12 },
        { 'K', 13 },
        { 'A', 14 }
    };
    
    public static bool HasJokers { get; set; }

    public static bool IsJoker(char card) => HasJokers && card == Joker;
    public static int GetRank(char card) => IsJoker(card) ? 1 : RankMap[card];
}