namespace Solutions.Y2023.D24;

public static class SystemSolver
{
    public static decimal[] Solve(decimal[,] a, int n)
    {
        var x = new decimal[n];
        PartialPivot(a, n);
        BackSubstitute(a, n, x);
        return x;
    }

    private static void PartialPivot(decimal[,] a, int n)
    {
        for (var i = 0; i < n; i++) 
        {
            var pivotRow = i;
            for (var j = i + 1; j < n; j++) {
                if (Math.Abs(a[j, i]) > Math.Abs(a[pivotRow, i])) {
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

    private static void BackSubstitute(decimal[,] a, int n, decimal[] x)
    {
        for (var i = n - 1; i >= 0; i--)
        {
            decimal sum = 0;
            for (var j = i + 1; j < n; j++)
            {
                sum += a[i, j] * x[j];
            }
            x[i] = (a[i, n] - sum) / a[i, i];
        }
    }
}