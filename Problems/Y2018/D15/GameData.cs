namespace Problems.Y2018.D15;

public static class GameData
{
    public const char Empty = '.';
    public const char Goblin = 'G';
    public const char Elf = 'E';

    public const int Hp = 200;
    public const int Dmg = 3;

    public static readonly SquareComparer SquareComparer = new();
    public static readonly Dictionary<char, char> EnemyMap = new()
    {
        { Elf, Goblin },
        { Goblin, Elf }
    };
}