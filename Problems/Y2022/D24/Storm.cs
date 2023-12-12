using Utilities.Geometry.Euclidean;

namespace Problems.Y2022.D24;

public class Storm
{
    private readonly Grid2D<char> _field;
    private readonly HashSet<Vector2D> _occupiedPositions;
    private readonly List<Blizzard> _blizzards;
    private readonly List<Blizzard> _temp;
    
    public IReadOnlySet<Vector2D> OccupiedPositions => _occupiedPositions;

    public Storm(Grid2D<char> field, List<Blizzard> blizzards)
    {
        _field = field;
        _occupiedPositions = blizzards.Select(b => b.Pos).ToHashSet();
        _blizzards = blizzards;
        _temp = new List<Blizzard>(capacity: blizzards.Count);
    }
    
    public void Step()
    {
        _occupiedPositions.Clear();
        _temp.Clear();
        
        foreach (var blizzard in _blizzards)
        {
            if (_field[blizzard.Ahead] == Terrain.Void)
            {
                _occupiedPositions.Add(blizzard.Ahead);
                _temp.Add(blizzard.Step());
                continue;
            }
            
            _occupiedPositions.Add(blizzard.RespawnAt);
            _temp.Add(blizzard.Respawn());
        }

        _blizzards.Clear();
        _blizzards.AddRange(_temp);
    }
    
    public IEnumerable<Vector2D> GetSafeMoves(Vector2D head)
    {
        return head
            .GetAdjacentSet(Metric.Taxicab)
            .Where(IsPositionSafe);
    }
    
    private bool IsPositionSafe(Vector2D target)
    {
        return
            _field.IsInDomain(target) &&
            _field[target] == Terrain.Void &&
            !OccupiedPositions.Contains(target);
    }
}