using Problems.Common;
using Utilities.Cartesian;
using Utilities.Hashing;

namespace Problems.Y2016.D17;

/// <summary>
/// Two Steps Forward: https://adventofcode.com/2016/day/17
/// </summary>
public class Solution : SolutionBase
{
    private static readonly Vector2D Start  = new(x: 0, y: 3);
    private static readonly Vector2D Target = new(x: 3, y: 0);
    private static readonly HashSet<char> OpenSet = new() { 'b', 'c', 'd', 'e', 'f' };
    
    private static readonly Dictionary<int, Vector2D> IndicesMap = new()
    {
        { 0, Vector2D.Up },
        { 1, Vector2D.Down },
        { 2, Vector2D.Left },
        { 3, Vector2D.Right }
    };
    
    private static readonly Dictionary<Vector2D, char> PathCharsMap = new()
    {
        { Vector2D.Up,    'U' },
        { Vector2D.Down,  'D' },
        { Vector2D.Left,  'L' },
        { Vector2D.Right, 'R' }
    };

    public override object Run(int part)
    {
        return part switch
        {
            1 => Traverse(criteria: PathCriteria.Shortest),
            2 => Traverse(criteria: PathCriteria.Longest),
            _ => ProblemNotSolvedString
        };
    }

    private string Traverse(PathCriteria criteria)
    {
        var passcode = GetInputText();
        var hashProvider = new Md5Provider();

        var initial = new State(Pos: Start, Path: string.Empty);
        var queue = new Queue<State>(new[] { initial });
        var visited = new HashSet<State> { initial };
        var longest = 0;

        while (queue.Any())
        {
            var state = queue.Dequeue();
            if (state.Pos == Target)
            {
                if (criteria == PathCriteria.Shortest)
                {
                    return state.Path;   
                }

                longest = Math.Max(longest, state.Path.Length);
                continue;
            }

            var input = string.Concat(passcode, state.Path);
            var hash = hashProvider.GetHashHex(input);
            
            foreach (var (index, dir) in IndicesMap)
            {
                if (!OpenSet.Contains(hash[index]))
                {
                    continue;
                }

                var nextPos = state.Pos + dir;
                var nextPath = state.Path + PathCharsMap[dir];
                var nextState = new State(Pos: nextPos, Path: nextPath);

                if (!InBounds(nextState.Pos) || visited.Contains(nextState))
                {
                    continue;
                }
                
                queue.Enqueue(nextState);
                visited.Add(nextState);
            }
        }

        return longest.ToString();
    }

    private static bool InBounds(Vector2D pos)
    {
        return pos.X is >= 0 and < 4 && pos.Y is >= 0 and < 4;
    }
}