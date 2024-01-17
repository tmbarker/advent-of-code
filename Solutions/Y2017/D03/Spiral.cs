using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D03;

public sealed class Spiral
{
    private readonly DefaultDict<Vec2D, int> _memory;
    private Vec2D _dir = Vec2D.Right;

    public int LastVal { get; private set; }
    public Vec2D LastPos { get; private set; }
    public Vec2D NextPos => LastPos + _dir;

    public int this[Vec2D pos]
    {
        get => _memory[pos];
        private set => StoreValue(pos, value);
    }
    
    public Spiral()
    {
        _memory = new DefaultDict<Vec2D, int>(defaultValue: 0);
        StoreValue(pos: Vec2D.Zero, value: 1);
    }

    public void Build(Func<Spiral, int> valueFunc, Predicate<Spiral> stopFunc)
    {
        var stepsBeforeTurn = 1;
        var stepsSinceLastTurn = 0;
        var turnsSinceStepIncrement = 0;

        while (!stopFunc(this))
        {
            if (stepsSinceLastTurn == stepsBeforeTurn)
            {
                _dir = Rot3D.P90Z * _dir;
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

    private void StoreValue(Vec2D pos, int value)
    {
        _memory[pos] = value;
        LastVal = value;
        LastPos = pos;
    }
}