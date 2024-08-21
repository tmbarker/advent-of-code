namespace Utilities.Geometry.Euclidean;

/// <summary>
///     Throw helper for common Vector exceptions.
/// </summary>
/// <typeparam name="TVec">The type of the Vector consumer, used to add detail to exception messages</typeparam>
public abstract class VecThrowHelper<TVec>
{
    public static Exception InvalidComponent(Axis component)
    {
        return new ArgumentException(
            message: $"The {component} component does not exist in {nameof(TVec)} space");
    }

    public static Exception InvalidMetric(Metric metric)
    {
        return new ArgumentException(
            message: $"The {metric} distance metric is not well defined over {nameof(TVec)} space");
    }
}