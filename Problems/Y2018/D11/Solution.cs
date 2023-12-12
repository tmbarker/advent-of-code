namespace Problems.Y2018.D11;

[PuzzleInfo("Chronal Charge", Topics.Vectors, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputText();
        var serial = int.Parse(input);
        
        return part switch
        {
            1 => GetMaxPowerFixed(serial, gridSize: 300, sqrSize: 3),
            2 => GetMaxPowerVariable(serial, gridSize: 300),
            _ => ProblemNotSolvedString
        };
    }

    private static string GetMaxPowerFixed(int serial, int gridSize, int sqrSize)
    {
        var powers = BuildPowerArray(serial, gridSize);
        var numRegions = (int)Math.Pow(gridSize - (sqrSize - 1), 2);
        var regionSums = new Dictionary<(int, int), int>(capacity: numRegions);
        
        for (var y = 0; y < gridSize - sqrSize; y++)
        for (var x = 0; x < gridSize - sqrSize; x++)
        {
            var sum = 0;
            for (var v = 0; v < sqrSize; v++)
            for (var u = 0; u < sqrSize; u++)
            {
                sum += powers[y + v, x + u];
            }

            regionSums[(x, y)] = sum;
        }
        
        var (xMax, yMax) = regionSums.Keys.MaxBy(v => regionSums[v]);
        return $"{xMax + 1},{yMax + 1}";
    }

    private static string GetMaxPowerVariable(int serial, int gridSize)
    {
        var powers = BuildPowerArray(serial, gridSize);
        var summedAreaTable = BuildSummedAreaTable(powers, gridSize);

        var (xMax, yMax) = (0, 0);
        var pMax = 0;
        var sMax = 0;
        
        for (var sqrSize = 1; sqrSize <= gridSize; sqrSize++)
        {
            for (var y = 0; y < gridSize - sqrSize; y++)
            for (var x = 0; x < gridSize - sqrSize; x++)
            {
                var power = GetSquareSummedArea(summedAreaTable, x, y, sqrSize);
                if (power <= pMax)
                {
                    continue;
                }
                
                (xMax, yMax) = (x, y);
                pMax = power;
                sMax = sqrSize;
            }
        }

        return $"{xMax + 1},{yMax + 1},{sMax}";
    }
    
    private static int[,] BuildPowerArray(int serial, int gridSize)
    {
        var powerCells = new int[gridSize, gridSize];
        
        for (var y = 0; y < gridSize; y++)
        for (var x = 0; x < gridSize; x++)
        {
            powerCells[y, x] = ComputePowerLevel(serial, x + 1, y + 1);
        }

        return powerCells;
    }

    private static int[,] BuildSummedAreaTable(int[,] sqrArray, int size)
    {
        var sat = new int[size, size];

        for (var y = 0; y < size; y++)
        for (var x = 0; x < size; x++)
        {
            var top  = y - 1 >= 0 ? sat[y - 1, x] : 0;
            var left   = x - 1 >= 0 ? sat[y, x - 1] : 0;
            var corner = y - 1 >= 0 && x - 1 >= 0 ? sat[y - 1, x - 1] : 0;
            
            sat[y, x] = sqrArray[y, x] + top + left - corner;
        }
        
        return sat;
    }
    
    private static int GetSquareSummedArea(int[,] sat, int x, int y, int size)
    {
        //  SAT returns the summed area above and to the left of (x,y), including (x,y) itself
        //   _____________________
        //  |      |             |  
        //  |_____A|____________B|
        //  |      |             |
        //  |_____C|____________D|
        //
        //  Therefore, Area(ABCD) = SAT(D) + SAT(A) - SAT(B) - SAT(C)
        //
        var offset = size - 1;
        var a = y > 0 && x > 0 ? sat[y - 1, x - 1] : 0;
        var b = y > 0 ? sat[y - 1, x + offset] : 0;
        var c = x > 0 ? sat[y + offset, x - 1] : 0;
        var d = sat[y + offset, x + offset];

        return d + a - b - c;
    }
    
    private static int ComputePowerLevel(int serial, int x, int y)
    {
        var rackId = x + 10;
        var power = rackId * y + serial;

        power *= rackId;
        power = power / 100 % 10 - 5;

        return power;
    }
}