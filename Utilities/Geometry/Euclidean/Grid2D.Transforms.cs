using System.Runtime.CompilerServices;
using Utilities.Extensions;

namespace Utilities.Geometry.Euclidean;

public sealed partial class Grid2D<T>
{
    /// <summary>
    ///     Flip the grid about the specified axis.
    /// </summary>
    /// <param name="about">The axis about which to flip the grid</param>
    /// <exception cref="ArgumentOutOfRangeException"><see cref="Axis.X" /> and <see cref="Axis.Y" /> axes only</exception>
    public void Flip(Axis about)
    {
        _array = about switch
        {
            Axis.X => FlipVertical(arr: _array),
            Axis.Y => FlipHorizontal(arr: _array),
            _ => throw ThrowHelper.InvalidFlipAxis(about)
        };
    }

    /// <summary>
    ///     Rotate the grid by the given argument.
    /// </summary>
    /// <param name="deg">The integral number of degrees to rotate the grid</param>
    public void Rotate(int deg)
    {
        switch (deg.Modulo(Degrees.P360))
        {
            case Degrees.Zero:
                return;
            case Degrees.P90:
                RotatePositive90();
                return;
            case Degrees.P180:
                Rotate180();
                return;
            case Degrees.P270:
                RotateNegative90();
                return;
            default:
                throw ThrowHelper.InvalidRotationAmount(deg);
        }
    }

    /// <summary>
    ///     Rotate the array 180 degrees.
    /// </summary>
    private void Rotate180()
    {
        var rows = Height;
        var cols = Width;

        for (var r = 0; r < rows / 2; r++)
        for (var c = 0; c < cols; c++)
        {
            SwapElements(arr: _array, r1: r, c1: c, r2: rows - r - 1, c2: cols - c - 1);
        }

        if (rows % 2 == 0)
        {
            return;
        }

        for (var c = 0; c < cols / 2; c++)
        {
            SwapElements(arr: _array, r1: rows / 2, c1: c, r2: rows / 2, c2: cols - c - 1);
        }
    }

    /// <summary>
    ///     Rotate the array +90 degrees (CCW).
    /// </summary>
    private void RotatePositive90()
    {
        _array = Height == Width
            ? TransposeInPlace(src: _array)
            : TransposeToArray(src: _array);
        _array = FlipHorizontal(arr: _array);
    }

    /// <summary>
    ///     Rotate the array -90 degrees (CW).
    /// </summary>
    private void RotateNegative90()
    {
        _array = Height == Width
            ? TransposeInPlace(src: _array)
            : TransposeToArray(src: _array);
        _array = FlipVertical(arr: _array);
    }

    /// <summary>
    ///     Transpose the <paramref name="src" /> array in place, must be square.
    /// </summary>
    private static T[,] TransposeInPlace(T[,] src)
    {
        for (var i = 0; i < src.GetLength(dimension: 0); i++)
        for (var j = 0; j < i; j++)
        {
            SwapElements(arr: src, r1: i, c1: j, r2: j, c2: i);
        }

        return src;
    }

    /// <summary>
    ///     Transpose the source array into a newly allocated destination array. If the source array is square
    ///     invoke <see cref="TransposeInPlace" /> isntead to avoid an unnecessary array allocation.
    /// </summary>
    private static T[,] TransposeToArray(T[,] src)
    {
        var rows = src.GetLength(dimension: 0);
        var cols = src.GetLength(dimension: 1);
        var dst = new T[cols, rows];

        for (var i = 0; i < rows; i++)
        for (var j = 0; j < cols; j++)
        {
            dst[j, i] = src[i, j];
        }

        return dst;
    }

    /// <summary>
    ///     Flip the array in place about the <see cref="Axis.X" /> axis.
    /// </summary>
    private static T[,] FlipVertical(T[,] arr)
    {
        var rows = arr.GetLength(dimension: 0);
        var cols = arr.GetLength(dimension: 1);

        for (var c = 0; c < cols; c++)
        for (var r = 0; r < rows / 2; r++)
        {
            SwapElements(arr: arr, r1: r, c1: c, r2: rows - r - 1, c2: c);
        }

        return arr;
    }

    /// <summary>
    ///     Flip the array in place about the <see cref="Axis.Y" /> axis.
    /// </summary>
    private static T[,] FlipHorizontal(T[,] arr)
    {
        var rows = arr.GetLength(dimension: 0);
        var cols = arr.GetLength(dimension: 1);

        for (var r = 0; r < rows; r++)
        for (var c = 0; c < cols / 2; c++)
        {
            SwapElements(arr: arr, r1: r, c1: c, r2: r, c2: cols - c - 1);
        }

        return arr;
    }

    /// <summary>
    ///     Swap two array elements in place.
    /// </summary>
    /// <param name="r1">The row index of element 1</param>
    /// <param name="c1">The col index of element 1</param>
    /// <param name="r2">The row index of element 2</param>
    /// <param name="c2">The col index of element 2</param>
    /// <param name="arr">The array</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SwapElements(T[,] arr, int r1, int c1, int r2, int c2)
    {
        (arr[r1, c1], arr[r2, c2]) = (arr[r2, c2], arr[r1, c1]);
    }

    /// <summary>
    ///     Internal throw helper for <see cref="Grid2D{T}" />
    /// </summary>
    private static class ThrowHelper
    {
        private const string InvalidFlipAxisError = $"{nameof(Grid2D<T>)} can only be flipped about the X and Y axis";

        private const string InvalidRotAmountError =
            $"{nameof(Grid2D<T>)} can only be rotated integral multiples of 90 degrees";

        internal static Exception InvalidFlipAxis(Axis about)
        {
            throw new ArgumentOutOfRangeException(nameof(about), about, message: InvalidFlipAxisError);
        }

        internal static Exception InvalidRotationAmount(int amount)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), amount, message: InvalidRotAmountError);
        }
    }
}