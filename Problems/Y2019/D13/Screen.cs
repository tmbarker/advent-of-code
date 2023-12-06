using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2019.D13;

public sealed class Screen
{
    private static readonly Vector2D ScoreKey = new(-1, 0);
    private static readonly Dictionary<long, GameObject> GobCodes = new()
    {
        { 0L, GameObject.Empty },
        { 1L, GameObject.Wall },
        { 2L, GameObject.Block },
        { 3L, GameObject.Paddle },
        { 4L, GameObject.Ball }
    };

    private readonly Dictionary<Vector2D, GameObject> _pixels = new();

    public long Score { get; private set; }
    public Vector2D Ball { get; private set; } = Vector2D.Zero;
    public Vector2D Paddle { get; private set; } = Vector2D.Zero;

    public Screen(Queue<long> machineOutput)
    {
        UpdatePixels(machineOutput);
    }

    public void UpdatePixels(Queue<long> machineOutput)
    {
        while (machineOutput.Any())
        {
            ParsePixel(machineOutput);
        }
    }
    
    public int GetCount(GameObject type)
    {
        return _pixels.WhereValues(t => t == type).Count;
    }

    private void ParsePixel(Queue<long> machineOutput)
    {
        var pos = new Vector2D(
            x: (int)machineOutput.Dequeue(),
            y: (int)machineOutput.Dequeue());

        if (pos == ScoreKey)
        {
            Score = machineOutput.Dequeue();
            return;
        }
        
        _pixels[pos] = GobCodes[machineOutput.Dequeue()];
        
        if (_pixels[pos] == GameObject.Ball)
        {
            Ball = pos;
        }
        if (_pixels[pos] == GameObject.Paddle)
        {
            Paddle = pos;
        }
    }
    
    public void Print((int Left, int Top) drawAt)
    {
        var bounds = new Aabb2D(extents: _pixels.Keys);
        var rows = bounds.Height;
        var cols = bounds.Width;
        var grid = Grid2D<GameObject>.WithDimensions(rows, cols, Origin.Uv);
        
        foreach (var (pixel, gobType) in _pixels)
        {
            grid[pixel] = gobType;
        }

        Console.SetCursorPosition(drawAt.Left, drawAt.Top);
        grid.Print(padding: 0, elementFormatter: (_, gameObject) =>
        {
            return gameObject switch
            {
                GameObject.Wall => "▓",
                GameObject.Block => "#",
                GameObject.Paddle => "─",
                GameObject.Empty => " ",
                GameObject.Ball => "O",
                _ => throw new ArgumentOutOfRangeException(nameof(gameObject))
            };
        });
    }
}