using Utilities.Geometry.Euclidean;

namespace Solutions.Y2016.D22;

public sealed class PosComparer : IComparer<Vec2D>
{
    public int Compare(Vec2D x, Vec2D y)
    {
        var xComparison = x.X.CompareTo(y.X);
        return xComparison != 0 
            ? xComparison : x.Y.CompareTo(y.Y);
    }
}