using System.Collections;

namespace Utilities.Collections;

/// <summary>
///     A generic fixed size circular buffer. An exposed indexer (<see cref="this[int]" />) allows for non-FIFO use cases.
/// </summary>
/// <typeparam name="T">The type associated with each element in the buffer</typeparam>
public sealed class CircularBuffer<T> : IEnumerable<T>
{
    private readonly T[] _buffer;
    private int _readIndex;
    private int _writeIndex;

    public int Count { get; private set; }
    public int Capacity => _buffer.Length;
    public bool IsEmpty => Count == 0;
    public bool IsFull => Count == Capacity;

    public CircularBuffer(int capacity) : this(capacity, items: [])
    {
    }

    public CircularBuffer(int capacity, T[] items)
    {
        if (capacity < 1)
        {
            throw ThrowHelper.CapacityInvalid();
        }

        if (items == null)
        {
            throw ThrowHelper.ItemsNull();
        }

        if (items.Length > capacity)
        {
            throw ThrowHelper.CapacityExceeded(items.Length, capacity);
        }

        Count = items.Length;

        _buffer = new T[capacity];
        _readIndex = 0;
        _writeIndex = Count == capacity ? 0 : Count;

        Array.Copy(sourceArray: items, destinationArray: _buffer, items.Length);
    }

    /// <summary>
    ///     Index into the buffer at the specified position.
    /// </summary>
    /// <param name="index">The position to index</param>
    /// <exception cref="IndexOutOfRangeException">The provided index is out of range of the buffer</exception>
    public T this[int index]
    {
        get
        {
            ValidateIndex(index);
            return _buffer[InternalIndex(index)];
        }
        set
        {
            ValidateIndex(index);
            _buffer[InternalIndex(index)] = value;
        }
    }

    /// <summary>
    ///     Enqueue the <paramref name="item" /> into the buffer. When full, enqueuing will overwrite the oldest element.
    /// </summary>
    /// <param name="item">The element to enqueue</param>
    public void Enqueue(T item)
    {
        if (IsFull)
        {
            _buffer[_writeIndex] = item;
            IncrementIndex(ref _writeIndex);
            _readIndex = _writeIndex;
        }
        else
        {
            _buffer[_writeIndex] = item;
            IncrementIndex(ref _writeIndex);
            Count++;
        }
    }

    public T Dequeue()
    {
        ThrowIfEmpty();
        var tmp = _buffer[_readIndex];
        IncrementIndex(ref _readIndex);
        Count--;
        return tmp;
    }

    public T PeekHead()
    {
        ThrowIfEmpty();
        return _buffer[_readIndex];
    }

    public T PeekTail()
    {
        ThrowIfEmpty();
        var tailIndex = _writeIndex != 0
            ? _writeIndex - 1
            : _buffer.Length - 1;

        return _buffer[tailIndex];
    }

    public void Clear()
    {
        _readIndex = 0;
        _writeIndex = 0;
        Count = 0;
        Array.Clear(_buffer, index: 0, _buffer.Length);
    }

    public void Print()
    {
        Console.WriteLine($"[{this}]");
    }

    public override string ToString()
    {
        return BuildRepresentativeString();
    }

    private string BuildRepresentativeString(string separator = ",", Func<T, string>? elementFormatter = null)
    {
        var elements = this.Select(element => elementFormatter != null
            ? elementFormatter(element)
            : element?.ToString() ?? string.Empty);

        return string.Join(separator, elements);
    }

    private int InternalIndex(int index)
    {
        var offset = _readIndex + index < Capacity
            ? index
            : index - Capacity;

        return _readIndex + offset;
    }

    private void IncrementIndex(ref int index)
    {
        if (++index == Capacity)
        {
            index = 0;
        }
    }

    private void ValidateIndex(int index)
    {
        ThrowIfEmpty();
        if (index < 0 || index >= Count)
        {
            throw ThrowHelper.IndexInvalid(index, Count);
        }
    }

    private void ThrowIfEmpty()
    {
        if (IsEmpty)
        {
            throw ThrowHelper.BufferEmpty();
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        var segments = ToArraySegments();
        foreach (var segment in segments)
        {
            for (var i = 0; i < segment.Count; i++)
            {
                yield return segment.Array![segment.Offset + i];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    ///     The buffer content consists of up to two non-contiguous segments: [d,e,...,a,b,c]
    ///     <para />
    ///     This method handles retrieving these segments for performant enumeration
    /// </summary>
    private IEnumerable<ArraySegment<T>> ToArraySegments()
    {
        yield return ArrayOne();
        yield return ArrayTwo();
    }

    /// <summary>
    ///     The buffer content consists of at most two non-contiguous segments: [d,e,...,a,b,c]
    /// </summary>
    /// <returns>The array segment starting at the head/read index: [a,b,c]</returns>
    private ArraySegment<T> ArrayOne()
    {
        if (IsEmpty)
        {
            return new ArraySegment<T>([]);
        }

        return _readIndex < _writeIndex
            ? new ArraySegment<T>(_buffer, offset: _readIndex, count: _writeIndex - _readIndex)
            : new ArraySegment<T>(_buffer, offset: _readIndex, count: _buffer.Length - _readIndex);
    }

    /// <summary>
    ///     The buffer content consists of at most two non-contiguous segments:  [d,e,...,a,b,c]
    /// </summary>
    /// <returns>The array segment starting at the buffer start up to the write index: [d,e]</returns>
    private ArraySegment<T> ArrayTwo()
    {
        if (IsEmpty)
        {
            return new ArraySegment<T>(array: []);
        }

        return _readIndex < _writeIndex
            ? new ArraySegment<T>(_buffer, offset: _writeIndex, count: 0)
            : new ArraySegment<T>(_buffer, offset: 0, count: _writeIndex);
    }

    private static class ThrowHelper
    {
        private const string EmptyMessage = "Cannot access an empty buffer";
        private const string ItemsNullMessage = "Items must be non-null";
        private const string CapacityInvalidMessage = "Capacity must be a positive, non-zero number";
        private const string CapacityExceededFormat = "Capacity exceeded: {0} is greater than the buffer size of {1}";
        private const string IndexInvalidFormat = "Cannot access index {0}, buffer size is {1}";

        public static InvalidOperationException BufferEmpty()
        {
            return new InvalidOperationException(message: EmptyMessage);
        }

        public static ArgumentException ItemsNull()
        {
            return new ArgumentException(message: ItemsNullMessage);
        }

        public static ArgumentException CapacityInvalid()
        {
            return new ArgumentException(message: CapacityInvalidMessage);
        }

        public static ArgumentException CapacityExceeded(int length, int capacity)
        {
            return new ArgumentException(message: string.Format(CapacityExceededFormat, length, capacity));
        }

        public static IndexOutOfRangeException IndexInvalid(int index, int count)
        {
            return new IndexOutOfRangeException(message: string.Format(IndexInvalidFormat, index, count));
        }
    }
}