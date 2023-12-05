namespace Problems.Y2023.D05;

public class MapTable
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
        var head = queue.Peek();

        //  Prepend with [0, Min(ranges.Min) - 1]
        //
        if (head.SourceMin != 0)
        {
            order.Add(item: MapEntry.Default(
                min: 0,
                max: head.SourceMin - 1));
        }
        else
        {
            order.Add(item: queue.Dequeue());
        }
            
        while (queue.Any())
        {
            var next = queue.Dequeue();
            var upper = order[^1].SourceMax;
                
            if (upper + 1 != next.SourceMin)
            {
                order.Add(item: MapEntry.Default(
                    min: upper + 1,
                    max: next.SourceMin - 1));
            }
                
            order.Add(next);
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