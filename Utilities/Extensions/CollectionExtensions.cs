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