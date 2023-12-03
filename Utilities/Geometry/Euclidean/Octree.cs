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
        var x1 = aabb.XLength is 1 or 2 ? aabb.XMin : aabb.XMin + aabb.XLength / 2;
        var x2 = aabb.XLength is 1 or 2 ? aabb.XMax : aabb.XMin + aabb.XLength / 2 + 1;
        var y1 = aabb.YLength is 1 or 2 ? aabb.YMin : aabb.YMin + aabb.YLength / 2;
        var y2 = aabb.YLength is 1 or 2 ? aabb.YMax : aabb.YMin + aabb.YLength / 2 + 1;
        var z1 = aabb.ZLength is 1 or 2 ? aabb.ZMin : aabb.ZMin + aabb.ZLength / 2;
        var z2 = aabb.ZLength is 1 or 2 ? aabb.ZMax : aabb.ZMin + aabb.ZLength / 2 + 1;

        // A
        set.Add(new Aabb3D(
            xMin: aabb.XMin,
            xMax: x1,
            yMin: y2,
            yMax: aabb.YMax,
            zMin: aabb.ZMin,
            zMax: z1));
        
        // B
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.XMax,
            yMin: y2,
            yMax: aabb.YMax,
            zMin: aabb.ZMin,
            zMax: z1));
        
        // C
        set.Add(new Aabb3D(
            xMin: aabb.XMin,
            xMax: x1,
            yMin: y2,
            yMax: aabb.YMax,
            zMin: z2,
            zMax: aabb.ZMax));
        
        // D
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.XMax,
            yMin: y2,
            yMax: aabb.YMax,
            zMin: z2,
            zMax: aabb.ZMax));
        
        // E
        set.Add(new Aabb3D(
            xMin: aabb.XMin,
            xMax: x1,
            yMin: aabb.YMin,
            yMax: y1,
            zMin: aabb.ZMin,
            zMax: z1));
        
        // F
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.XMax,
            yMin: aabb.YMin,
            yMax: y1,
            zMin: aabb.ZMin,
            zMax: z1));
        
        // G
        set.Add(new Aabb3D(
            xMin: aabb.XMin,
            xMax: x1,
            yMin: aabb.YMin,
            yMax: y1,
            zMin: z2,
            zMax: aabb.ZMax));
        
        // H
        set.Add(new Aabb3D(
            xMin: x2,
            xMax: aabb.XMax,
            yMin: aabb.YMin,
            yMax: y1,
            zMin: z2,
            zMax: aabb.ZMax));

        return set;
    }
}