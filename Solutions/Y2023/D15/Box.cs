namespace Solutions.Y2023.D15;

public readonly struct Box(int n)
{
    private Dictionary<string, LinkedListNode<int>> Index { get; } = new();
    private LinkedList<int> Lenses { get; } = [];

    public void Remove(string label)
    {
        if (Index.Remove(label, out var lens))
        {
            Lenses.Remove(lens);   
        }
    }

    public void Add(string label, int focalLength)
    {
        if (Index.TryGetValue(label, out var oldLens))
        {
            oldLens.Value = focalLength;
        }
        else
        {
            Lenses.AddLast(value: focalLength);
            Index[label] = Lenses.Last!;
        }
    }
    
    public int GetPower()
    {
        var sum = 0;
        var i = 0;
        var lens = Lenses.First;
            
        while (lens != null)
        {
            sum += (n + 1) * (i++ + 1) * lens.Value;
            lens = lens.Next;
        }

        return sum;
    }
}