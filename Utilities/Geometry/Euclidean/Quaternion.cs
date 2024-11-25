namespace Utilities.Geometry.Euclidean;

/// <summary>
///     A simple readonly quaternion value type used for transforming <see cref="Vec2D"/> and <see cref="Vec3D"/>
///     instances.
/// </summary>
/// <param name="W">The scalar part</param>
/// <param name="X">The <b>i</b> component of the vector part</param>
/// <param name="Y">The <b>j</b> component of the vector part</param>
/// <param name="Z">The <b>k</b> component of the vector part</param>
public readonly record struct Quaternion(double W, double X, double Y, double Z)
{
    public static readonly Quaternion Identity = new(W: 1, X: 0, Y: 0, Z: 0);
    
    private Quaternion(Vec3D vec) : this(W: 0, vec.X, vec.Y, vec.Z)
    {
    }

    private Quaternion(Vec2D vec) : this(W: 0, vec.X, vec.Y, Z: 0)
    {
    }
    
    /// <summary>
    ///     Apply the rotation: <b>q * v * q⁻¹</b>.
    /// </summary>
    /// <param name="v">The vector to transform</param>
    /// <returns>The resulting transformed vector</returns>
    public Vec3D Transform(Vec3D v)
    {
        var result = this * new Quaternion(v) * Conjugate();
        return new Vec3D(
            X: (int)Math.Round(result.X),
            Y: (int)Math.Round(result.Y),
            Z: (int)Math.Round(result.Z));
    }
    
    /// <summary>
    ///     Apply the rotation: <b>q * v * q⁻¹</b>.
    /// </summary>
    /// <param name="v">The vector to transform</param>
    /// <returns>The resulting transformed vector</returns>
    public Vec2D Transform(Vec2D v)
    {
        var result = this * new Quaternion(v) * Conjugate();
        return new Vec2D(
            X: (int)Math.Round(result.X),
            Y: (int)Math.Round(result.Y));
    }
    
    public static Quaternion operator *(Quaternion a, Quaternion b)
    {
        return new Quaternion(
            W: a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
            X: a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
            Y: a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X,
            Z: a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W
        );
    }
    
    public override string ToString()
    {
        return $"{W} + <{X}i,{Y}j,{Z}k>";
    }

    private Quaternion Conjugate()
    {
        return new Quaternion(W, -X, -Y, -Z);
    }
}