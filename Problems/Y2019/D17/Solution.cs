using System.Text;
using Problems.Common;
using Problems.Y2019.Common;
using Problems.Y2019.IntCode;
using Utilities.Cartesian;

namespace Problems.Y2019.D17;

/// <summary>
/// Set and Forget: https://adventofcode.com/2019/day/17
/// </summary>
public class Solution : SolutionBase2019
{
    private const char Scaffold = '#';
    
    private static readonly Dictionary<char, Vector2D> Directions = new()
    {
        // NOTE: The UV coordinate system is used for this problem, while the Vector2D static vectors are XY
        { 'v', Vector2D.Up },
        { '^', Vector2D.Down },
        { '<', Vector2D.Left },
        { '>', Vector2D.Right },
    };
    private static readonly Dictionary<char, Rotation3D> TurnCommands = new()
    {
        { 'L', Rotation3D.Negative90Z },
        { 'R', Rotation3D.Positive90Z }
    };

    public override int Day => 17;
    
    public override object Run(int part)
    {
        var ascii = GetCameraOutput();
        var state = ParseCameraOutput(ascii);
        
        return part switch
        {
            1 => SumAlignmentParams(state.Scaffolding),
            2 => GetCollectedDust(state.Scaffolding, state.Robot),
            _ => ProblemNotSolvedString
        };
    }

    private static int SumAlignmentParams(IReadOnlySet<Vector2D> positions)
    {
        return positions
            .Where(p => p.GetAdjacentSet(DistanceMetric.Taxicab).Count(positions.Contains) == 4)
            .Aggregate(0, (sum, pos) => sum + pos.X * pos.Y);
    }

    private long GetCollectedDust(IReadOnlySet<Vector2D> positions, Pose pose)
    {
        var robot = IntCodeVm.Create(LoadRobotProgram());
        var commands = ComputeCommands(positions, pose);

        foreach (var line in RoutineBuilder.Build(commands, false))
        {
            foreach (var chr in line)
            {
                robot.InputBuffer.Enqueue(chr);
            }
        }
        
        return robot.Run() == IntCodeVm.ExitCode.Halted
            ? robot.OutputBuffer.Last()
            : throw new NoSolutionException();
    }

    private static IEnumerable<string> ComputeCommands(IReadOnlySet<Vector2D> positions, Pose pose)
    {
        var commands = new List<string>();
        var visited = new HashSet<Vector2D> { pose.Position };

        while (visited.Count < positions.Count)
        {
            foreach (var (cmd, turn) in TurnCommands)
            {
                if (!positions.Contains(pose.Turn(turn).Step().Position))
                {
                    continue;
                }
                
                pose = pose.Turn(turn);
                commands.Add(cmd.ToString());
                break;
            }

            var steps = 0;
            while (positions.Contains(pose.Step().Position))
            {
                steps++;
                pose = pose.Step();
                visited.Add(pose.Position);
            }
            
            commands.Add(steps.ToString());
        }

        return commands;
    }

    private static State ParseCameraOutput(string ascii)
    {
        var scaffolding = new HashSet<Vector2D>();
        var robotPose = new Pose(Vector2D.Zero, Vector2D.Zero);

        var rows = ascii.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var cols = rows[0].Length;

        for (var y = 0; y < rows.Length; y++)
        for (var x = 0; x < cols; x++)
        {
            var chr = rows[y][x];
            var pos = new Vector2D(x, y);

            if (Directions.ContainsKey(chr))
            {
                scaffolding.Add(pos);
                robotPose = new Pose(pos, Directions[chr]);
            }
            
            if (chr == Scaffold)
            {
                scaffolding.Add(pos);
            }
        }

        return new State(scaffolding, robotPose);
    }

    private string GetCameraOutput()
    {
        var vm = IntCodeVm.Create(LoadIntCodeProgram());
        vm.Run();
        return ReadAsciiOutput(vm);
    }

    private static string ReadAsciiOutput(IntCodeVm vm)
    {
        var sb = new StringBuilder();
        while (vm.OutputBuffer.Any())
        {
            sb.Append((char)vm.OutputBuffer.Dequeue());
        }

        return sb.ToString();
    }
    
    private IList<long> LoadRobotProgram()
    {
        var unmodified = LoadIntCodeProgram();
        unmodified[0] = 2L;
        return unmodified;
    }
}