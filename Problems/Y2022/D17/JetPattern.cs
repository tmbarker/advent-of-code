using Utilities.Cartesian;

namespace Problems.Y2022.D17;

public class JetPattern
{
    private static readonly Dictionary<char, Vector2D> JetVectorMap = new()
    {
        { '>', Vector2D.Right },
        { '<', Vector2D.Left },
    };

    private readonly Queue<Vector2D> _queue = new();

    private JetPattern(IEnumerable<Vector2D> vectors)
    {
        foreach (var vector in vectors)
        {
            _queue.Enqueue(vector);   
        }
    }

    public Vector2D Next()
    {
        var vector = _queue.Dequeue();
        _queue.Enqueue(vector);
        return vector;
    }

    public static JetPattern Parse(string input)
    {
        return new JetPattern(input.Select(c => JetVectorMap[c]).ToList());
    }
}