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
}