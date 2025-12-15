namespace Solutions.Y2025.D10;

public static class SimplexBranchAndBound
{
    private const double Epsilon = 1e-9;

    public static int Solve(Machine machine)
    {
        var numVariables = machine.Buttons.Count;
        var constraints = BuildConstraints(machine);
        return BranchAndBound(constraints, numVariables);
    }
    
    private static List<double[]> BuildConstraints(Machine machine)
    {
        var constraints = new List<double[]>();
        var numVariables = machine.Buttons.Count;
        
        for (var i = 0; i < machine.Joltage.Length; i++)
        {
            var equality = new double[numVariables + 1];
            var negatedEquality = new double[numVariables + 1];
            
            for (var j = 0; j < numVariables; j++)
            {
                if (machine.Buttons[j].Contains(i))
                {
                    equality[j] =         1;
                    negatedEquality[j] = -1;
                }
            }
            
            equality[numVariables] =         machine.Joltage[i];
            negatedEquality[numVariables] = -machine.Joltage[i];
            
            constraints.Add(equality);
            constraints.Add(negatedEquality);
        }
        
        for (var i = 0; i < numVariables; i++)
        {
            var nonNegativity = new double[numVariables + 1];
            nonNegativity[i] = -1;
            constraints.Add(nonNegativity);
        }
        
        return constraints;
    }
    
    private static int BranchAndBound(List<double[]> constraints, int numVariables)
    {
        var bestValue = double.PositiveInfinity;
        var objective = Enumerable.Repeat(1.0, numVariables).ToArray();
        
        Branch(constraints);
        return (int)Math.Round(bestValue);

        void Branch(List<double[]> currentConstraints)
        {
            var (value, solution) = Simplex(currentConstraints, objective);
            
            if (value + Epsilon >= bestValue || double.IsNegativeInfinity(value))
            {
                return;
            }
            
            var fractionalIndex = -1;
            for (var i = 0; i < solution!.Length; i++)
            {
                if (Math.Abs(solution[i] - Math.Round(solution[i])) > Epsilon)
                {
                    fractionalIndex = i;
                    break;
                }
            }
            
            if (fractionalIndex == -1)
            {
                bestValue = value;
                return;
            }
            
            var floorValue = (int)solution[fractionalIndex];
            var upperBound = new double[numVariables + 1];
            var lowerBound = new double[numVariables + 1];
            
            upperBound[fractionalIndex] = 1;
            upperBound[numVariables] = floorValue;
            Branch([..currentConstraints, upperBound]);
            
            lowerBound[fractionalIndex] = -1;
            lowerBound[numVariables] = ~floorValue;
            Branch([..currentConstraints, lowerBound]);
        }
    }
    
    private static (double value, double[]? solution) Simplex(List<double[]> constraints, double[] objective)
    {
        var m = constraints.Count;
        var n = objective.Length;
        
        var tableau = new double[m + 2, n + 2];
        var basis = new int[m];
        var nonBasis = new int[n + 1];
        
        for (var i = 0; i < n; i++) nonBasis[i] = i;
        nonBasis[n] = -1;
        
        for (var i = 0; i < m; i++) basis[i] = n + i;
        
        for (var i = 0; i < m; i++)
        {
            for (var j = 0; j < n; j++)
            {
                tableau[i, j] = constraints[i][j];
            }
            tableau[i, n] = -1;
            tableau[i, n + 1] = constraints[i][n];
        }
        
        for (var j = 0; j < n; j++)
        {
            tableau[m, j] = objective[j];
        }
        
        tableau[m + 1, n] = 1;
        
        var minRow = 0;
        for (var i = 1; i < m; i++)
        {
            if (tableau[i, n + 1] < tableau[minRow, n + 1])
            {
                minRow = i;
            }
        }

        if (tableau[minRow, n + 1] < -Epsilon)
        {
            Pivot(minRow, n);
            if (!Optimize(m + 1) || tableau[m + 1, n + 1] < -Epsilon)
            {
                return (double.NegativeInfinity, null);
            }
        }
        
        for (var i = 0; i < m; i++)
        {
            if (basis[i] == -1)
            {
                var minCol = 0;
                for (var j = 1; j < n; j++)
                {
                    if (tableau[i, j] < tableau[i, minCol] - Epsilon || 
                        (Math.Abs(tableau[i, j] - tableau[i, minCol]) < Epsilon && nonBasis[j] < nonBasis[minCol]))
                    {
                        minCol = j;
                    }
                }
                Pivot(i, minCol);
            }
        }
        
        if (!Optimize(m))
        {
            return (double.NegativeInfinity, null);
        }
        
        var solution = new double[n];
        for (var i = 0; i < m; i++)
        {
            if (basis[i] >= 0 && basis[i] < n)
            {
                solution[basis[i]] = tableau[i, n + 1];
            }
        }
        
        var value = 0.0;
        for (var i = 0; i < n; i++)
        {
            value += objective[i] * solution[i];
        }
        
        return (value, solution);

        void Pivot(int r, int s)
        {
            var inv = 1.0 / tableau[r, s];
            
            for (var i = 0; i < m + 2; i++)
            {
                if (i == r) continue;
                for (var j = 0; j < n + 2; j++)
                {
                    if (j != s)
                    {
                        tableau[i, j] -= tableau[r, j] * tableau[i, s] * inv;
                    }
                }
            }
            
            for (var j = 0; j < n + 2; j++)
            {
                tableau[r, j] *= inv;
            }
            
            for (var i = 0; i < m + 2; i++)
            {
                tableau[i, s] *= -inv;
            }
            
            tableau[r, s] = inv;
            (basis[r], nonBasis[s]) = (nonBasis[s], basis[r]);
        }
        
        bool Optimize(int objRow)
        {
            while (true)
            {
                var pivotCol = -1;
                var minVal = double.PositiveInfinity;
                
                for (var j = 0; j < n + 1; j++)
                {
                    if (objRow == m && nonBasis[j] == -1) continue;
                    
                    if (tableau[objRow, j] < minVal - Epsilon || 
                        (Math.Abs(tableau[objRow, j] - minVal) < Epsilon && 
                         (pivotCol == -1 || nonBasis[j] < nonBasis[pivotCol])))
                    {
                        minVal = tableau[objRow, j];
                        pivotCol = j;
                    }
                }
                
                if (tableau[objRow, pivotCol] > -Epsilon)
                {
                    return true;
                }
                
                var pivotRow = -1;
                var minRatio = double.PositiveInfinity;
                
                for (var i = 0; i < m; i++)
                {
                    if (tableau[i, pivotCol] > Epsilon)
                    {
                        var ratio = tableau[i, n + 1] / tableau[i, pivotCol];
                        if (ratio < minRatio - Epsilon || 
                            (Math.Abs(ratio - minRatio) < Epsilon && 
                             (pivotRow == -1 || basis[i] < basis[pivotRow])))
                        {
                            minRatio = ratio;
                            pivotRow = i;
                        }
                    }
                }
                
                if (pivotRow == -1)
                {
                    return false;
                }
                
                Pivot(pivotRow, pivotCol);
            }
        }
    }
}