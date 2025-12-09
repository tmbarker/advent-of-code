using System.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2020.D11;

public sealed class SeatMap : IEnumerable<KeyValuePair<Vec2D, bool>>
{
    private readonly IDictionary<Vec2D, bool> _occupancyMap;
    private readonly Aabb2D _bounds;

    private SeatMap(IDictionary<Vec2D, bool> occupancyMap)
    {
        _occupancyMap = occupancyMap;
        _bounds = new Aabb2D(extents: occupancyMap.Keys);
    }

    public bool this[Vec2D seat] => _occupancyMap[seat];
    
    public void UpdateOccupancy(IReadOnlySet<Vec2D> occupied)
    {
        foreach (var seat in _occupancyMap.Keys)
        {
            _occupancyMap[seat] = occupied.Contains(seat);
        }
    }
    
    public bool SeatExistsAt(Vec2D pos)
    {
        return _occupancyMap.ContainsKey(pos);
    }
    
    public bool IsPosInBounds(Vec2D pos)
    {
        return _bounds.ContainsInclusive(pos);
    }
    
    public int CountOccupied()
    {
        return _occupancyMap.Values.Count(occupied => occupied);
    }

    public static SeatMap Parse(IList<string> input)
    {
        var occupancyMap = new Dictionary<Vec2D, bool>();
        var rows = input.Count;
        var cols = input[0].Length;

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[y][x] == 'L')
            {
                occupancyMap[new Vec2D(x, Y: rows - y - 1)] = false;
            }
        }

        return new SeatMap(occupancyMap);
    }

    public IEnumerator<KeyValuePair<Vec2D, bool>> GetEnumerator()
    {
        return _occupancyMap.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_occupancyMap).GetEnumerator();
    }
}