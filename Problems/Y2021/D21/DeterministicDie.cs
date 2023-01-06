namespace Problems.Y2021.D21;

public class DeterministicDie
{
    private const int Sides = 100;
    private int _nextRollValue = 1;
    
    public int NumRolls { get; private set; }

    public int Roll()
    {
        NumRolls++;
        var value = _nextRollValue++;

        if (_nextRollValue > Sides)
        {
            _nextRollValue = 1;
        }
        
        return value;
    }
}