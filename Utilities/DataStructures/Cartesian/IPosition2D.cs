namespace Utilities.DataStructures.Cartesian;

/// <summary>
/// A simple  interface which can be used to index <see cref="Grid2D{T}"/> instances
/// </summary>
public interface IPosition2D
{
    /// <summary>
    /// An ID which is unique to any given (<see cref="X"/>, <see cref="Y"/>) pair
    /// </summary>
    string Id { get; }
    /// <summary>
    /// The X coordinate
    /// </summary>
    int X { get; }
    /// <summary>
    /// The Y coordinate
    /// </summary>
    int Y { get; }
}