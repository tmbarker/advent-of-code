using Problems.Common;
using Utilities.Cartesian;

namespace Problems.Y2018.D22;

public class Cave
{
    public static readonly Dictionary<RegionType, int> RegionRiskLevels = new()
    {
        { RegionType.Rocky,  0 },
        { RegionType.Wet,    1 },
        { RegionType.Narrow, 2 }
    };

    public static readonly Dictionary<RegionType, IReadOnlySet<EquippedTool>> RegionAllowedTools = new()
    {
        { RegionType.Rocky,  new HashSet<EquippedTool> { EquippedTool.ClimbingGear, EquippedTool.Torch } },
        { RegionType.Wet,    new HashSet<EquippedTool> { EquippedTool.ClimbingGear, EquippedTool.None } },
        { RegionType.Narrow, new HashSet<EquippedTool> { EquippedTool.Torch, EquippedTool.None } }
    };

    private const long GeoIndexCoefficientX = 16807L;
    private const long GeoIndexCoefficientY = 48271L;
    private const long ErosionModulus = 20183L;
    
    private readonly Scan _scan;
    private readonly Dictionary<Vector2D, Region> _regionMap = new();

    public Region this[Vector2D pos] => GetRegionInternal(pos);

    public Cave(Scan scan)
    {
        _scan = scan;
    }

    private Region GetRegionInternal(Vector2D pos)
    {
        if (pos.X < 0 || pos.Y < 0)
        {
            throw new NoSolutionException();
        }
        
        if (!_regionMap.ContainsKey(pos))
        {
            _regionMap[pos] = FormRegion(pos);
        }

        return _regionMap[pos];
    }
    
    private Region FormRegion(Vector2D pos)
    {
        var index = ComputeGeologicIndex(pos);
        var erosion = (index + _scan.Depth) % ErosionModulus;
        var type = (erosion % 3) switch
        {
            0 => RegionType.Rocky,
            1 => RegionType.Wet,
            2 => RegionType.Narrow,
            _ => throw new NoSolutionException()
        };

        return new Region(erosion, type);
    }
    
    private long ComputeGeologicIndex(Vector2D pos)
    {
        if (pos == _scan.Mouth || pos == _scan.Target)
        {
            return 0;
        }

        if (pos.Y == 0)
        {
            return GeoIndexCoefficientX * pos.X;
        }

        if (pos.X == 0)
        {
            return GeoIndexCoefficientY * pos.Y;
        }

        var p1 = new Vector2D(x: pos.X - 1, y: pos.Y);
        var p2 = new Vector2D(x: pos.X, y: pos.Y - 1);
        var r1 = GetRegionInternal(p1);
        var r2 = GetRegionInternal(p2);

        return r1.Erosion * r2.Erosion;
    }
}