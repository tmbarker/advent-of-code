namespace Utilities.Collections;

internal sealed class DisjointSetNode<T> where T : IEquatable<T>
{
    public T Element { get; }
    public DisjointSetNode<T> Parent { get; set; }
    public int Rank { get; set; }

    public DisjointSetNode(T element)
    {
        Element = element;
        Parent = this;
        Rank = 0;
    }
}