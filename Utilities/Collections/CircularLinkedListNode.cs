namespace Utilities.Collections;

/// <summary>
///     A node in a <see cref="CircularLinkedList{T}" />.
/// </summary>
/// <typeparam name="T">The type of the encapsulated <see cref="Value" /></typeparam>
public sealed class CircularLinkedListNode<T>
{
    public CircularLinkedList<T>? List { get; internal set; }
    public CircularLinkedListNode<T>? Next { get; internal set; }
    public CircularLinkedListNode<T>? Prev { get; internal set; }
    public T Value { get; set; }

    internal CircularLinkedListNode(CircularLinkedList<T> list, T value)
    {
        List = list;
        Value = value;
    }

    internal void Invalidate()
    {
        List = null;
        Next = null;
        Prev = null;
    }
}