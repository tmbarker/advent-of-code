using Utilities.Geometry.Euclidean;

namespace Solutions.Y2025.D09;

public sealed class RectilinearPolygon 
{
    private readonly Vec2D[] _vertices;
    private readonly int[] _criticalX;
    private readonly int[] _criticalY;
    private readonly Dictionary<(int X, int Y), bool> _memo = new();

    public RectilinearPolygon(Vec2D[] vertices)
    {
        _vertices = vertices;
        _criticalX = vertices.Select(v => v.X).Distinct().Order().ToArray();
        _criticalY = vertices.Select(v => v.Y).Distinct().Order().ToArray();
    }

    public bool Contains(Aabb2D aabb)
    {
        if (!ContainsPoint(aabb.Min.X, aabb.Min.Y) || !ContainsPoint(aabb.Max.X, aabb.Max.Y) ||
            !ContainsPoint(aabb.Max.X, aabb.Min.Y) || !ContainsPoint(aabb.Min.X, aabb.Max.Y))
        {
            return false;
        }

        foreach (var y in _criticalY.Where(y => y >= aabb.Min.Y && y <= aabb.Max.Y))
        {
            if (!ContainsPoint(aabb.Min.X, y) || !ContainsPoint(aabb.Max.X, y))
            {
                return false;
            }
        }

        foreach (var x in _criticalX.Where(x => x >= aabb.Min.X && x <= aabb.Max.X))
        {
            if (!ContainsPoint(x, aabb.Min.Y) || !ContainsPoint(x, aabb.Max.Y))
            {
                return false;
            }
        }

        return true;
    }

    private bool ContainsPoint(int x, int y)
    {
        if (_memo.TryGetValue((x, y), out var result))
        {
            return result;
        }
        
        return _memo[(x, y)] = IsPointInsideOrOnBoundary(x, y);
    }

    private bool IsPointInsideOrOnBoundary(int x, int y)
    {
        var intersections = 0;
        
        for (var i = 0; i < _vertices.Length; i++)
        {
            var v1 = _vertices[i];
            var v2 = _vertices[(i + 1) % _vertices.Length];

            if (v1.X == v2.X)
            {
                if (x == v1.X && IsBetween(y, v1.Y, v2.Y, maxInclusive: true))
                {
                    return true;
                }
                
                if (IsBetween(y, v1.Y, v2.Y, maxInclusive: false) && x < v1.X)
                {
                    intersections++;
                }
            }
            else if (y == v1.Y && IsBetween(x, v1.X, v2.X, maxInclusive: true))
            {
                return true;
            }
        }

        return (intersections & 1) == 1;
    }

    private static bool IsBetween(int value, int a, int b, bool maxInclusive)
    {
        var (min, max) = a < b ? (a, b) : (b, a);
        return maxInclusive 
            ? value >= min && value <= max 
            : value >= min && value < max;
    }
}