using System.Collections;
using JetBrains.Annotations;

namespace Utilities.Collections;

/// <summary>
///     Represents a collection of keys and values. Querying a value using a key which is not in the collection
///     returns a default value. This default value can be static, or generated using a delegate specified during
///     construction.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
public sealed class DefaultDict<TKey, TValue>(Func<TKey, TValue> defaultSelector)
    : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary = new();

    public bool IsReadOnly => false;
    public int Count => _dictionary.Count;
    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;

    [CollectionAccess(CollectionAccessType.UpdatedContent)]
    public TValue this[TKey key]
    {
        get => IndexGetInternal(key);
        set => IndexSetInternal(key, value);
    }

    public DefaultDict(TValue defaultValue) : this(defaultSelector: _ => defaultValue)
    {
    }

    public DefaultDict(TValue defaultValue, IEnumerable<(TKey Key, TValue Value)> items) : this(defaultValue)
    {
        foreach (var item in items)
        {
            Add(key: item.Key, value: item.Value);
        }
    }
    
    public DefaultDict(TValue defaultValue, IEnumerable<KeyValuePair<TKey, TValue>> items) : this(defaultValue)
    {
        foreach (var item in items)
        {
            Add(key: item.Key, value: item.Value);
        }
    }

    public void Add(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _dictionary[item.Key] = item.Value;
    }

    public bool Remove(TKey key)
    {
        return _dictionary.Remove(key);
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Remove(item.Key);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.TryGetValue(item.Key, out var value) && Equals(value, item.Value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        value = this[key];
        return true;
    }

    public void Clear()
    {
        _dictionary.Clear();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    private void IndexSetInternal(TKey key, TValue value)
    {
        _dictionary[key] = value;
    }

    private TValue IndexGetInternal(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value))
        {
            return value;
        }

        _dictionary[key] = defaultSelector.Invoke(key);
        return _dictionary[key];
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }
}