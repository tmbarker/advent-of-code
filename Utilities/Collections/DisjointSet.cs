namespace Utilities.Collections;

/// <summary>
///     A primitive Disjoint Set Forest exposing the regular Union Find operations. <see cref="FindSet" /> uses Path
///     Compression, and <see cref="Union" /> is by Rank
/// </summary>
/// <typeparam name="T">The type of value associated with each element in the set</typeparam>
public sealed class DisjointSet<T> where T : IEquatable<T>
{
    private readonly Dictionary<T, DisjointSetNode<T>> _nodes = new();

    /// <summary>
    ///     The number of total elements (nodes) in the set.
    /// </summary>
    public int ElementsCount => _nodes.Count;
    
    /// <summary>
    ///     The number of partitions (components) in the set.
    /// </summary>
    public int PartitionsCount { get; private set; }

    /// <summary>
    ///     Initialize an empty <see cref="DisjointSet{T}"/>.
    /// </summary>
    public DisjointSet()
    {
    }

    /// <summary>
    ///     Initialize a <see cref="DisjointSet{T}"/> with a set for each element in the
    ///     specified <paramref name="collection"/>.
    /// </summary>
    /// <param name="collection">The elements to make a set with</param>
    public DisjointSet(IEnumerable<T> collection)
    {
        foreach (var element in collection)
        {
            MakeSet(element);
        }
    }
    
    /// <summary>
    ///     Check if the disjoint set contains the specified element. 
    /// </summary>
    /// <param name="element">The element to query membership of</param>
    /// <returns>A <see cref="bool"/> representing the success of the operation</returns>
    public bool ContainsElement(T element)
    {
        return _nodes.ContainsKey(element);
    }

    /// <summary>
    ///     Attempt to add the provided element to the set.
    /// </summary>
    /// <param name="element">The element to add</param>
    /// <returns>
    ///     A <see cref="bool" /> representing the success of the operation. Returns false if the element is already in
    ///     the disjoint set
    /// </returns>
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

    /// <summary>
    ///     Attempt to merge two sets.
    /// </summary>
    /// <param name="elementA">Element of set a</param>
    /// <param name="elementB">Element of set b</param>
    /// <returns>A <see cref="bool"/> representing the success of the operation</returns>
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

    /// <summary>
    ///     Find the set representative for the specified element.
    /// </summary>
    /// <param name="element">The element to find the set representative for</param>
    /// <returns>The representative of the set the element belongs to</returns>
    public T FindSet(T element)
    {
        return FindSet(_nodes[element]).Element;
    }

    /// <summary>
    ///     Clear all nodes from the set.
    /// </summary>
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