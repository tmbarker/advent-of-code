using Utilities.Numerics;

namespace Solutions.Y2023.D24;

[PuzzleInfo("Never Tell Me The Odds", Topics.Vectors|Topics.Math, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var rays = GetInputLines()
            .Select(Ray3.Parse)
            .ToArray();
        
        return part switch
        {
            1 => Intersect2D(rays, aabb: new Aabb2(Min: 2e14m, Max: 4e14m)),
            2 => Intersect3D(rays),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Intersect2D(Ray3[] rays, Aabb2 aabb)
    {
        var n = 0L;
        
        for (var i = 0; i < rays.Length - 1; i++)
        for (var j = i + 1; j < rays.Length; j++)
        {
            if (Ray3.Intersect2D(a: rays[i], b: rays[j], out var p) && aabb.Contains(p))
            {
                n++;
            }
        }

        return n;
    }

    private static decimal Intersect3D(Ray3[] rays)
    {
        //  Let:
        //    <p_rock>(t) = <X,Y,Z> + t <DX,DY,DZ> 
        //    <p_hail>(t) = <x,y,z> + t <dx,dy,dz>
          
        //  A rock-hail collision requires the following to be true:
        //    X + t DX = x + t dx
        //    Y + t DY = y + t dy
        //    Z + t DZ = z + t dz
        
        //  Which implies:
        //    t = (X-x)/(dx-DX)
        //    t = (Y-y)/(dy-DY)
        //    t = (Z-z)/(dz-DZ)
        
        //  Equating the first two equalities from above yields:
        //    (X-x)/(dx-DX) = (Y-y)/(dy-DY)
        //    (X-x) (dy-DY) = (Y-y) (dx-DX)
        //    X*dy - X*DY - x*dy + x*DY = Y*dx - Y*DX - y*dx + y*DX
        //    Y*DX - X*DY = Y*dx - y*dx + y*DX - X*dy + x*dy - x*DY
        
        //  Note that the LHS of the above equation is true for any hail stone. Evaluating
        //  the RHS again for a different hailstone, and setting the two RHS equal, yields 
        //  the first of the below equations:
        
        //  (dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY =  x' dy' - y' dx' - x dy + y dx
        //  (dz'-dz) X + (dx-dx') Z + (z-z') DX + (x'-x) DZ =  x' dz' - z' dx' - x dz + z dx
        //  (dz-dz') Y + (dy'-dy) Z + (z'-z) DY + (y-y') DZ = -y' dz' + z' dy' + y dz - z dy
        
        //  The second and third are yielded by repeating the above process with X & Z, and 
        //  then Y & Z. This is a system of equations with 6 unknowns. Using two different
        //  pairs of hailstones (e.g. three total hailstones) yields 6 equations with 6
        //  unknowns, which we can now solve relatively trivially using linear algebra.
        
        var matrix = new decimal[6, 7];
        FillRow(matrix, row: 0, vals: Coefficients1(a: rays[0], b: rays[1]));
        FillRow(matrix, row: 1, vals: Coefficients1(a: rays[0], b: rays[2]));
        FillRow(matrix, row: 2, vals: Coefficients2(a: rays[0], b: rays[1]));
        FillRow(matrix, row: 3, vals: Coefficients2(a: rays[0], b: rays[2]));
        FillRow(matrix, row: 4, vals: Coefficients3(a: rays[0], b: rays[1]));
        FillRow(matrix, row: 5, vals: Coefficients3(a: rays[0], b: rays[2]));

        return LinearSolver
            .Solve(a: matrix)
            .Take(3)
            .Select(v => Math.Round(v))
            .Sum();
    }

    private static void FillRow(decimal[,] matrix, int row, decimal[] vals)
    {
        for (var j = 0; j < vals.Length; j++)
        {
            matrix[row, j] = vals[j];
        }
    }
    
    private static decimal[] Coefficients1(Ray3 a, Ray3 b)
    {
        // (dy'-dy) X + (dx-dx') Y + (y-y') DX + (x'-x) DY =  x' dy' - y' dx' - x dy + y dx
        var arr = new decimal[7];
        arr[0] = b.V.Y - a.V.Y;
        arr[1] = a.V.X - b.V.X;
        arr[3] = a.S.Y - b.S.Y;
        arr[4] = b.S.X - a.S.X;
        arr[6] = b.S.X * b.V.Y - b.S.Y * b.V.X - a.S.X * a.V.Y + a.S.Y * a.V.X;
        return arr;
    }
    
    private static decimal[] Coefficients2(Ray3 a, Ray3 b)
    {
        // (dz'-dz) X + (dx-dx') Z + (z-z') DX + (x'-x) DZ =  x' dz' - z' dx' - x dz + z dx
        var arr = new decimal[7];
        arr[0] = b.V.Z - a.V.Z;
        arr[2] = a.V.X - b.V.X;
        arr[3] = a.S.Z - b.S.Z;
        arr[5] = b.S.X - a.S.X;
        arr[6] = b.S.X * b.V.Z - b.S.Z * b.V.X - a.S.X * a.V.Z + a.S.Z * a.V.X;
        return arr;
    }
    
    private static decimal[] Coefficients3(Ray3 a, Ray3 b)
    {
        // (dz-dz') Y + (dy'-dy) Z + (z'-z) DY + (y-y') DZ = -y' dz' + z' dy' + y dz - z dy
        var arr = new decimal[7];
        arr[1] = a.V.Z - b.V.Z;
        arr[2] = b.V.Y - a.V.Y;
        arr[4] = b.S.Z - a.S.Z;
        arr[5] = a.S.Y - b.S.Y;
        arr[6] = -b.S.Y * b.V.Z + b.S.Z * b.V.Y + a.S.Y * a.V.Z - a.S.Z * a.V.Y;
        return arr;
    }
}