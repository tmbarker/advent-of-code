using System.Numerics;

namespace Utilities.Numerics;

/// <summary>
/// A generic utility class for solving linear systems of equations
/// </summary>
public static class LinearSolver
{
    /// <summary>
    /// Solve a linear system of equations specified by an augmented coefficient matrix.
    /// </summary>
    /// <param name="a">The augmented coefficient matrix</param>
    /// <typeparam name="T">The type associated with each matrix element</typeparam>
    /// <returns>A vector containing a solution to the system of equations</returns>
    public static T[] Solve<T>(T[,] a) where T : INumber<T>
    {
        var n = a.GetLength(dimension: 0);
        var x = new T[n];
        PartialPivot(a, n);
        BackSubstitute(a, n, x);
        return x;
    }

    private static void PartialPivot<T>(T[,] a, int n) where T : INumber<T>
    {
        for (var i = 0; i < n; i++) 
        {
            var pivotRow = i;
            for (var j = i + 1; j < n; j++) {
                if (T.Abs(a[j, i]) > T.Abs(a[pivotRow, i])) {
                    pivotRow = j;
                }
            }
            
            if (pivotRow != i) {
                for (var j = i; j <= n; j++) 
                {
                    (a[i, j], a[pivotRow, j]) = (a[pivotRow, j], a[i, j]);
                }
            }
            
            for (var j = i + 1; j < n; j++) 
            {
                var factor = a[j, i] / a[i, i];
                for (var k = i; k <= n; k++) 
                {
                    a[j, k] -= factor * a[i, k];
                }
            }
        }
    }
    
    private static void BackSubstitute<T>(T[,] a, int n, T[] x) where T : INumber<T>
    {
        for (var i = n - 1; i >= 0; i--)
        {
            var sum = T.Zero;
            for (var j = i + 1; j < n; j++)
            {
                sum += a[i, j] * x[j];
            }
            x[i] = (a[i, n] - sum) / a[i, i];
        }
    }
}