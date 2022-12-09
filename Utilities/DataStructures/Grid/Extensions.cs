namespace Utilities.DataStructures.Grid;

public static class Extensions
{
    /// <summary>
    /// Determine if two positions are adjacent, where adjacent means the Chebyshev distance is less than or equal to 1
    /// </summary>
    public static bool IsAdjacentTo(this IPosition2D lhs, IPosition2D rhs)
    {
        return ChebyshevDistance(lhs, rhs) <= 1;
    }

    /// <summary>
    /// Compute the Chebyshev distance, which is also known as the Chessboard distance
    /// </summary>
    public static int ChebyshevDistance(this IPosition2D lhs, IPosition2D rhs)
    {
        var dx = Math.Abs(lhs.X - rhs.X);
        var dy = Math.Abs(lhs.Y - rhs.Y);

        return Math.Max(dx, dy);
    }
}