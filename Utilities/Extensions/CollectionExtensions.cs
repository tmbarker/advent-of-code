namespace Utilities.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    /// Ensure the <paramref name="dictionary"/> contains <paramref name="key"/>, add the default value of <typeparamref name="TValue"/> if it doesn't
    /// </summary>
    public static void EnsureContainsKey<TKey, TValue>(this IDictionary<TKey, TValue?> dictionary, TKey key) where TKey : notnull
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, default);
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
    /// Ensure the <paramref name="set"/> contains <paramref name="item"/>
    /// </summary>
    public static void EnsureContains<T>(this ISet<T> set, T item)
    {
        if (!set.Contains(item))
        {
            set.Add(item);
        }
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
}