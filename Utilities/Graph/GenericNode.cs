namespace Utilities.Graph;

/// <summary>
/// A generic type representing a node in a tree
/// </summary>
/// <typeparam name="T">The type of value associated with each node</typeparam>
public sealed class GenericNode<T>(T value)
{
    public T Value { get; set; } = value;
    public List<GenericNode<T>> Children { get; } = [];
}