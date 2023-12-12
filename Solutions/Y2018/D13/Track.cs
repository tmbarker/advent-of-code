using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D13;

public static class Track
{
    public const char Junction = '+';
    public const char Left = '\\';
    public const char Right = '/';
    
    public static readonly Dictionary<char, Vector2D> CartFacings = new()
    {
        { '<', Vector2D.Left },
        { '>', Vector2D.Right },
        { '^', Vector2D.Down },
        { 'v', Vector2D.Up }
    };

    public static readonly Dictionary<int, Rotation3D> TurnChoices = new()
    {
        { 0, Rotation3D.Negative90Z },
        { 1, Rotation3D.Zero },
        { 2, Rotation3D.Positive90Z }
    };
    
    public static Vector2D TurnForCorner(char corner, Vector2D face)
    {
        var eastOrWest = face == Vector2D.Left || face == Vector2D.Right;
        var rot = corner switch
        {
            Left  => eastOrWest ? Rotation3D.Positive90Z : Rotation3D.Negative90Z,
            Right => eastOrWest ? Rotation3D.Negative90Z : Rotation3D.Positive90Z,
            _ => throw new ArgumentOutOfRangeException(nameof(corner))
        };

        return rot * face;
    }
}