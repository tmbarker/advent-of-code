namespace Utilities.Geometry.Euclidean;

/// <summary>
///     Represents a metric in <see cref="Euclidean" /> space
/// </summary>
public enum Metric
{
    /// <summary>
    ///     Also known as the chessboard metric, equivalent to the max of the individual component magnitudes
    /// </summary>
    Chebyshev,
    /// <summary>
    ///     Also known as the Manhattan metric, equivalent to the sum of the individual component magnitudes
    /// </summary>
    Taxicab
}