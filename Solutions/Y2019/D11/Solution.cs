using System.Text;
using Solutions.Y2019.IntCode;
using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D11;

[PuzzleInfo("Space Police", Topics.IntCode|Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : IntCodeSolution
{
    private const long Black = 0L;
    private const long White = 1L;

    private static readonly Vec2D InitialRobotPos = Vec2D.Zero;
    private static readonly Vec2D InitialRobotFacing = Vec2D.Up;
    
    private static readonly Dictionary<long, Quaternion> OutputRotations = new()
    {
        { 0L, Rot3D.P90Z },
        { 1L, Rot3D.N90Z }
    };
    private static readonly Dictionary<long, char> DrawChars = new()
    {
        { Black, '.' },
        { White, '#' }
    };

    public override object Run(int part)
    {
        var program = LoadIntCodeProgram();
        return part switch
        {
            1 => RunRobot(program, startColour: Black).Count,
            2 => Draw(RunRobot(program, startColour: White)),
            _ => PuzzleNotSolvedString
        };
    }

    private static DefaultDict<Vec2D, long> RunRobot(IList<long> intCodeProgram, long startColour)
    {
        var robot = IntCodeVm.Create(intCodeProgram);
        var painted = new DefaultDict<Vec2D, long>(defaultValue: Black) 
            { { InitialRobotPos, startColour } };
        
        var pos = InitialRobotPos;
        var facing = InitialRobotFacing;

        while (true)
        {
            robot.InputBuffer.Enqueue(painted[pos]);
            if (robot.Run() == IntCodeVm.ExitCode.Halted)
            {
                break;
            }
            
            var color = robot.OutputBuffer.Dequeue();
            var rotKey = robot.OutputBuffer.Dequeue();

            painted[pos] = color;
            facing = OutputRotations[rotKey].Transform(facing);
            pos += facing;
        }

        return painted;
    }

    private static string Draw(DefaultDict<Vec2D, long> painted)
    {
        var white = painted.Keys
            .Where(c => painted[c] == White)
            .Normalize()
            .ToHashSet();

        var aabb = new Aabb2D(extents: white);
        var image = new StringBuilder();

        for (var y = aabb.Height - 1; y >= 0; y--)
        {
            image.Append('\n');
            for (var x = 0; x < aabb.Width; x++)
            {
                image.Append(white.Contains(new Vec2D(x, y)) 
                    ? DrawChars[White] 
                    : DrawChars[Black]);
            }
        }
        
        return image.ToString();
    }
}