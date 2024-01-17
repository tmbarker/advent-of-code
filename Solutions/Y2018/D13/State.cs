using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D13;

public sealed class State
{
    private static readonly IComparer<Vec2D> TickOrderComparer = new TickOrderComparer();

    public Dictionary<int, Vec2D> Positions { get; }
    public Dictionary<int, Vec2D> Facings { get; }
    public Dictionary<int, int> NextTurns { get; }

    public IEnumerable<int> GetCurrentOrder()
    {
        return Positions.Keys.OrderBy(id => Positions[id], TickOrderComparer);
    }

    public void RemoveCart(int id)
    {
        Positions.Remove(id);
        Facings.Remove(id);
        NextTurns.Remove(id);
    }
    
    public static State Parse(IList<string> trackElements)
    {
        var positions = new Dictionary<int, Vec2D>();
        var facings = new Dictionary<int, Vec2D>();
        var nextTurns = new Dictionary<int, int>();

        for (var y = 0; y < trackElements.Count; y++)
        for (var x = 0; x < trackElements[0].Length; x++)
        {
            var id = positions.Count;
            var element = trackElements[y][x];
            
            if (!Track.CartFacings.ContainsKey(element))
            {
                continue;
            }
            
            positions.Add(id, new Vec2D(x, y));
            facings.Add(id, Track.CartFacings[element]);
            nextTurns.Add(id, 0);
        }

        return new State(
            positions: positions,
            facings: facings,
            nextTurns: nextTurns);
    }

    private State(
        Dictionary<int, Vec2D> positions, 
        Dictionary<int, Vec2D> facings,
        Dictionary<int, int> nextTurns)
    {
        Positions = positions;
        Facings = facings;
        NextTurns = nextTurns;
    }
}