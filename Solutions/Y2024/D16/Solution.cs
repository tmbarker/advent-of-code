using System.Collections.Immutable;
using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2024.D16;

using Path = ImmutableStack<Vec2D>;

[PuzzleInfo("Reindeer Maze", Topics.Vectors|Topics.Graphs, Difficulty.Easy)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Result(int MinScore, List<Path> BestPaths);
    private readonly record struct State(Pose2D Pose, int Score, Path Path)
    {
        public State Step()  => new(Pose.Step(), Score + 1, Path.Push(Pose.Ahead));
        public State Left()  => new(Pose.Turn(Rot3D.P90Z), Score + 1000, Path);
        public State Right() => new(Pose.Turn(Rot3D.N90Z), Score + 1000, Path);
    }
    
    public override object Run(int part)
    {
        var grid = GetInputGrid();
        var result = Navigate(grid);

        return part == 1
            ? result.MinScore
            : result.BestPaths
                .SelectMany(path => path)
                .Distinct()
                .Count();
    }
    
    private static Result Navigate(Grid2D<char> grid)
    {
        var pose = new Pose2D(Pos: grid.Find('S'), Face: Vec2D.Right);
        var seed = new State(pose, Score: 0, Path: [pose.Pos]);
        var queue = new Queue<State>([seed]);
        
        var bestScore = int.MaxValue;
        var bestPaths = new List<Path>();
        var minScores = new DefaultDict<Pose2D, int>(defaultValue: int.MaxValue);
        
        while (queue.Count != 0)
        {
            var state = queue.Dequeue();
            if (state.Score > bestScore)
            {
                continue;
            }
            
            if (grid[state.Pose.Pos] == 'E')
            {
                if (state.Score == bestScore)
                {
                    bestPaths.Add(state.Path);
                }
                else if (state.Score < bestScore)
                {
                    bestPaths = [state.Path];
                    bestScore = state.Score;
                }
                continue;
            }

            if (grid[state.Pose.Ahead] != '#') EnqueueIfBetter(state.Step());
            if (grid[state.Pose.Left]  != '#') EnqueueIfBetter(state.Left());
            if (grid[state.Pose.Right] != '#') EnqueueIfBetter(state.Right());
        }

        return new Result(bestScore, bestPaths);
        
        void EnqueueIfBetter(State candidate)
        {
            if (minScores[candidate.Pose] >= candidate.Score)
            {
                minScores[candidate.Pose] = candidate.Score;
                queue.Enqueue(candidate);
            }
        }
    }
}