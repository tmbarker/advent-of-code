namespace Problems.Y2015.D24;

public sealed class GroupComparer : IComparer<HashSet<long>?>
{
    public static GroupComparer Instance { get; } = new();
    
    private GroupComparer()
    {
    }
    
    public int Compare(HashSet<long>? x, HashSet<long>? y)
    {
        if (x!.Count != y!.Count)
        {
            return x.Count.CompareTo(y.Count);
        }

        var xS = x.Aggregate(seed: 1L, (i, j) => i * j);
        var yS = y.Aggregate(seed: 1L, (i, j) => i * j);

        return xS.CompareTo(yS);
    }
}