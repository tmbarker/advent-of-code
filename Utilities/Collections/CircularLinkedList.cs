namespace Utilities.Collections;

/// <summary>
/// A doubly linked circular linked list. The implementation/members are the same as the .NET standard library
/// <see cref="LinkedList{T}"/>, except it is circular
/// </summary>
/// <typeparam name="T">The type of the value encapsulated in each <see cref="CircularLinkedListNode{T}"/></typeparam>
public sealed class CircularLinkedList<T>
{
   public CircularLinkedList()
   {
   }

   public CircularLinkedList(IEnumerable<T> collection)
   {
      AddRange(collection);
   }

   public int Count { get; private set; }
   public CircularLinkedListNode<T>? Head { get; private set; }
   public CircularLinkedListNode<T>? Tail => Head?.Prev;
   
   public void MarkHead(CircularLinkedListNode<T> newHead)
   {
      Head = newHead;
   }
   
   public void Add(T value)
   {
      AddLast(value);
   }

   /// <summary>
   /// Add each element in the <see cref="collection"/> one by one in front of the <see cref="Head"/>  
   /// </summary>
   /// <param name="collection"></param>
   public void AddRange(IEnumerable<T> collection)
   {
      foreach (var value in collection)
      {
         AddLast(value);
      }
   }
   
   public CircularLinkedListNode<T> AddAfter(CircularLinkedListNode<T> node, T value)
   {
      var newNode = new CircularLinkedListNode<T>(node.List!, value);
      InternalInsertNodeBefore(node.Next!, newNode);
      return newNode;
   }

   public CircularLinkedListNode<T> AddAfter(CircularLinkedListNode<T> node, CircularLinkedListNode<T> newNode)
   {
      if (newNode.List != null)
      {
         throw new InvalidOperationException($"Cannot insert node, it is already in a {nameof(CircularLinkedList<T>)}");
      }

      newNode.List = node.List!;
      InternalInsertNodeBefore(node.Next!, newNode);
      return newNode;
   }

   public CircularLinkedListNode<T> AddBefore(CircularLinkedListNode<T> node, T value)
   {
      var newNode = new CircularLinkedListNode<T>(node.List!, value);
      InternalInsertNodeBefore(node, newNode);
      return newNode;
   }
   
   public CircularLinkedListNode<T> AddBefore(CircularLinkedListNode<T> node, CircularLinkedListNode<T> newNode)
   {
      if (newNode.List != null)
      {
         throw new InvalidOperationException($"Cannot insert node, it is already in a {nameof(CircularLinkedList<T>)}");
      }

      newNode.List = node.List!;
      InternalInsertNodeBefore(node, newNode);
      return newNode;
   }

   /// <summary>
   /// Add the value to the <see cref="CircularLinkedList{T}"/>, it will become the <see cref="Head"/>
   /// </summary>
   public CircularLinkedListNode<T> AddFirst(T value)
   {
      var node = new CircularLinkedListNode<T>(this, value);
      if (Head == null)
      {
         InternalInsertNodeToEmptyList(node);
      }
      else
      {
         InternalInsertNodeBefore(Head, node);
         Head = node;
      }
      return node;
   }

   /// <summary>
   /// Add the value to the <see cref="CircularLinkedList{T}"/>, it will become the <see cref="Tail"/>
   /// </summary>
   public CircularLinkedListNode<T> AddLast(T value)
   {
      var node = new CircularLinkedListNode<T>(this, value);
      if (Head == null)
      {
         InternalInsertNodeToEmptyList(node);
      }
      else
      {
         InternalInsertNodeBefore(Head, node);
      }
      return node;
   }

   /// <summary>
   /// Linearly traverse from the <see cref="Head"/> node
   /// </summary>
   public CircularLinkedListNode<T> GetNode(int index)
   {
      if (index < 0 || index >= Count)
      {
         throw new ArgumentOutOfRangeException(nameof(index));
      }
      
      var current = Head!;
      for (var i = 0; i < index; i++)
      {
         current = current.Next!;
      }

      return current;
   }

   /// <summary>
   /// Linearly search from the <see cref="Head"/> node
   /// </summary>
   public CircularLinkedListNode<T>? FindNode(T value, out int index)
   {
      var current = Head;
      for (var i = 0; i < Count && current != null; i++)
      {
         if (EqualityComparer<T>.Default.Equals(x: current.Value, y: value))
         {
            index = i;
            return current;
         }

         current = current.Next;
      }

      index = -1;
      return null;
   }

   public void Remove(CircularLinkedListNode<T> node)
   {
      InternalRemoveNode(node);
   }

   public void RemoveHead()
   {
      if (Head == null)
      {
         throw new InvalidOperationException();
      }
      
      InternalRemoveNode(Head);
   }

   public void Clear()
   {
      var current = Head;
      while (current != null)
      {
         var temp = current;
         current = current.Next;
         temp.Invalidate();
      }

      Head = null;
      Count = 0;
   }

   public void Reverse(bool preserveHead)
   {
      var temp = default(CircularLinkedListNode<T>?);
      var current = Head;

      for (var i = 0; i < Count && current != null; i++)
      {
         temp = current.Prev!;
         current.Prev = current.Next;
         current.Next = temp;
         current = current.Prev!;
      }

      if (temp != null && !preserveHead)
      {
         Head = temp.Prev;  
      }
   }

   /// <summary>
   /// Reverse a sub-range of the list, note that <see cref="Head"/> is preserved
   /// </summary>
   /// <param name="start">The first node in the reversed segment of the list</param>
   /// <param name="count">The number of nodes to be reversed</param>
   /// <param name="preserveHead">Do not modify the <see cref="Head"/> reference</param>
   /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"></paramref> is negative</exception>
   /// <exception cref="ArgumentException"><paramref name="count"/> is greater than the length of the list</exception>
   public void ReverseRange(CircularLinkedListNode<T> start, int count, bool preserveHead)
   {
      if (count < 0)
      {
         throw new ArgumentOutOfRangeException(nameof(count));
      }

      if (count > Count)
      {
         throw new ArgumentException(
            message: $"{nameof(count)} must be less than or equal to the length of the list",
            paramName: nameof(count));
      }

      var cachedHead = Head;
      if (count == Count)
      {
         Reverse(preserveHead);
      }
      else
      {
         InternalReverseRange(start, count);
      }

      if (preserveHead)
      {
         Head = cachedHead;  
      }
   }

   public void Print(string separator = ", ", Func<T, string>? nodeFormatter = null)
   {
      Console.WriteLine(BuildRepresentativeString(separator, nodeFormatter));
   }
   
   public string BuildRepresentativeString(string separator = ", ", Func<T, string>? nodeFormatter = null)
   {
      var elements = new List<string>();
      var current = Head;

      for (var i = 0; i < Count && current != null; i++)
      {
         var elementString = nodeFormatter != null
            ? nodeFormatter(current.Value)
            : current.Value?.ToString() ?? string.Empty;

         elements.Add(elementString);
         current = current.Next;
      }

      return string.Join(separator, elements);
   }

   private void InternalInsertNodeToEmptyList(CircularLinkedListNode<T> newNode)
   {
      newNode.Next = newNode;
      newNode.Prev = newNode;
      Head = newNode;
      Count++;
   }

   private void InternalInsertNodeBefore(CircularLinkedListNode<T> node, CircularLinkedListNode<T> newNode)
   {
      newNode.Next = node;
      newNode.Prev = node.Prev;
      node.Prev!.Next = newNode;
      node.Prev = newNode;
      Count++;
   }
   
   private void InternalRemoveNode(CircularLinkedListNode<T> node)
   {
      if (node.Next == node)
      {
         Head = null;
      }
      else
      {
         node.Next!.Prev = node.Prev;
         node.Prev!.Next = node.Next;

         if (Head == node)
         {
            Head = node.Next;
         }
      }
      
      node.Invalidate();
      Count--;
   }

   private void InternalReverseRange(CircularLinkedListNode<T> start, int count)
   {
      var removeAfter = start.Prev!;
      var current = start;
      var queue = new Queue<CircularLinkedListNode<T>>();

      for (var i = 0; i < count; i++)
      {
         var next = current.Next;
         queue.Enqueue(current);
         InternalRemoveNode(current);
         current = next!;
      }
      
      while (queue.Count != 0)
      {
         var insertBefore = removeAfter.Next!;
         var insertNode = queue.Dequeue();
         InternalInsertNodeBefore(insertBefore, insertNode);
      }
   }
}