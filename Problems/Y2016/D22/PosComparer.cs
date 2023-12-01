using Utilities.Geometry;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2016.D22;

public class PosComparer : IComparer<Vector2D>
{
    public int Compare(Vector2D x, Vector2D y)
    {
        var xComparison = x.X.CompareTo(y.X);
        return xComparison != 0 
            ? xComparison : x.Y.CompareTo(y.Y);
    }
}