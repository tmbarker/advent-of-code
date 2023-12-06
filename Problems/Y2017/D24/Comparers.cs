namespace Problems.Y2017.D24;

public sealed class StrengthComparer : IComparer<Bridge>
{
    public int Compare(Bridge x, Bridge y)
    {
        return x.Strength.CompareTo(y.Strength);
    }
}

public sealed class LengthComparer : IComparer<Bridge>
{
    public int Compare(Bridge x, Bridge y)
    {
        return x.Length != y.Length
            ? x.Length.CompareTo(y.Length)
            : x.Strength.CompareTo(y.Length);
    }
}