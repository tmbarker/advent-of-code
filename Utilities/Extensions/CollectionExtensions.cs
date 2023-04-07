using Utilities.Cartesian;

namespace Utilities.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Ensure the <paramref name="dictionary"/> contains <paramref name="key"/>, add the default value of <typeparamref name="TValue"/> if it doesn't
    /// </summary>
    public static void EnsureContainsKey<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull where TValue : notnull
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, default!);
        }
    }

    /// <summary>
    /// Filter the dictionary entries so only Key-Value pairs with distinct a Value are returned
    /// </summary>
    public static Dictionary<TKey, TValue> FilterByDistinctValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : notnull
    {
        return dictionary
            .GroupBy(kvp => kvp.Value)
            .Where(g => g.Count() == 1)
            .Select(g => g.First())
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
    
    /// <summary>
    /// Filter the dictionary entries using a Value Predicate
    /// </summary>
    public static Dictionary<TKey, TValue> WhereValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<TValue> predicate) where TKey : notnull
    {
        return dictionary.Where(kvp => predicate(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
    
    /// <summary>
    /// Filter the dictionary entries using a Key Predicate
    /// </summary>
    public static Dictionary<TKey, TValue> WhereKeys<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Predicate<TKey> predicate) where TKey : notnull
    {
        return dictionary.Where(kvp => predicate(kvp.Key))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    /// Check if an <see cref="IList{T}"/> has an element at <paramref name="index"/>
    /// </summary>
    public static bool HasElementAtIndex<T>(this IList<T> list, int index)
    {
        return index >= 0 && index < list.Count;
    }
    
    /// <summary>
    /// Add <see cref="item"/> if and only if it is not null
    /// </summary>
    public static void AddIfNotNull<T>(this IList<T> list, T? item)
    {
        if (item != null)
        {
            list.Add(item);
        }
    }

    /// <summary>
    /// Initialize a <see cref="IReadOnlyCollection{T}"/> using the elements from <paramref name="collection"/>
    /// </summary>
    public static IReadOnlyCollection<T> Freeze<T>(this IEnumerable<T> collection)
    {
        return new List<T>(collection);
    }
    
    /// <summary>
    /// Remove a single element from <paramref name="collection"/>, if it exists
    /// </summary>
    public static ICollection<T> Except<T>(this IEnumerable<T> collection, T single)
    {
        return new List<T>(collection.Except(new[] { single }));
    }

    /// <summary>
    /// Find the intersection of multiple collections
    /// </summary>
    /// <param name="collections">The collections to intersect</param>
    /// <returns>A set containing the elements which are present in all provided collections</returns>
    public static ISet<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> collections)
    {
        var enumerable = collections.ToList();
        var presentInAll = enumerable
            .Skip(1)
            .Aggregate(
                new HashSet<T>(enumerable.First()),
                (inAll, nextCollection) =>
                {
                    inAll.IntersectWith(nextCollection);
                    return inAll;
                }
            );

        return presentInAll;
    } 

    /// <summary>
    /// Normalize the values of the collection such that the "smallest" becomes <see cref="Vector2D.Zero"/>
    /// </summary>
    public static IEnumerable<Vector2D> Normalize(this IEnumerable<Vector2D> collection)
    {
        var enumerated = collection.ToList();
        var delta = new Vector2D(
            x: enumerated.Min(v => v.X),
            y: enumerated.Min(v => v.Y));

        foreach (var vector in enumerated)
        {
            yield return vector - delta;
        }
    }

    /// <summary>
    /// Normalize the values of the dictionary such that the "smallest" value is <see cref="Vector2D.Zero"/>
    /// </summary>
    public static void NormalizeValues<TKey>(this IDictionary<TKey, Vector2D> dictionary)
    {
        var delta = new Vector2D(
            x: dictionary.Values.Min(v => v.X),
            y: dictionary.Values.Min(v => v.Y));
        
        foreach (var (key, _) in dictionary)
        {
            dictionary[key] -= delta;
        }
    }

    /// <summary>
    /// Return the mode of the collection, i.e. the element with the highest frequency, 
    /// </summary>
    public static T Mode<T>(this IEnumerable<T> collection) where T : notnull
    {
        return collection
            .GroupBy(e => e)
            .MaxBy(g => g.Count())!.Key;
    }
}