namespace Utilities.Geometry.Euclidean;

/// <summary>
/// Throw helper for common Vector exceptions.
/// </summary>
/// <typeparam name="TVector">The type of the Vector consumer, used to add detail to exception messages</typeparam>
public abstract class VectorThrowHelper<TVector>
{
    public static Exception InvalidComponent(Axis component)
    {
        return new ArgumentException(
            message: $"The {component} component does not exist in {nameof(TVector)} space");
    }
        
    public static Exception InvalidMetric(Metric metric)
    {
        return new ArgumentException(
            message: $"The {metric} distance metric is not well defined over {nameof(TVector)} space");
    }
}