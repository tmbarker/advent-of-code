using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D17;

public sealed class JetPattern
{
    private static readonly Dictionary<char, Vec2D> JetVectorMap = new()
    {
        { '>', Vec2D.Right },
        { '<', Vec2D.Left }
    };


    private readonly List<Vec2D> _list;

    public int Index { get; private set; }

    private JetPattern(IEnumerable<Vec2D> vectors)
    {
        Index = 0;
        _list = new List<Vec2D>(vectors);
    }

    public Vec2D Next()
    {
        var next = _list[Index % _list.Count];
        Index = (Index + 1) % _list.Count;
        return next;
    }

    public static JetPattern Parse(string sequence)
    {
        return new JetPattern(sequence.Select(c => JetVectorMap[c]));
    }
}