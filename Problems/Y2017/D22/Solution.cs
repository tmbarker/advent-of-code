using Problems.Attributes;
using Problems.Common;
using Utilities.Cartesian;
using Utilities.Collections;

namespace Problems.Y2017.D22;

/// <summary>
/// Sporifica Virus: https://adventofcode.com/2017/day/22
/// </summary>
[Favourite("Sporifica Virus", Topics.Vectors, Difficulty.Medium)]
public class Solution : SolutionBase
{
    private static readonly Dictionary<State, Func<Pose2D, Pose2D>> Behaviors = new()
    {
        { State.Clean,    pose => pose.Turn(rot:Rotation3D.Positive90Z) },
        { State.Weakened, pose => pose },
        { State.Infected, pose => pose.Turn(rot:Rotation3D.Negative90Z) },
        { State.Flagged,  pose => pose.Turn(rot:Rotation3D.Positive180Z) }
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var grid = ParseGrid(input);
        var pose = new Pose2D(pos: Vector2D.Zero, face: Vector2D.Up);
        
        return part switch
        {
            1 => CountInfected(grid, pose, strength: 2, bursts: 10000),
            2 => CountInfected(grid, pose, strength: 1, bursts: 10000000),
            _ => ProblemNotSolvedString
        };
    }

    private static int CountInfected(IDictionary<Vector2D, State> grid, Pose2D pose, int strength, int bursts)
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
    
    private static DefaultDictionary<Vector2D, State> ParseGrid(IReadOnlyList<string> input)
    {
        var grid = new DefaultDictionary<Vector2D, State>(defaultValue: State.Clean);
        var height = input.Count;
        var width = input[0].Length;

        var yOffset = height / 2;
        var xOffset = width / 2;
        
        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
        {
            grid[new Vector2D(x: x - xOffset, y: y - yOffset)] =
                input[height - y - 1][x] == '#' ? State.Infected : State.Clean;
        }
        
        return grid;
    }
}