namespace Problems.Y2016.D10;

public sealed class Node
{
    public string Id { get; }
    public string? Low { get; init; }
    public string? High { get; init; }
    public HashSet<int> Values { get; } = new(capacity: 2);

    public bool Ready => Values.Count == 2;
    
    public Node(string id)
    {
        Id = id;
    }
}