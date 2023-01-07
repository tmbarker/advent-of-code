namespace Utilities.Cartesian;

public enum DistanceMetric
{
    /// <summary>
    /// The standard distance metric in Cartesian space, the length of a line segment between two points
    /// </summary>
    Euclidean,
    /// <summary>
    /// Also known as the chessboard metric, equivalent to the max of the individual component magnitudes
    /// </summary>
    Chebyshev,
    /// <summary>
    /// Also known as the Manhattan metric, equivalent to the sum of the individual component magnitudes
    /// </summary>
    Taxicab,
}