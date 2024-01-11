namespace Utilities.Geometry.Euclidean;

/// <summary>
/// An Octree utility exposing <see cref="Subdivide"/>
/// </summary>
public static class Octree
{
    /// <summary>
    /// Subdivide the provided <see cref="Aabb3D"/> into octants
    /// </summary>
    public static IEnumerable<Aabb3D> Subdivide(Aabb3D aabb)
    { 
        // TOP: C D
        //      A B
        // BOT: G H
        //      E F  
        
        var set = new HashSet<Aabb3D>();
        var x1 = aabb.XLength is 1 or 2 ? aabb.Min.X : aabb.Min.X + aabb.XLength / 2;
        var x2 = aabb.XLength is 1 or 2 ? aabb.Max.X : aabb.Min.X + aabb.XLength / 2 + 1;
        var y1 = aabb.YLength is 1 or 2 ? aabb.Min.Y : aabb.Min.Y + aabb.YLength / 2;
        var y2 = aabb.YLength is 1 or 2 ? aabb.Max.Y : aabb.Min.Y + aabb.YLength / 2 + 1;
        var z1 = aabb.ZLength is 1 or 2 ? aabb.Min.Z : aabb.Min.Z + aabb.ZLength / 2;
        var z2 = aabb.ZLength is 1 or 2 ? aabb.Max.Z : aabb.Min.Z + aabb.ZLength / 2 + 1;

        // A
        set.Add(new Aabb3D(
            xMin: aabb.Min.X,
            xMax: x1,
            yMin: y2,
            yMax: aabb.Max.Y,
            zMin: aabb.Min.Z,
            zMax: z1));
        
        // B
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.Max.X,
            yMin: y2,
            yMax: aabb.Max.Y,
            zMin: aabb.Min.Z,
            zMax: z1));
        
        // C
        set.Add(new Aabb3D(
            xMin: aabb.Min.X,
            xMax: x1,
            yMin: y2,
            yMax: aabb.Max.Y,
            zMin: z2,
            zMax: aabb.Max.Z));
        
        // D
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.Max.X,
            yMin: y2,
            yMax: aabb.Max.Y,
            zMin: z2,
            zMax: aabb.Max.Z));
        
        // E
        set.Add(new Aabb3D(
            xMin: aabb.Min.X,
            xMax: x1,
            yMin: aabb.Min.Y,
            yMax: y1,
            zMin: aabb.Min.Z,
            zMax: z1));
        
        // F
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.Max.X,
            yMin: aabb.Min.Y,
            yMax: y1,
            zMin: aabb.Min.Z,
            zMax: z1));
        
        // G
        set.Add(new Aabb3D(
            xMin: aabb.Min.X,
            xMax: x1,
            yMin: aabb.Min.Y,
            yMax: y1,
            zMin: z2,
            zMax: aabb.Max.Z));
        
        // H
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.Max.X,
            yMin: aabb.Min.Y,
            yMax: y1,
            zMin: z2,
            zMax: aabb.Max.Z));

        return set;
    }
}