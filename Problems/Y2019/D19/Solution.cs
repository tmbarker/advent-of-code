using Problems.Y2019.IntCode;
using Utilities.Cartesian;

namespace Problems.Y2019.D19;

/// <summary>
/// Tractor Beam: https://adventofcode.com/2019/day/19
/// </summary>
public class Solution : IntCodeSolution
{
    private const int SearchSize = 50;
    private const int ShipSize = 100;

    public override object Run(int part)
    {
        return part switch
        {
            1 => CountBeamPoints(SearchSize),
            2 => FindShip(ShipSize),
            _ => ProblemNotSolvedString
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
        var searchArea = new Aabb2D(0, searchDimension - 1, 0, searchDimension - 1);

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