using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D15;

public sealed class SquareComparer : IComparer<Vec2D>
{
    public int Compare(Vec2D a, Vec2D b)
    {
        var yComparison = a.Y.CompareTo(b.Y);
        return yComparison != 0 
            ? yComparison 
            : a.X.CompareTo(b.X);
    }
}