using Utilities.Geometry.Euclidean;

namespace Solutions.Y2025.D09;

public sealed class RectilinearPolygon2D 
{
    private readonly Vec2D[] _wrappedVertices;
    private readonly int[] _criticalX;
    private readonly int[] _criticalY;
    
    private readonly Dictionary<Vec2D, bool> _containsMemo = new();

    public RectilinearPolygon2D(Vec2D[] vertices)
    {
        _wrappedVertices = vertices.Append(vertices[0]).ToArray();
        _criticalX = vertices.Select(v => v.X).Distinct().Order().ToArray();
        _criticalY = vertices.Select(v => v.Y).Distinct().Order().ToArray();
    }

    public bool Contains(Aabb2D aabb)
    {
        var minX = aabb.Min.X;
        var maxX = aabb.Max.X;
        var minY = aabb.Min.Y;
        var maxY = aabb.Max.Y;
        
        if (!Contains(minX, minY) || !Contains(maxX, maxY) || !Contains(maxX, minY) || !Contains(minX, maxY))
        {
            return false;
        }

        var criticalXStart = Array.BinarySearch(_criticalX, minX);
        var criticalXEnd   = Array.BinarySearch(_criticalX, maxX);
        var criticalYStart = Array.BinarySearch(_criticalY, minY);
        var criticalYEnd   = Array.BinarySearch(_criticalY, maxY);
        
        if (criticalXStart < 0) criticalXStart = ~criticalXStart;
        if (criticalXEnd   < 0) criticalXEnd   = ~criticalXEnd;
        if (criticalYStart < 0) criticalYStart = ~criticalYStart;
        if (criticalYEnd   < 0) criticalYEnd   = ~criticalYEnd;
        
        for (var i = criticalXStart; i < criticalXEnd; i++)
        {
            var x = _criticalX[i];
            if (x > minX && x < maxX && (!Contains(x, minY) || !Contains(x, maxY)))
            {
                return false;
            }
        }
        
        for (var i = criticalYStart; i < criticalYEnd; i++)
        {
            var y = _criticalY[i];
            if (y > minY && y < maxY && (!Contains(minX, y) || !Contains(maxX, y)))
            {
                return false;
            }
        }

        return true;
    }

    private bool Contains(int x, int y)
    {
        var point = new Vec2D(x, y);
        var rcHit = 0;
        
        if (_containsMemo.TryGetValue(point, out var result))
        {
            return result;
        }
        
        for (var i = 1; i < _wrappedVertices.Length; i++)
        {
            var vi = _wrappedVertices[i];
            var vj = _wrappedVertices[i - 1];
            
            if (vi.X == vj.X)
            {
                if (x == vi.X && ((vi.Y <= y && y <= vj.Y) || (vj.Y <= y && y <= vi.Y)))
                {
                    return _containsMemo[point] = true;
                }
                
                if (x < vi.X && ((vi.Y <= y && y < vj.Y) || (vj.Y <= y && y < vi.Y)))
                {
                    rcHit++;
                }
            }
            else if (y == vi.Y && ((vi.X <= x && x <= vj.X) || (vj.X <= x && x <= vi.X)))
            {
                return _containsMemo[point] = true;
            }
        }

        return _containsMemo[point] = (rcHit & 1) == 1;
    }
}