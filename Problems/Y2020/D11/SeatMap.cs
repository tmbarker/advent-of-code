using System.Collections;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2020.D11;

public sealed class SeatMap : IEnumerable<KeyValuePair<Vector2D, bool>>
{
    private readonly IDictionary<Vector2D, bool> _occupancyMap;
    private readonly Aabb2D _bounds;

    private SeatMap(IDictionary<Vector2D, bool> occupancyMap)
    {
        _occupancyMap = occupancyMap;
        _bounds = new Aabb2D(extents: occupancyMap.Keys);
    }

    public bool this[Vector2D seat] => _occupancyMap[seat];
    
    public void UpdateOccupancy(IReadOnlySet<Vector2D> occupied)
    {
        foreach (var seat in _occupancyMap.Keys)
        {
            _occupancyMap[seat] = occupied.Contains(seat);
        }
    }
    
    public bool SeatExistsAt(Vector2D pos)
    {
        return _occupancyMap.ContainsKey(pos);
    }
    
    public bool IsPosInBounds(Vector2D pos)
    {
        return _bounds.Contains(pos, inclusive: true);
    }
    
    public int CountOccupied()
    {
        return _occupancyMap.Values.Count(occupied => occupied);
    }

    public static SeatMap Parse(IList<string> input)
    {
        var occupancyMap = new Dictionary<Vector2D, bool>();
        var rows = input.Count;
        var cols = input[0].Length;

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[y][x] == 'L')
            {
                occupancyMap[new Vector2D(x, y: rows - y - 1)] = false;
            }
        }

        return new SeatMap(occupancyMap);
    }

    public IEnumerator<KeyValuePair<Vector2D, bool>> GetEnumerator()
    {
        return _occupancyMap.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_occupancyMap).GetEnumerator();
    }
}