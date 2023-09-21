namespace Utilities.Graph;

/// <summary>
/// A generic type representing the node in a tree
/// </summary>
/// <typeparam name="T">The type of value associated with each node</typeparam>
public class GenericTreeNode<T>
{
    public T Value { get; set; }
    public List<GenericTreeNode<T>> Children { get; } = new();
    
    public GenericTreeNode(T value)
    {
        Value = value;
    }
}