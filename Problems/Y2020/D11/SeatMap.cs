using Utilities.Cartesian;

namespace Problems.Y2020.D11;

public class SeatMap
{
    private readonly IDictionary<Vector2D, bool> _occupancyMap;
    private readonly Aabb2D _bounds;

    private SeatMap(IDictionary<Vector2D, bool> occupancyMap)
    {
        _occupancyMap = occupancyMap;
        _bounds = new Aabb2D(occupancyMap.Keys, true);
    }

    public bool this[Vector2D seat] => _occupancyMap[seat];
    
    public IEnumerator<KeyValuePair<Vector2D, bool>> GetEnumerator()
    {
        return _occupancyMap.GetEnumerator();
    }

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
        return _bounds.Contains(pos, true);
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
                occupancyMap.Add(new Vector2D(x, rows - y - 1), false);
            }
        }

        return new SeatMap(occupancyMap);
    }
}