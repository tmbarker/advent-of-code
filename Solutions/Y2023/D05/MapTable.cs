namespace Solutions.Y2023.D05;

public sealed class MapTable
{
    public IReadOnlyList<MapEntry> OrderedEntries { get; }

    private MapTable(IEnumerable<MapEntry> orderedEntries)
    {
        OrderedEntries = new List<MapEntry>(collection: orderedEntries);
    }

    public static MapTable Build(IEnumerable<MapEntry> entries)
    {
        var queue = new Queue<MapEntry>(collection: entries.OrderBy(mapping => mapping.SourceMin));
        var order = new List<MapEntry>();
        var upper = -1L;
        
        while (queue.Count != 0)
        {
            var next = queue.Dequeue();
            if (upper + 1 != next.SourceMin)
            {
                order.Add(item: MapEntry.Default(
                    min: upper + 1,
                    max: next.SourceMin - 1));
            }
                
            order.Add(next);
            upper = order[^1].SourceMax;
        }
            
        //  Append with [Max(ranges.Max) + 1, long.MaxValue]
        //
        if (order[^1].SourceMax != long.MaxValue)
        {
            order.Add(item: MapEntry.Default(
                min: order[^1].SourceMax + 1,
                max: long.MaxValue));
        }

        return new MapTable(orderedEntries: order);
    }
}