namespace Utilities.Geometry.Euclidean;

/// <summary>
/// Represents a point in Euclidean space 
/// </summary>
/// <typeparam name="T">The type associated with each point</typeparam>
public interface IPoint<T>
{
    /// <summary>
    /// Get a set of adjacent vectors, where adjacency is defined as having a distance of 1
    /// </summary>
    /// <param name="metric">The distance metric to use</param>
    /// <returns>The set of adjacent points</returns>
    public ISet<T> GetAdjacentSet(Metric metric);

    /// <summary>
    /// /// Get the magnitude of the vector according to the specified distance <paramref name="metric"/>
    /// </summary>
    /// <param name="metric"></param>
    /// <returns>The magnitude of the vector</returns>
    public int Magnitude(Metric metric);

    /// <summary>
    /// Get the magnitude of the specified component of the vector
    /// </summary>
    /// <param name="component">The axis of the component</param>
    /// <returns></returns>
    public int GetComponent(Axis component);
}