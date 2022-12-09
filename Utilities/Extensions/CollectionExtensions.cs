namespace Utilities.Extensions;

public static class CollectionExtensions
{
    public static void EnsureContainsKey<TKey, TValue>(this Dictionary<TKey, TValue?> dictionary, TKey key) where TKey : notnull
    {
        if (!dictionary.ContainsKey(key))
        {
            dictionary.Add(key, default);
        }
    }
}