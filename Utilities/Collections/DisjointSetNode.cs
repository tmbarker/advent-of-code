namespace Utilities.Collections;

internal sealed class DisjointSetNode<T> where T : IEquatable<T>
{
    internal T Element { get; }
    internal DisjointSetNode<T> Parent { get; set; }
    internal int Rank { get; set; }

    internal DisjointSetNode(T element)
    {
        Element = element;
        Parent = this;
        Rank = 0;
    }
}