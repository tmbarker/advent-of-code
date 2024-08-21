namespace Utilities.Collections;

/// <summary>
///     A primitive Disjoint Set Forest exposing the regular Union Find operations. <see cref="FindSet" /> uses Path
///     Compression, and <see cref="Union" /> is by Rank
/// </summary>
/// <typeparam name="T">The type of value associated with each element in the set</typeparam>
public sealed class DisjointSet<T> where T : IEquatable<T>
{
    private readonly Dictionary<T, DisjointSetNode<T>> _nodes = new();

    public int ElementsCount => _nodes.Count;
    public int PartitionsCount { get; private set; }

    public bool ContainsElement(T element)
    {
        return _nodes.ContainsKey(element);
    }

    public bool MakeSet(T element)
    {
        if (ContainsElement(element))
        {
            return false;
        }

        _nodes[element] = new DisjointSetNode<T>(element);
        PartitionsCount++;
        return true;
    }

    public bool Union(T elementA, T elementB)
    {
        var parentA = FindSet(_nodes[elementA]);
        var parentB = FindSet(_nodes[elementB]);

        if (parentA == parentB)
        {
            return false;
        }

        if (parentA.Rank >= parentB.Rank)
        {
            if (parentA.Rank == parentB.Rank)
            {
                parentA.Rank++;
            }

            parentB.Parent = parentA;
        }
        else
        {
            parentA.Parent = parentB;
        }

        PartitionsCount--;
        return true;
    }

    public T FindSet(T element)
    {
        return FindSet(_nodes[element]).Element;
    }

    public void Clear()
    {
        _nodes.Clear();
        PartitionsCount = 0;
    }

    private static DisjointSetNode<T> FindSet(DisjointSetNode<T> node)
    {
        var parent = node.Parent;
        if (parent == node)
        {
            return node;
        }

        node.Parent = FindSet(node.Parent);
        return node.Parent;
    }
}