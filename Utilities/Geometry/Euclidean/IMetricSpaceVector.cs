namespace Utilities.Geometry.Euclidean;

/// <summary>
/// Represents a vector in <see cref="Euclidean"/> space 
/// </summary>
public interface IMetricSpaceVector<TVector>
{
    /// <summary>
    /// Get the magnitude of the specified component of the vector
    /// </summary>
    /// <param name="component">The axis of the component</param>
    /// <returns>The value of the specified component</returns>
    int this[Axis component] { get; }
    
    /// <summary>
    /// Get the magnitude of the specified component of the vector
    /// </summary>
    /// <param name="component">The axis of the component</param>
    /// <returns>The value of the specified component</returns>
    public int GetComponent(Axis component);
    
    /// <summary>
    /// /// Get the magnitude of the vector according to the specified distance <paramref name="metric"/>
    /// </summary>
    /// <param name="metric"></param>
    /// <returns>The magnitude of the vector</returns>
    public int Magnitude(Metric metric);
    
    /// <summary>
    /// Get a set of adjacent vectors, where adjacency is defined as having a distance of 1
    /// </summary>
    /// <param name="metric">The distance metric to use</param>
    /// <returns>The set of adjacent points</returns>
    public ISet<TVector> GetAdjacentSet(Metric metric);
}