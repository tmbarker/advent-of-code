using Utilities.Cartesian;

namespace Problems.Y2017.D03;

public class SpiralMemory
{
    private readonly Dictionary<Vector2D, int> _grid = new();
    private Vector2D _dir = Vector2D.Right;

    public int LastVal { get; private set; }
    public Vector2D LastPos { get; private set; }
    public Vector2D NextPos => LastPos + _dir;

    public int this[Vector2D pos]
    {
        get => _grid.TryGetValue(pos, out var value) ? value : 0;
        private set => StoreValue(pos, value);
    }
    
    public SpiralMemory()
    {
        StoreValue(pos: Vector2D.Zero, value: 1);
    }

    public void Run(Func<SpiralMemory, int> valueFunc, Predicate<SpiralMemory> stopPredicate)
    {
        var stepsBeforeTurn = 1;
        var stepsSinceLastTurn = 0;
        var turnsSinceStepIncrement = 0;

        while (!stopPredicate(this))
        {
            if (stepsSinceLastTurn == stepsBeforeTurn)
            {
                _dir = Rotation3D.Positive90Z * _dir;
                stepsSinceLastTurn = 0;
                turnsSinceStepIncrement++;
            }

            if (turnsSinceStepIncrement == 2)
            {
                stepsBeforeTurn++;
                turnsSinceStepIncrement = 0;
            }
            
            stepsSinceLastTurn++;
            this[LastPos + _dir] = valueFunc(this);
        }
    }

    private void StoreValue(Vector2D pos, int value)
    {
        _grid[pos] = value;

        LastVal = value;
        LastPos = pos;
    }
}