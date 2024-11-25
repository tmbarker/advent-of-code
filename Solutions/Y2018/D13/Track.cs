using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D13;

public static class Track
{
    public const char Junction = '+';
    public const char Left = '\\';
    public const char Right = '/';
    
    public static readonly Dictionary<char, Vec2D> CartFacings = new()
    {
        { '<', Vec2D.Left },
        { '>', Vec2D.Right },
        { '^', Vec2D.Down },
        { 'v', Vec2D.Up }
    };

    public static readonly Dictionary<int, Quaternion> TurnChoices = new()
    {
        { 0, Rot3D.N90Z },
        { 1, Rot3D.Zero },
        { 2, Rot3D.P90Z }
    };
    
    public static Vec2D TurnForCorner(char corner, Vec2D face)
    {
        var eastOrWest = face == Vec2D.Left || face == Vec2D.Right;
        var rot = corner switch
        {
            Left  => eastOrWest ? Rot3D.P90Z : Rot3D.N90Z,
            Right => eastOrWest ? Rot3D.N90Z : Rot3D.P90Z,
            _ => throw new ArgumentOutOfRangeException(nameof(corner))
        };

        return rot.Transform(face);
    }
}