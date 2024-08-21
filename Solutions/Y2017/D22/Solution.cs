using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D22;

[PuzzleInfo("Sporifica Virus", Topics.Vectors|Topics.Simulation, Difficulty.Medium, favourite: true)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<State, Func<Pose2D, Pose2D>> Behaviors = new()
    {
        { State.Clean,    pose => pose.Turn(rot:Rot3D.P90Z) },
        { State.Weakened, pose => pose },
        { State.Infected, pose => pose.Turn(rot:Rot3D.N90Z) },
        { State.Flagged,  pose => pose.Turn(rot:Rot3D.P180Z) }
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var grid = ParseGrid(input);
        var pose = new Pose2D(pos: Vec2D.Zero, face: Vec2D.Up);
        
        return part switch
        {
            1 => CountInfected(grid, pose, strength: 2, bursts: 10000),
            2 => CountInfected(grid, pose, strength: 1, bursts: 10000000),
            _ => PuzzleNotSolvedString
        };
    }

    private static int CountInfected(IDictionary<Vec2D, State> grid, Pose2D pose, int strength, int bursts)
    {
        var count = 0;
        for (var i = 0; i < bursts; i++)
        {
            var pos = pose.Pos;
            var state = grid[pos];
            var next = (State)(((int)state + strength) % 4);
            
            if (next == State.Infected)
            {
                count++;
            }
            
            grid[pos] = next;
            pose = Behaviors[state](pose).Step();
        }
        
        return count;
    }
    
    private static DefaultDict<Vec2D, State> ParseGrid(IReadOnlyList<string> input)
    {
        var grid = new DefaultDict<Vec2D, State>(defaultValue: State.Clean);
        var height = input.Count;
        var width = input[0].Length;

        var yOffset = height / 2;
        var xOffset = width / 2;
        
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            grid[new Vec2D(x: x - xOffset, y: y - yOffset)] =
                input[height - y - 1][x] == '#' ? State.Infected : State.Clean;
        }
        
        return grid;
    }
}