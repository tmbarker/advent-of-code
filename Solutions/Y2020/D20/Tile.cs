using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D20;

public sealed class Tile
{
    private const int BorderWidth = 1;
    private readonly Grid2D<char> _pixels;
    private readonly Dictionary<Vector2D, Func<string>> _edgeStringGetters;

    public Dictionary<EdgeId, Vector2D> EdgeDirections { get; }
    public Dictionary<EdgeId, EdgeFingerprint> EdgeFingerprints { get; }
    public int ContentSize => _pixels.Height - 2 * BorderWidth;

    public Tile(Grid2D<char> pixels)
    {
        _pixels = pixels;
        _edgeStringGetters = BuildEdgeStringGetters();
        
        EdgeDirections = new Dictionary<EdgeId, Vector2D>
        {
            { EdgeId.A, Vector2D.Down },
            { EdgeId.B, Vector2D.Left },
            { EdgeId.C, Vector2D.Up },
            { EdgeId.D, Vector2D.Right }
        };
        EdgeFingerprints = new Dictionary<EdgeId, EdgeFingerprint>
        {
            { EdgeId.A, new(GetEdgeString(EdgeId.A)) },
            { EdgeId.B, new(GetEdgeString(EdgeId.B)) },
            { EdgeId.C, new(GetEdgeString(EdgeId.C)) },
            { EdgeId.D, new(GetEdgeString(EdgeId.D)) }
        };
    }

    public char this[int x, int y] => _pixels[x, y];

    public void OrientToMatch(EdgeId matchEdge, EdgeId toOtherEdge, Tile onOtherTile)
    {
        var requiredEdgeDir = -1 * onOtherTile.EdgeDirections[toOtherEdge];
        while (EdgeDirections[matchEdge] != requiredEdgeDir)
        {
            Rotate(rot: Rotation3D.Positive90Z);
        }
        
        if (GetEdgeString(matchEdge) == onOtherTile.GetEdgeString(toOtherEdge))
        {
            return;
        }
        
        Flip();
        while (EdgeDirections[matchEdge] != requiredEdgeDir)
        {
            Rotate(rot: Rotation3D.Positive90Z);
        }
    }

    private string GetEdgeString(EdgeId edge)
    {
        return _edgeStringGetters[EdgeDirections[edge]].Invoke();
    }
    
    private void Rotate(Rotation3D rot)
    {
        _pixels.Rotate(rot);
        foreach (var (edgeId, edgeDirection) in EdgeDirections)
        {
            EdgeDirections[edgeId] = rot * edgeDirection;
        }
    }
    
    private void Flip()
    {
        _pixels.Flip(about: Axis.Y);
        foreach (var (edgeId, edgeDirection) in EdgeDirections)
        {
            EdgeDirections[edgeId] = new Vector2D(
                x: -edgeDirection.X,
                y: edgeDirection.Y);
        }
    }

    private Dictionary<Vector2D, Func<string>> BuildEdgeStringGetters()
    {
        return new Dictionary<Vector2D, Func<string>>
        {
            {Vector2D.Up,    () => string.Concat(_pixels.EnumerateRow(_pixels.Height - 1))},
            {Vector2D.Down,  () => string.Concat(_pixels.EnumerateRow(0))},
            {Vector2D.Left,  () => string.Concat(_pixels.EnumerateCol(0))},
            {Vector2D.Right, () => string.Concat(_pixels.EnumerateCol(_pixels.Width - 1))}
        };
    }
    
    public readonly record struct Congruence(EdgeId FromEdge, EdgeId ToEdge, int ToTile);
    public enum EdgeId
    {
        A,
        B,
        C,
        D
    }
}