using Utilities.Cartesian;

namespace Problems.Y2021.D23;

public static class Input
{
    public static readonly Dictionary<Vector2D, char> Part1 = new()
    {
        { new Vector2D(2, 1), 'B' },
        { new Vector2D(2, 0), 'C' },
        
        { new Vector2D(4, 1), 'C' },
        { new Vector2D(4, 0), 'D' },
        
        { new Vector2D(6, 1), 'A' },
        { new Vector2D(6, 0), 'D' },
        
        { new Vector2D(8, 1), 'B' },
        { new Vector2D(8, 0), 'A' },
    };
    
    public static readonly Dictionary<Vector2D, char> Part2 = new()
    {
        { new Vector2D(2, 3), 'B' },
        { new Vector2D(2, 2), 'D' },
        { new Vector2D(2, 1), 'D' },
        { new Vector2D(2, 0), 'C' },
        
        { new Vector2D(4, 3), 'C' },
        { new Vector2D(4, 2), 'C' },
        { new Vector2D(4, 1), 'B' },
        { new Vector2D(4, 0), 'D' },
        
        { new Vector2D(6, 3), 'A' },
        { new Vector2D(6, 2), 'B' },
        { new Vector2D(6, 1), 'A' },
        { new Vector2D(6, 0), 'D' },
        
        { new Vector2D(8, 3), 'B' },
        { new Vector2D(8, 2), 'A' },
        { new Vector2D(8, 1), 'C' },
        { new Vector2D(8, 0), 'A' },
    };
}