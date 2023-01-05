namespace Problems.Y2021.D21;

public class DeterministicDie
{
    private readonly int _sides;
    private int _nextRollValue;

    public DeterministicDie(int sides)
    {
        _sides = sides;
        _nextRollValue = 1;
        
        NumRolls = 0;
    }

    public int NumRolls { get; private set; }

    public int Roll()
    {
        NumRolls++;
        var value = _nextRollValue++;

        if (_nextRollValue > _sides)
        {
            _nextRollValue = 1;
        }
        
        return value;
    }
}