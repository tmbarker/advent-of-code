using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D20;

public static class MapChars
{
    public const char Start = 'X';
    public const char Empty = '.';
    
    public static readonly Dictionary<Vec2D, char> DoorChars = new()
    {
        { Vec2D.Up, '-' },
        { Vec2D.Down, '-' },
        { Vec2D.Left, '|' },
        { Vec2D.Right, '|' }
    };
}