using System.Text;
using Problems.Y2019.IntCode;
using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2019.D11;

/// <summary>
/// Space Police: https://adventofcode.com/2019/day/11
/// </summary>
public class Solution : IntCodeSolution
{
    private const long Black = 0L;
    private const long White = 1L;

    private static readonly Vector2D InitialRobotPos = Vector2D.Zero;
    private static readonly Vector2D InitialRobotFacing = Vector2D.Up;
    
    private static readonly Dictionary<long, Rotation3D> OutputRotations = new()
    {
        { 0L, Rotation3D.Positive90Z },
        { 1L, Rotation3D.Negative90Z }
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
            _ => ProblemNotSolvedString
        };
    }

    private static DefaultDictionary<Vector2D, long> RunRobot(IList<long> intCodeProgram, long startColour)
    {
        var robot = IntCodeVm.Create(intCodeProgram);
        var painted = new DefaultDictionary<Vector2D, long>(defaultValue: Black) 
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
            facing = OutputRotations[rotKey] * facing;
            pos += facing;
        }

        return painted;
    }

    private static string Draw(IDictionary<Vector2D, long> painted)
    {
        var white = painted
            .WhereValues(c => c == White).Keys
            .Normalize()
            .ToHashSet();

        var aabb = new Aabb2D(extents: white);
        var image = new StringBuilder();

        for (var y = aabb.Height - 1; y >= 0; y--)
        {
            image.Append('\n');
            for (var x = 0; x < aabb.Width; x++)
            {
                image.Append(white.Contains(new Vector2D(x, y)) 
                    ? DrawChars[White] 
                    : DrawChars[Black]);
            }
        }
        
        return image.ToString();
    }
}