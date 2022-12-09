namespace Utilities.DataStructures.Grid;

public static class Extensions
{
    public static bool IsAdjacent(this IPosition2D lhs, IPosition2D rhs)
    {
        var dx = Math.Abs(lhs.X - rhs.X);
        var dy = Math.Abs(lhs.Y - rhs.Y);

        return dx <= 1 && dy <= 1;
    }
}