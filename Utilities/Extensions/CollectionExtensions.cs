using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Utilities.Geometry.Euclidean;

namespace Utilities.Extensions;

public static class CollectionExtensions
{
    /// <summary>
    ///     Filter the dictionary entries using a Value Predicate.
    /// </summary>
    public static Dictionary<TKey, TValue> WhereValues<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
        Predicate<TValue> predicate) where TKey : notnull
    {
        return dictionary
            .Where(kvp => predicate(kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    /// <summary>
    ///     Initialize a <see cref="IReadOnlyCollection{T}" /> using the elements from <paramref name="source" />.
    /// </summary>
    public static IReadOnlyCollection<T> Freeze<T>(this IEnumerable<T> source)
    {
        return new List<T>(source);
    }

    /// <summary>
    ///     Remove a single element from <paramref name="source" />, if it exists.
    /// </summary>
    public static ICollection<T> Except<T>(this IEnumerable<T> source, T single)
    {
        return new List<T>(collection: source.Except([single]));
    }

    /// <summary>
    ///     Find the intersection of multiple collections.
    /// </summary>
    /// <param name="collections">The collections to intersect</param>
    /// <returns>A set containing the elements which are present in all provided collections</returns>
    public static ISet<T> IntersectAll<T>(this IEnumerable<IEnumerable<T>> collections)
    {
        var enumerable = collections.ToList();
        var presentInAll = enumerable
            .Skip(1)
            .Aggregate(
                seed: new HashSet<T>(collection: enumerable.First()),
                func: (inAll, nextCollection) =>
                {
                    inAll.IntersectWith(nextCollection);
                    return inAll;
                }
            );

        return presentInAll;
    }

    /// <summary>
    ///     Normalize the values of the collection such that the "smallest" becomes <see cref="Vec2D.Zero" />.
    /// </summary>
    public static IEnumerable<Vec2D> Normalize(this IEnumerable<Vec2D> collection)
    {
        var enumerated = collection.ToList();
        var dx = int.MaxValue;
        var dy = int.MaxValue;

        foreach (var vector in enumerated)
        {
            dx = Math.Min(dx, vector.X);
            dy = Math.Min(dy, vector.Y);
        }

        var delta = new Vec2D(dx, dy);
        foreach (var vector in enumerated)
        {
            yield return vector - delta;
        }
    }

    /// <summary>
    ///     Return the mode of the collection, i.e. the element with the highest occurence frequency.
    /// </summary>
    public static T Mode<T>(this IEnumerable<T> collection) where T : notnull
    {
        return collection
            .GroupBy(e => e)
            .MaxBy(g => g.Count())!.Key;
    }

    /// <summary>
    ///     Chunk the source based on a <paramref name="takePredicate" /> and an
    ///     optional <paramref name="skipPredicate" />.
    /// </summary>
    /// <param name="source">The source collection</param>
    /// <param name="takePredicate">Select which elements should be included in a chunk</param>
    /// <param name="skipPredicate">
    ///     Select which elements to skip between chunks, if not provided
    ///     then <paramref name="takePredicate" /> is used and negated
    /// </param>
    /// <typeparam name="T">The type associated with each element in the source</typeparam>
    /// <returns>An iterator which yields chunk arrays</returns>
    public static IEnumerable<T[]> ChunkBy<T>(this IEnumerable<T> source, Predicate<T> takePredicate,
        Predicate<T>? skipPredicate = null)
    {
        var chunks = new List<T[]>();
        var enumerable = source as IList<T> ?? source.ToArray();

        for (var i = 0; i < enumerable.Count;)
        {
            var chunk = enumerable
                .Skip(i)
                .TakeWhile(takePredicate.Invoke)
                .ToArray();

            if (chunk.Length > 0)
            {
                chunks.Add(chunk);
            }

            i += chunk.Length;
            i += enumerable
                .Skip(i)
                .TakeWhile(element => skipPredicate?.Invoke(element) ?? !takePredicate.Invoke(element))
                .Count();
        }

        return chunks;
    }

    /// <summary>
    ///     Chunk the source between null or whitespace strings.
    /// </summary>
    /// <param name="source">The source collection</param>
    /// <returns>An iterator which yields chunk arrays</returns>
    public static IEnumerable<string[]> ChunkByNonEmpty(this IEnumerable<string> source)
    {
        return source.ChunkBy(takePredicate: s => !string.IsNullOrWhiteSpace(s));
    }

    /// <summary>
    ///     Apply an action to each element in the <paramref name="source" /> collection.
    /// </summary>
    /// <param name="source">The source collection to act upon</param>
    /// <param name="action">The <see cref="Action{T}" /> to apply to each element</param>
    /// <typeparam name="T">The type associated with each element in the <paramref name="source" /> collection</typeparam>
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var element in source)
        {
            action.Invoke(element);
        }
    }

    /// <summary>
    ///     Returns all permutations of the source sequence elements.
    /// </summary>
    public static IEnumerable<IEnumerable<T>> Permute<T>(this IEnumerable<T> elements)
    {
        var enumerated = elements.ToArray();
        if (enumerated.Length == 0)
        {
            yield return [];
            yield break;
        }

        var startingElementIndex = 0;
        foreach (var startingElement in enumerated)
        {
            var index = startingElementIndex;
            var remainingItems = enumerated.Where((_, i) => i != index);

            foreach (var permutationOfRemainder in remainingItems.Permute())
            {
                yield return permutationOfRemainder.Prepend(startingElement);
            }

            startingElementIndex++;
        }
    }

    /// <summary>
    ///     Return all combinations of the source sequence elements of size <paramref name="k" />.
    /// </summary>
    /// <param name="elements">The source sequence</param>
    /// <param name="k">The number of elements in each returned combination</param>
    public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> elements, int k)
    {
        if (k < 0)
        {
            throw new InvalidOperationException();
        }
        
        ArgumentNullException.ThrowIfNull(elements);

        var enumerated = elements as T[] ?? elements.ToArray();
        return
            from combination in Combinations(n: enumerated.Length, k)
            select enumerated.ZipWhere(combination);
    }

    /// <summary>
    ///     Takes two numbers non-negative numbers, <paramref name="n" /> and <paramref name="k" />, and
    ///     produces all sequences of n bits with k true bits and n-k false bits.
    /// </summary>
    /// <param name="n">The total number of bits in each sequence</param>
    /// <param name="k">The number of set bits in each sequence</param>
    private static IEnumerable<ImmutableStack<bool>> Combinations(int n, int k)
    {
        if (k == 0 && n == 0)
        {
            yield return ImmutableStack<bool>.Empty;
            yield break;
        }

        if (n < k)
        {
            yield break;
        }

        //  First produce all the sequences that start with true,
        //  and then all the sequences that start with false.
        //
        //  At least one of n or k is not zero, and 0 <= k <= n,
        //  therefore n is not zero. But k could be.
        //
        if (k > 0)
        {
            foreach (var r in Combinations(n: n - 1, k: k - 1))
            {
                yield return r.Push(true);
            }
        }

        foreach (var r in Combinations(n: n - 1, k: k))
        {
            yield return r.Push(false);
        }
    }

    /// <summary>
    ///     Filters a sequence of items based on a corresponding sequence of boolean selectors.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <paramref name="items" /> sequence</typeparam>
    /// <param name="items">The sequence of items to filter</param>
    /// <param name="selectors">
    ///     A sequence of boolean values that determines whether each corresponding element in <paramref name="items" />
    ///     is included in the result. An element is included if the corresponding selector is <c>true</c>
    /// </param>
    /// <returns>
    ///     An <see cref="IEnumerable{T}" /> containing elements from <paramref name="items" /> where the corresponding
    ///     value in <paramref name="selectors" /> is <c>true</c>
    /// </returns>
    public static IEnumerable<T> ZipWhere<T>(this IEnumerable<T> items, IEnumerable<bool> selectors)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(selectors);
        
        return ZipWhereIterator(items, selectors);
    }

    private static IEnumerable<T> ZipWhereIterator<T>(IEnumerable<T> items, IEnumerable<bool> selectors)
    {
        using var itemEnumerator = items.GetEnumerator();
        using var selectorEnumerator = selectors.GetEnumerator();

        while (itemEnumerator.MoveNext() && selectorEnumerator.MoveNext())
        {
            if (selectorEnumerator.Current)
            {
                yield return itemEnumerator.Current;
            }
        }
    }
    
    /// <summary>
    ///     Determines whether the sequence contains exactly one element that matches the specified predicate.
    /// </summary>
    /// <param name="source">The source collection</param>
    /// <param name="predicate">The predicate to invoke on each source element</param>
    /// <param name="element">When successful, the single element which passes the predicate</param>
    /// <returns>A <see cref="bool"/> representing the success of the query</returns>
    public static bool HasExactlyOne<T>(this IEnumerable<T> source, Func<T, bool> predicate,
        [NotNullWhen(returnValue: true)] out T? element)
    {
        var candidates = source.Where(predicate).Take(2).ToArray();
        if (candidates.Length == 1)
        {
            element = candidates.Single()!;
            return true;
        }
        
        element = default;
        return false;
    }
}