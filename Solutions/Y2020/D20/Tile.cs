using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D20;

public sealed class Tile
{
    public readonly record struct Congruence(EdgeId FromEdge, EdgeId ToEdge, int ToTile);
    public enum EdgeId
    {
        A,
        B,
        C,
        D
    }
    
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
            { EdgeId.A, new EdgeFingerprint(GetEdgeString(EdgeId.A)) },
            { EdgeId.B, new EdgeFingerprint(GetEdgeString(EdgeId.B)) },
            { EdgeId.C, new EdgeFingerprint(GetEdgeString(EdgeId.C)) },
            { EdgeId.D, new EdgeFingerprint(GetEdgeString(EdgeId.D)) }
        };
    }

    public char this[int x, int y] => _pixels[x, y];

    public void OrientToMatch(EdgeId matchEdge, EdgeId toOtherEdge, Tile onOtherTile)
    {
        var requiredEdgeDir = -1 * onOtherTile.EdgeDirections[toOtherEdge];
        while (EdgeDirections[matchEdge] != requiredEdgeDir)
        {
            Rotate(thetaDeg: Degrees.P90);
        }
        
        if (GetEdgeString(matchEdge) == onOtherTile.GetEdgeString(toOtherEdge))
        {
            return;
        }
        
        Flip();
        while (EdgeDirections[matchEdge] != requiredEdgeDir)
        {
            Rotate(thetaDeg: Degrees.P90);
        }
    }

    private string GetEdgeString(EdgeId edge)
    {
        return _edgeStringGetters[EdgeDirections[edge]].Invoke();
    }
    
    private void Rotate(int thetaDeg)
    {
        _pixels.Rotate(thetaDeg);
        foreach (var (edgeId, edgeDirection) in EdgeDirections)
        {
            EdgeDirections[edgeId] = Rot3D.FromAxisAngle(Axis.Z, thetaDeg).Transform(edgeDirection);
        }
    }
    
    private void Flip()
    {
        _pixels.Flip(about: Axis.Y);
        foreach (var (edgeId, edgeDirection) in EdgeDirections)
        {
            EdgeDirections[edgeId] = edgeDirection with { X = -edgeDirection.X };
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
}