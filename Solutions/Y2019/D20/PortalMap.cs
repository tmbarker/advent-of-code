using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D20;

public sealed class PortalMap(IDictionary<PortalKey, List<PortalEntrance>> entranceMap)
{
    private readonly IDictionary<Vec2D, EntranceType> _typeMap = BuildTypeMap(entranceMap);
    private readonly IDictionary<Vec2D, Vec2D> _connectionMap = BuildConnectionMap(entranceMap);

    public bool TryTakePortal(Vec2D from, out EntranceType entranceType, out Vec2D exit)
    {
        entranceType = EntranceType.Inner;
        exit = Vec2D.Zero;

        if (!_connectionMap.ContainsKey(from))
        {
            return false;
        }
        
        entranceType = _typeMap[from];
        exit = _connectionMap[from];
        
        return true;
    }
    
    public IEnumerable<Vec2D> GetEntrancePositions(PortalKey key)
    {
        return entranceMap[key].Select(e => e.Pos);
    }

    private static Dictionary<Vec2D, EntranceType> BuildTypeMap(
        IDictionary<PortalKey, List<PortalEntrance>> entranceMap)
    {
        return entranceMap.Values
            .SelectMany(e => e)
            .ToDictionary(entrance => entrance.Pos, entrance => entrance.Type);
    }
    
    private static Dictionary<Vec2D, Vec2D> BuildConnectionMap(
        IDictionary<PortalKey, List<PortalEntrance>> entranceMap)
    {
        var map = new Dictionary<Vec2D, Vec2D>();
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