namespace Utilities.Geometry.Euclidean;

public partial class Grid2D<T>
{
    /// <summary>
    /// Flip the grid about the specified axis
    /// </summary>
    /// <param name="about">The axis about which to flip the grid</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Axis.X"/> and <see cref="Axis.Y"/> axes only</exception>
    public void Flip(Axis about)
    {
        var tmp = new T[Height, Width];
        for (var x = 0; x < Width; x++)
        for (var y = 0; y < Height; y++)
        {
            tmp[y, x] = about switch
            {
                Axis.X => _array[Height - y - 1, x],
                Axis.Y => _array[y, Width - x - 1],
                _ => throw ThrowHelper.InvalidFlipAxis(about)
            };
        }

        _array = tmp;
    }
    
    /// <summary>
    /// Rotate the grid by the given argument
    /// </summary>
    /// <param name="rot">The rotation to perform on the grid</param>
    /// <exception cref="ArgumentOutOfRangeException">Argument must be a rotation around the <see cref="Axis.Z"/> axis</exception>
    public void Rotate(Rotation3D rot)
    {
        if (rot.ThetaDeg % Rotation3D.DegreesPerRotation == 0)
        {
            return;
        }
        
        if (rot.Axis != Axis.Z)
        {
            throw ThrowHelper.InvalidRotationAxis(rot.Axis);
        }

        if (rot.ThetaDeg % Rotation3D.NinetyDegrees != 0)
        {
            throw ThrowHelper.InvalidRotationAmount(rot.ThetaDeg);
        }
        
        var map = new Dictionary<Vector3D, Vector3D>();
        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++)
        {
            var from = new Vector3D(x, y, z: 0);
            var to = rot * from;
            map[to] = from;
        }

        var xMin = map.Keys.Min(pos => pos.X);
        var xMax = map.Keys.Max(pos => pos.X);
        var yMin = map.Keys.Min(pos => pos.Y);
        var yMax = map.Keys.Max(pos => pos.Y);
        
        var rows = yMax - yMin + 1;
        var cols = xMax - xMin + 1;
        var tmp = new T[rows, cols];
        
        foreach (var (to, from) in map)
        {
            tmp[to.Y - yMin, to.X - xMin] = _array[from.Y, from.X];
        }

        _array = tmp;
    }
    
    /// <summary>
    /// Internal throw helper for <see cref="Grid2D{T}"/>
    /// </summary>
    private static class ThrowHelper
    {
        private const string InvalidFlipAxisError = $"{nameof(Grid2D<T>)} can only be flipped about the X and Y axis";
        private const string InvalidRotAxisError = $"{nameof(Grid2D<T>)} can only be rotated about the Z axis";
        private const string InvalidRotAmountError = $"{nameof(Grid2D<T>)} can only be rotated integral multiples of 90 degrees";

        internal static Exception InvalidFlipAxis(Axis about)
        {
            throw new ArgumentOutOfRangeException(nameof(about), about, message: InvalidFlipAxisError);
        }
        
        internal static Exception InvalidRotationAxis(Axis axis)
        {
            throw new ArgumentOutOfRangeException(nameof(axis), axis, message: InvalidRotAxisError);
        }
        
        internal static Exception InvalidRotationAmount(int amount)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount, message: InvalidRotAmountError);
        }
    }
}