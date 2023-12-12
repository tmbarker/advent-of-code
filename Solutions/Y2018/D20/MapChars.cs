using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D20;

public static class MapChars
{
    public const char Start = 'X';
    public const char Empty = '.';
    
    public static readonly Dictionary<Vector2D, char> DoorChars = new()
    {
        { Vector2D.Up, '-' },
        { Vector2D.Down, '-' },
        { Vector2D.Left, '|' },
        { Vector2D.Right, '|' }
    };
}