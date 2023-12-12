using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D23;

public static class Input
{
    public static readonly Dictionary<Vector2D, char> Part1 = new()
    {
        { new Vector2D(x: 2, y: 1), 'B' },
        { new Vector2D(x: 2, y: 0), 'C' },
        
        { new Vector2D(x: 4, y: 1), 'C' },
        { new Vector2D(x: 4, y: 0), 'D' },
        
        { new Vector2D(x: 6, y: 1), 'A' },
        { new Vector2D(x: 6, y: 0), 'D' },
        
        { new Vector2D(x: 8, y: 1), 'B' },
        { new Vector2D(x: 8, y: 0), 'A' }
    };
    
    public static readonly Dictionary<Vector2D, char> Part2 = new()
    {
        { new Vector2D(x: 2, y: 3), 'B' },
        { new Vector2D(x: 2, y: 2), 'D' },
        { new Vector2D(x: 2, y: 1), 'D' },
        { new Vector2D(x: 2, y: 0), 'C' },
        
        { new Vector2D(x: 4, y: 3), 'C' },
        { new Vector2D(x: 4, y: 2), 'C' },
        { new Vector2D(x: 4, y: 1), 'B' },
        { new Vector2D(x: 4, y: 0), 'D' },
        
        { new Vector2D(x: 6, y: 3), 'A' },
        { new Vector2D(x: 6, y: 2), 'B' },
        { new Vector2D(x: 6, y: 1), 'A' },
        { new Vector2D(x: 6, y: 0), 'D' },
        
        { new Vector2D(x: 8, y: 3), 'B' },
        { new Vector2D(x: 8, y: 2), 'A' },
        { new Vector2D(x: 8, y: 1), 'C' },
        { new Vector2D(x: 8, y: 0), 'A' }
    };
}