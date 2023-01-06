namespace Problems.Y2021.D21;

public record struct Player(int Position, int Score);
public record struct GameState(Player P1, Player P2);
public record struct WinCounts(long P1, long P2);