using Solutions.Y2019.IntCode;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D19;

[PuzzleInfo("Tractor Beam", Topics.IntCode, Difficulty.Medium)]
public sealed class Solution : IntCodeSolution
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountBeamPoints(searchDimension: 50),
            2 => FindShip(shipSize: 100),
            _ => PuzzleNotSolvedString
        };
    }

    private long FindShip(int shipSize)
    {
        var program = LoadIntCodeProgram();
        var x = 0;
        var y = 0;

        while (!CheckPointInBeam(x + shipSize - 1, y, program))
        {
            y += 1;
            while (!CheckPointInBeam(x, y + shipSize - 1, program))
            {
                x += 1;
            }
        }

        return 10000L * x + y;
    }

    private int CountBeamPoints(int searchDimension)
    {
        var program = LoadIntCodeProgram();
        var searchArea = new Aabb2D(
            xMin: 0,
            xMax: searchDimension - 1,
            yMin: 0,
            yMax: searchDimension - 1);

        return searchArea.Sum(point => CheckPointInBeam(point.X, point.Y, program) ? 1 : 0);
    }

    private static bool CheckPointInBeam(int x, int y, IList<long> program)
    {
        var inputs = new long[] { x, y };
        var vm = IntCodeVm.Create(program, inputs);
        
        vm.Run();
        return vm.OutputBuffer.Dequeue() > 0L;
    }
}