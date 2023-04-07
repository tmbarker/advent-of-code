namespace Utilities.Collections;

/// <summary>
/// A node in a <see cref="CircularLinkedList{T}"/>
/// </summary>
/// <typeparam name="T">The type of the encapsulated <see cref="Value"/></typeparam>
public class CircularLinkedListNode<T>
{
    public CircularLinkedListNode<T>? Next { get; protected internal set; }
    public CircularLinkedListNode<T>? Prev { get; protected internal set; }
    public T Value { get; set; }

    public CircularLinkedListNode(T value)
    {
        Value = value;
    }

    internal void Invalidate()
    {
        Next = null;
        Prev = null;
    }
}