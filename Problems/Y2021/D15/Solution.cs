using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2021.D15;

/// <summary>
/// Chiton: https://adventofcode.com/2021/day/15
/// </summary>
public class Solution : SolutionBase
{
    private const int MaxRisk = 9;

    public override object Run(int part)
    {
        return part switch
        {
            1 => FindLeastRiskyPath(tilesPerSide: 1),
            2 => FindLeastRiskyPath(tilesPerSide: 5),
            _ => ProblemNotSolvedString
        };
    }

    private int FindLeastRiskyPath(int tilesPerSide)
    {
        ParseRiskMap(GetInputLines(), tilesPerSide, out var riskMap, out var start, out var end);

        var visited = new HashSet<Vector2D> { start };
        var heap = new PriorityQueue<Vector2D, int>(new[] { (start, 0) });
        var risks = riskMap.GetAllPositions().ToDictionary(
            keySelector: p => p, 
            elementSelector: p => p == start ? 0 : int.MaxValue);
        
        while (heap.Count > 0)
        {
            var current = heap.Dequeue();
            if (current == end)
            {
                return risks[end];
            }
            
            foreach (var move in GetMoves(current, riskMap))
            {
                if (visited.Contains(move))
                {
                    continue;
                }
                
                var riskViaCurrent = risks[current] + riskMap[move]; 
                if (riskViaCurrent < risks[move])
                {
                    risks[move] = riskViaCurrent;
                }
                
                visited.Add(move);
                heap.Enqueue(move, risks[move]);
            }
        }

        throw new NoSolutionException();
    }

    private static IEnumerable<Vector2D> GetMoves(Vector2D current, Grid2D<int> map)
    {
        return current
            .GetAdjacentSet(Metric.Taxicab)
            .Where(map.IsInDomain);
    }

    private static void ParseRiskMap(IList<string> input, int tilesPerSide, out Grid2D<int> map, out Vector2D s, out Vector2D e)
    {
        var rowsPerTile = input.Count;
        var colsPerTile = input[0].Length;
        var totalRows = tilesPerSide * rowsPerTile;
        var totalCols = tilesPerSide * colsPerTile;

        map = Grid2D<int>.WithDimensions(totalRows, totalCols);
        s = new Vector2D(0, totalRows - 1);
        e = new Vector2D(totalCols - 1, 0);
        
       for (var y = totalRows - 1; y >= 0; y--)
       for (var x = 0; x < totalCols; x++)
       {
           var xInTile = x % colsPerTile;
           var yInTile = y % rowsPerTile;

           var xTileIndex = (x - xInTile) / colsPerTile;
           var yTileIndex = tilesPerSide - 1 - (y - yInTile) / rowsPerTile;

           var rawRisk = input[rowsPerTile - yInTile - 1][xInTile].AsDigit();
           var unclampedRisk = rawRisk + xTileIndex + yTileIndex;

           map[x, y] = unclampedRisk > MaxRisk
               ? unclampedRisk % MaxRisk
               : unclampedRisk;
       }
    }
}