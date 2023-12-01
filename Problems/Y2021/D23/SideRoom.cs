using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D23;

public class SideRoom
{
    private readonly int _abscissa;
    private readonly int _depth;
    
    public SideRoom(int abscissa, int depth)
    {
        _abscissa = abscissa;
        _depth = depth;
    }

    public Vector2D Top => new(_abscissa, _depth - 1);
    public Vector2D Bottom => new(_abscissa, 0);

    public bool Contains(Vector2D pos)
    {
        return pos.X == _abscissa && pos.Y >= 0 && pos.Y < _depth;
    }
}