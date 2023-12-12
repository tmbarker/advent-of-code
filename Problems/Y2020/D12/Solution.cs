using Utilities.Geometry.Euclidean;

namespace Problems.Y2020.D12;

[PuzzleInfo("Rain Risk", Topics.Vectors, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Instruction(char Command, int Amount);
    
    private const char North = 'N';
    private const char South = 'S';
    private const char East = 'E';
    private const char West = 'W';
    private const char Forward = 'F';
    private const char Left = 'L';
    private const char Right = 'R';

    private static readonly Vector2D InitialShipPos = Vector2D.Zero;
    private static readonly Vector2D InitialShipFacing = Vector2D.Right;
    private static readonly Vector2D InitialWaypointPos = new(10, 1);
    private static readonly Dictionary<char, Vector2D> DirectionVectors = new()
    {
        { North, Vector2D.Up },
        { South, Vector2D.Down },
        { East, Vector2D.Right },
        { West, Vector2D.Left }
    };

    public override object Run(int part)
    {
        var instructions = ParseInputLines(parseFunc: ParseInstruction);
        return part switch
        {
            1 => NavigateSimple(instructions),
            2 => NavigateWaypoint(instructions),
            _ => ProblemNotSolvedString
        };
    }

    private static int NavigateSimple(IEnumerable<Instruction> instructions)
    {
        var pos = InitialShipPos;
        var facing = InitialShipFacing;
        
        foreach (var (command, amount) in instructions)
        {
            switch (command)
            {
                case North:
                case South:
                case East:
                case West:
                    pos += amount * DirectionVectors[command];
                    continue;
                case Forward:
                    pos += amount * facing;
                    continue;
                case Left:
                    facing = new Rotation3D(Axis.Z, amount) * facing;
                    continue;
                case Right:
                    facing = new Rotation3D(Axis.Z, -amount) * facing;
                    continue;
            }
        }

        return Vector2D.Distance(InitialShipPos, pos, Metric.Taxicab);
    }
    
    private static int NavigateWaypoint(IEnumerable<Instruction> instructions)
    {
        var waypointPos = InitialWaypointPos;
        var shipPos = InitialShipPos;
        
        foreach (var (command, amount) in instructions)
        {
            switch (command)
            {
                case North:
                case South:
                case East:
                case West:
                    waypointPos += amount * DirectionVectors[command];
                    continue;
                case Forward:
                    shipPos += amount * waypointPos;
                    continue;
                case Left:
                    waypointPos = new Rotation3D(Axis.Z, amount) * waypointPos;
                    continue;
                case Right:
                    waypointPos = new Rotation3D(Axis.Z, -amount) * waypointPos;
                    continue;
            }
        }

        return Vector2D.Distance(InitialShipPos, shipPos, Metric.Taxicab);
    }

    private static Instruction ParseInstruction(string line)
    {
        return new Instruction(
            Command: line[0],
            Amount: int.Parse(line[1..]));
    }
}