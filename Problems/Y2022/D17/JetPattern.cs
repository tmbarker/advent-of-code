using Utilities.Cartesian;

namespace Problems.Y2022.D17;

public class JetPattern
{
    private static readonly Dictionary<char, Vector2D> JetVectorMap = new()
    {
        { '>', Vector2D.Right },
        { '<', Vector2D.Left }
    };


    private readonly List<Vector2D> _list;

    public int Index { get; private set; }

    private JetPattern(IEnumerable<Vector2D> vectors)
    {
        Index = 0;
        _list = new List<Vector2D>(vectors);
    }

    public Vector2D Next()
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