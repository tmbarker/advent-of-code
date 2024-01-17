using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2021.D15;

[PuzzleInfo("Chiton", Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
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

        var visited = new HashSet<Vec2D>(collection: [start]);
        var heap = new PriorityQueue<Vec2D, int>(items: [(start, 0)]);
        var risks = riskMap.ToDictionary(
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

    private static IEnumerable<Vec2D> GetMoves(Vec2D current, Grid2D<int> map)
    {
        return current
            .GetAdjacentSet(Metric.Taxicab)
            .Where(map.Contains);
    }

    private static void ParseRiskMap(IList<string> input, int tilesPerSide, out Grid2D<int> map, out Vec2D s, out Vec2D e)
    {
        var rowsPerTile = input.Count;
        var colsPerTile = input[0].Length;
        var totalRows = tilesPerSide * rowsPerTile;
        var totalCols = tilesPerSide * colsPerTile;

        map = Grid2D<int>.WithDimensions(totalRows, totalCols);
        s = new Vec2D(0, totalRows - 1);
        e = new Vec2D(totalCols - 1, 0);
        
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