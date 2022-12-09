namespace Utilities.DataStructures.Grid;

public static class Extensions
{
    public static bool IsAdjacent(this IPosition2D lhs, IPosition2D rhs)
    {
        return ChebyshevDistance(lhs, rhs) <= 1;
    }

    public static int ChebyshevDistance(this IPosition2D lhs, IPosition2D rhs)
    {
        var dx = Math.Abs(lhs.X - rhs.X);
        var dy = Math.Abs(lhs.Y - rhs.Y);

        return Math.Max(dx, dy);
    }
}