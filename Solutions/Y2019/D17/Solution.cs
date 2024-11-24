using System.Text;
using Solutions.Y2019.IntCode;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D17;

[PuzzleInfo("Set and Forget", Topics.IntCode|Topics.Vectors, Difficulty.Medium)]
public sealed class Solution : IntCodeSolution
{
    private const char Scaffold = '#';
    
    private static readonly Dictionary<char, Vec2D> Directions = new()
    {
        //  NOTE: The UV coordinate system is used for this puzzle, while the Vec2D static vectors are XY
        // 
        { 'v', Vec2D.Up },
        { '^', Vec2D.Down },
        { '<', Vec2D.Left },
        { '>', Vec2D.Right }
    };
    private static readonly Dictionary<char, Rot3D> TurnCommands = new()
    {
        { 'L', Rot3D.N90Z },
        { 'R', Rot3D.P90Z }
    };

    public override object Run(int part)
    {
        var ascii = GetCameraOutput();
        var state = ParseCameraOutput(ascii);
        
        return part switch
        {
            1 => SumAlignmentParams(state.Scaffolding),
            2 => GetCollectedDust(state.Scaffolding, state.Robot),
            _ => PuzzleNotSolvedString
        };
    }

    private static int SumAlignmentParams(IReadOnlySet<Vec2D> positions)
    {
        return positions
            .Where(p => p.GetAdjacentSet(Metric.Taxicab).Count(positions.Contains) == 4)
            .Aggregate(0, (sum, pos) => sum + pos.X * pos.Y);
    }

    private long GetCollectedDust(IReadOnlySet<Vec2D> positions, Pose2D pose)
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

        var ec = robot.Run(); 
        return ec == IntCodeVm.ExitCode.Halted
            ? robot.OutputBuffer.Last()
            : throw new NoSolutionException(message: $"Invalid VM exit code [{ec}]");
    }

    private static List<string> ComputeCommands(IReadOnlySet<Vec2D> positions, Pose2D pose)
    {
        var commands = new List<string>();
        var visited = new HashSet<Vec2D>(collection: [pose.Pos]);

        while (visited.Count < positions.Count)
        {
            foreach (var (cmd, turn) in TurnCommands)
            {
                if (!positions.Contains(pose.Turn(turn).Step().Pos))
                {
                    continue;
                }
                
                pose = pose.Turn(turn);
                commands.Add(cmd.ToString());
                break;
            }

            var steps = 0;
            while (positions.Contains(pose.Step().Pos))
            {
                steps++;
                pose = pose.Step();
                visited.Add(pose.Pos);
            }
            
            commands.Add(steps.ToString());
        }

        return commands;
    }

    private static State ParseCameraOutput(string ascii)
    {
        var scaffolding = new HashSet<Vec2D>();
        var robotPose = new Pose2D(Pos:Vec2D.Zero, Face:Vec2D.Zero);

        var rows = ascii.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var cols = rows[0].Length;

        for (var y = 0; y < rows.Length; y++)
        for (var x = 0; x < cols; x++)
        {
            var chr = rows[y][x];
            var pos = new Vec2D(x, y);

            if (Directions.TryGetValue(chr, out var direction))
            {
                scaffolding.Add(pos);
                robotPose = new Pose2D(pos, direction);
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
        while (vm.OutputBuffer.Count > 0)
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