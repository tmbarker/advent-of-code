namespace Problems.Y2022.D20;

public class MemoryList<T>
{
    private readonly Dictionary<int, MemoryListItem> _originalIndicesMap;
    private readonly List<MemoryListItem> _elements;

    public MemoryList(IList<T> elements)
    {
        _originalIndicesMap = new Dictionary<int, MemoryListItem>(elements.Count);
        _elements = new List<MemoryListItem>(elements.Count);
        
        for (var i = 0; i < elements.Count; i++)
        {
            var item = new MemoryListItem(elements[i]);
            _originalIndicesMap.Add(i, item);
            _elements.Add(item);
        }
    }

    public int Count => _elements.Count;
    
    public T GetValueOriginal(int index)
    {
        ValidateIndex(index);
        return _originalIndicesMap[index].Value;
    }

    public T GetValueCurrent(int index)
    {
        ValidateIndex(index);
        return _elements[index].Value;
    }

    public int OriginalToCurrentIndex(int originalIndex)
    {
        ValidateIndex(originalIndex);
        return _elements.IndexOf(_originalIndicesMap[originalIndex]);
    }

    public void MoveOriginalElement(int originalIndex, int desiredIndex)
    {
        ValidateIndex(originalIndex);
        ValidateIndex(desiredIndex);
        
        var item = _originalIndicesMap[originalIndex];
        var currentIndex = OriginalToCurrentIndex(originalIndex);

        if (currentIndex == desiredIndex)
        {
            return;
        }

        _elements.Remove(item);
        _elements.Insert(desiredIndex, item);
    }

    private void ValidateIndex(int index)
    {
        if (!_originalIndicesMap.ContainsKey(index))
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
    }
    
    private class MemoryListItem
    {
        public MemoryListItem(T value)
        {
            Value = value;
        }
        
        public T Value { get; }
    }
}