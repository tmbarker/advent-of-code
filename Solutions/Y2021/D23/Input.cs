using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D23;

public static class Input
{
    public static readonly Dictionary<Vec2D, char> Part1 = new()
    {
        { new Vec2D(X: 2, Y: 1), 'B' },
        { new Vec2D(X: 2, Y: 0), 'C' },
        
        { new Vec2D(X: 4, Y: 1), 'C' },
        { new Vec2D(X: 4, Y: 0), 'D' },
        
        { new Vec2D(X: 6, Y: 1), 'A' },
        { new Vec2D(X: 6, Y: 0), 'D' },
        
        { new Vec2D(X: 8, Y: 1), 'B' },
        { new Vec2D(X: 8, Y: 0), 'A' }
    };
    
    public static readonly Dictionary<Vec2D, char> Part2 = new()
    {
        { new Vec2D(X: 2, Y: 3), 'B' },
        { new Vec2D(X: 2, Y: 2), 'D' },
        { new Vec2D(X: 2, Y: 1), 'D' },
        { new Vec2D(X: 2, Y: 0), 'C' },
        
        { new Vec2D(X: 4, Y: 3), 'C' },
        { new Vec2D(X: 4, Y: 2), 'C' },
        { new Vec2D(X: 4, Y: 1), 'B' },
        { new Vec2D(X: 4, Y: 0), 'D' },
        
        { new Vec2D(X: 6, Y: 3), 'A' },
        { new Vec2D(X: 6, Y: 2), 'B' },
        { new Vec2D(X: 6, Y: 1), 'A' },
        { new Vec2D(X: 6, Y: 0), 'D' },
        
        { new Vec2D(X: 8, Y: 3), 'B' },
        { new Vec2D(X: 8, Y: 2), 'A' },
        { new Vec2D(X: 8, Y: 1), 'C' },
        { new Vec2D(X: 8, Y: 0), 'A' }
    };
}