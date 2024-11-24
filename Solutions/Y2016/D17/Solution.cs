using Utilities.Geometry.Euclidean;
using Utilities.Hashing;

namespace Solutions.Y2016.D17;

[PuzzleInfo("Two Steps Forward", Topics.Vectors|Topics.Graphs, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Vec2D Start  = new(X: 0, Y: 3);
    private static readonly Vec2D Target = new(X: 3, Y: 0);
    private static readonly HashSet<char> OpenSet = ['b', 'c', 'd', 'e', 'f'];
    
    private static readonly Dictionary<int, Vec2D> IndicesMap = new()
    {
        { 0, Vec2D.Up },
        { 1, Vec2D.Down },
        { 2, Vec2D.Left },
        { 3, Vec2D.Right }
    };
    
    private static readonly Dictionary<Vec2D, char> PathCharsMap = new()
    {
        { Vec2D.Up,    'U' },
        { Vec2D.Down,  'D' },
        { Vec2D.Left,  'L' },
        { Vec2D.Right, 'R' }
    };

    public override object Run(int part)
    {
        return part switch
        {
            1 => Traverse(criteria: PathCriteria.Shortest),
            2 => Traverse(criteria: PathCriteria.Longest),
            _ => PuzzleNotSolvedString
        };
    }

    private string Traverse(PathCriteria criteria)
    {
        using var hashProvider = new Md5Provider();
        var passcode = GetInputText();

        var initial = new State(Pos: Start, Path: string.Empty);
        var queue = new Queue<State>(collection: [initial]);
        var visited = new HashSet<State>(collection: [initial]);
        var longest = 0;

        while (queue.Count != 0)
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

    private static bool InBounds(Vec2D pos)
    {
        return pos.X is >= 0 and < 4 && pos.Y is >= 0 and < 4;
    }
}