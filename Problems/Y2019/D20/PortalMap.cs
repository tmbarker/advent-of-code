using Utilities.Geometry.Euclidean;

namespace Problems.Y2019.D20;

public class PortalMap
{
    private readonly Dictionary<PortalKey, IList<PortalEntrance>> _entranceMap;
    private readonly Dictionary<Vector2D, EntranceType> _typeMap;
    private readonly Dictionary<Vector2D, Vector2D> _connectionMap;
    
    public PortalMap(Dictionary<PortalKey, IList<PortalEntrance>> entranceMap)
    {
        _entranceMap = entranceMap;
        _typeMap = BuildTypeMap(entranceMap);
        _connectionMap = BuildConnectionMap(entranceMap);
    }
    
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
        return _entranceMap[key].Select(e => e.Pos);
    }

    private static Dictionary<Vector2D, EntranceType> BuildTypeMap(
        Dictionary<PortalKey, IList<PortalEntrance>> entranceMap)
    {
        return entranceMap.Values
            .SelectMany(e => e)
            .ToDictionary(entrance => entrance.Pos, entrance => entrance.Type);
    }
    
    private static Dictionary<Vector2D, Vector2D> BuildConnectionMap(
        Dictionary<PortalKey, IList<PortalEntrance>> entranceMap)
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