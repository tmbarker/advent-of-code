using Utilities.Geometry.Euclidean;

namespace Problems.Y2018.D13;

public class TickOrderComparer : IComparer<Vector2D>
{
    public int Compare(Vector2D a, Vector2D b)
    {
        var yComparison = a.Y.CompareTo(b.Y);
        return yComparison != 0 
            ? yComparison 
            : a.X.CompareTo(b.X);
    }
}