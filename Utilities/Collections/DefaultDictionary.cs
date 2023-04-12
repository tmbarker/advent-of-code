using System.Collections;

namespace Utilities.Collections;

/// <summary>
/// Represents a collection of keys and values. Querying a value using a key which is not in the collection
/// returns a default value. This default value can be static, or generated using a delegate specified during
/// construction.
/// </summary>
/// <typeparam name="TKey">The type of the keys in the dictionary</typeparam>
/// <typeparam name="TValue">The type of the values in the dictionary</typeparam>
public class DefaultDictionary<TKey, TValue> : IDictionary<TKey, TValue> where TKey : notnull
{
    private readonly Dictionary<TKey, TValue> _dictionary;
    private readonly Func<TKey, TValue> _defaultSelector;

    public bool IsReadOnly => false;
    public int Count => _dictionary.Count;
    public ICollection<TKey> Keys => _dictionary.Keys;
    public ICollection<TValue> Values => _dictionary.Values;
    
    public TValue this[TKey key]
    {
        get => _dictionary.TryGetValue(key, out var value) ? value : _defaultSelector.Invoke(key);
        set => _dictionary[key] = value;
    }

    public DefaultDictionary(Func<TKey, TValue> defaultSelector)
    {
        _dictionary = new Dictionary<TKey, TValue>();
        _defaultSelector = defaultSelector;
    }

    public DefaultDictionary(TValue defaultValue) : this(defaultSelector: _ => defaultValue)
    {
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
        return _dictionary.Contains(item);
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

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionary).GetEnumerator();
    }
}