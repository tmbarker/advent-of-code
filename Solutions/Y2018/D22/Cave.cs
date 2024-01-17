using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D22;

public sealed class Cave(Scan scan)
{
    public static readonly Dictionary<RegionType, int> RegionRiskLevels = new()
    {
        { RegionType.Rocky,  0 },
        { RegionType.Wet,    1 },
        { RegionType.Narrow, 2 }
    };

    public static readonly Dictionary<RegionType, HashSet<EquippedTool>> RegionAllowedTools = new()
    {
        { RegionType.Rocky,  [EquippedTool.ClimbingGear, EquippedTool.Torch] },
        { RegionType.Wet,    [EquippedTool.ClimbingGear, EquippedTool.None] },
        { RegionType.Narrow, [EquippedTool.Torch,        EquippedTool.None] }
    };

    private const long GeoIndexCoefficientX = 16807L;
    private const long GeoIndexCoefficientY = 48271L;
    private const long ErosionModulus = 20183L;

    private readonly Dictionary<Vec2D, Region> _regionMap = new();

    public Region this[Vec2D pos] => GetRegionInternal(pos);

    private Region GetRegionInternal(Vec2D pos)
    {
        if (!_regionMap.ContainsKey(pos))
        {
            _regionMap[pos] = FormRegion(pos);
        }

        return _regionMap[pos];
    }
    
    private Region FormRegion(Vec2D pos)
    {
        var index = ComputeGeologicIndex(pos);
        var erosion = (index + scan.Depth) % ErosionModulus;
        var type = (erosion % 3) switch
        {
            0 => RegionType.Rocky,
            1 => RegionType.Wet,
            2 => RegionType.Narrow,
            _ => throw new NoSolutionException()
        };

        return new Region(erosion, type);
    }
    
    private long ComputeGeologicIndex(Vec2D pos)
    {
        if (pos == scan.Mouth || pos == scan.Target)
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

        var p1 = new Vec2D(x: pos.X - 1, y: pos.Y);
        var p2 = new Vec2D(x: pos.X, y: pos.Y - 1);
        var r1 = GetRegionInternal(p1);
        var r2 = GetRegionInternal(p2);

        return r1.Erosion * r2.Erosion;
    }
}