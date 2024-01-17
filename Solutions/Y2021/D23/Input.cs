using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D23;

public static class Input
{
    public static readonly Dictionary<Vec2D, char> Part1 = new()
    {
        { new Vec2D(x: 2, y: 1), 'B' },
        { new Vec2D(x: 2, y: 0), 'C' },
        
        { new Vec2D(x: 4, y: 1), 'C' },
        { new Vec2D(x: 4, y: 0), 'D' },
        
        { new Vec2D(x: 6, y: 1), 'A' },
        { new Vec2D(x: 6, y: 0), 'D' },
        
        { new Vec2D(x: 8, y: 1), 'B' },
        { new Vec2D(x: 8, y: 0), 'A' }
    };
    
    public static readonly Dictionary<Vec2D, char> Part2 = new()
    {
        { new Vec2D(x: 2, y: 3), 'B' },
        { new Vec2D(x: 2, y: 2), 'D' },
        { new Vec2D(x: 2, y: 1), 'D' },
        { new Vec2D(x: 2, y: 0), 'C' },
        
        { new Vec2D(x: 4, y: 3), 'C' },
        { new Vec2D(x: 4, y: 2), 'C' },
        { new Vec2D(x: 4, y: 1), 'B' },
        { new Vec2D(x: 4, y: 0), 'D' },
        
        { new Vec2D(x: 6, y: 3), 'A' },
        { new Vec2D(x: 6, y: 2), 'B' },
        { new Vec2D(x: 6, y: 1), 'A' },
        { new Vec2D(x: 6, y: 0), 'D' },
        
        { new Vec2D(x: 8, y: 3), 'B' },
        { new Vec2D(x: 8, y: 2), 'A' },
        { new Vec2D(x: 8, y: 1), 'C' },
        { new Vec2D(x: 8, y: 0), 'A' }
    };
}