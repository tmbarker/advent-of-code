namespace Problems.Y2021.D21;

public class Player
{
    public Player(int initialPosition)
    {
        Position = initialPosition;
    }
    
    public int Score { get; set; }
    public int Position { get; set; }

    public override string ToString()
    {
        return $"[Position = {Position}, Score = {Score}]";
    }
}