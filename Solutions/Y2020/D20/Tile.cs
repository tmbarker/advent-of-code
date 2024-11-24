using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D20;

public sealed class Tile
{
    private const int BorderWidth = 1;
    private readonly Grid2D<char> _pixels;
    private readonly Dictionary<Vec2D, Func<string>> _edgeStringGetters;

    public Dictionary<EdgeId, Vec2D> EdgeDirections { get; }
    public Dictionary<EdgeId, EdgeFingerprint> EdgeFingerprints { get; }
    public int ContentSize => _pixels.Height - 2 * BorderWidth;

    public Tile(Grid2D<char> pixels)
    {
        _pixels = pixels;
        _edgeStringGetters = BuildEdgeStringGetters();
        
        EdgeDirections = new Dictionary<EdgeId, Vec2D>
        {
            { EdgeId.A, Vec2D.Down },
            { EdgeId.B, Vec2D.Left },
            { EdgeId.C, Vec2D.Up },
            { EdgeId.D, Vec2D.Right }
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
            Rotate(rot: Rot3D.P90Z);
        }
        
        if (GetEdgeString(matchEdge) == onOtherTile.GetEdgeString(toOtherEdge))
        {
            return;
        }
        
        Flip();
        while (EdgeDirections[matchEdge] != requiredEdgeDir)
        {
            Rotate(rot: Rot3D.P90Z);
        }
    }

    private string GetEdgeString(EdgeId edge)
    {
        return _edgeStringGetters[EdgeDirections[edge]].Invoke();
    }
    
    private void Rotate(Rot3D rot)
    {
        _pixels.Rotate(rot.ThetaDeg);
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
            EdgeDirections[edgeId] = new Vec2D(
                X: -edgeDirection.X,
                Y: edgeDirection.Y);
        }
    }

    private Dictionary<Vec2D, Func<string>> BuildEdgeStringGetters()
    {
        return new Dictionary<Vec2D, Func<string>>
        {
            {Vec2D.Up,    () => string.Concat(_pixels.EnumerateRow(_pixels.Height - 1))},
            {Vec2D.Down,  () => string.Concat(_pixels.EnumerateRow(0))},
            {Vec2D.Left,  () => string.Concat(_pixels.EnumerateCol(0))},
            {Vec2D.Right, () => string.Concat(_pixels.EnumerateCol(_pixels.Width - 1))}
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