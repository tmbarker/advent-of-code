using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D22;

public sealed class Brick(int id, Aabb3D extents)
{
    public int Id => id;
    public Aabb3D Extents { get; set; } = extents;
    
    public static Brick Parse(int id, string line)
    {
        var ps = line.Split(separator: '~');
        var p1 = Vec3D.Parse(ps[0]);
        var p2 = Vec3D.Parse(ps[1]);

        return new Brick(id, extents: new Aabb3D(extents: [p1, p2], inclusive: true));
    }
}