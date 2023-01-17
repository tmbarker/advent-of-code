namespace Problems.Y2020.D20;

public readonly struct EdgeFingerprint
{
    private readonly string _forwards;
    private readonly string _backwards;

    public EdgeFingerprint(string edgeString)
    {
        _forwards = edgeString;
        _backwards = string.Concat(_forwards.Reverse());
    }

    public bool IsCongruentTo(EdgeFingerprint other)
    {
        return _forwards == other._forwards || _forwards == other._backwards;
    }
}