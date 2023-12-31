using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D20;

public sealed class PortalMap(IDictionary<PortalKey, List<PortalEntrance>> entranceMap)
{
    private readonly IDictionary<Vector2D, EntranceType> _typeMap = BuildTypeMap(entranceMap);
    private readonly IDictionary<Vector2D, Vector2D> _connectionMap = BuildConnectionMap(entranceMap);

    public bool TryTakePortal(Vector2D from, out EntranceType entranceType, out Vector2D exit)
    {
        entranceType = EntranceType.Inner;
        exit = Vector2D.Zero;

        if (!_connectionMap.ContainsKey(from))
        {
            return false;
        }
        
        entranceType = _typeMap[from];
        exit = _connectionMap[from];
        
        return true;
    }
    
    public IEnumerable<Vector2D> GetEntrancePositions(PortalKey key)
    {
        return entranceMap[key].Select(e => e.Pos);
    }

    private static Dictionary<Vector2D, EntranceType> BuildTypeMap(
        IDictionary<PortalKey, List<PortalEntrance>> entranceMap)
    {
        return entranceMap.Values
            .SelectMany(e => e)
            .ToDictionary(entrance => entrance.Pos, entrance => entrance.Type);
    }
    
    private static Dictionary<Vector2D, Vector2D> BuildConnectionMap(
        IDictionary<PortalKey, List<PortalEntrance>> entranceMap)
    {
        var map = new Dictionary<Vector2D, Vector2D>();
        foreach (var pair in entranceMap.Values.Where(c => c.Count == 2))
        {
            var n1 = pair.First();
            var n2 = pair.Last();
            
            map.Add(n1.Pos, n2.Pos);
            map.Add(n2.Pos, n1.Pos);
        }

        return map;
    }
}