using Utilities.Geometry.Euclidean;

namespace Solutions.Y2023.D10;

[PuzzleInfo("Pipe Maze", Topics.Graphs, Difficulty.Hard)]
public class Solution : SolutionBase
{
    private readonly record struct Map(HashSet<Vec2D> LoopPositions, int MaxDepth);
    
    private static readonly HashSet<char> CornerPipes = ['L', 'J', '7', 'F'];
    private static readonly Dictionary<char, HashSet<Vec2D>> PipeAdjacency = new()
    {
        { '|', [Vec2D.Down,  Vec2D.Up] },
        { '-', [Vec2D.Right, Vec2D.Left] },
        { 'L', [Vec2D.Right, Vec2D.Up] },
        { 'J', [Vec2D.Left,  Vec2D.Up] },
        { '7', [Vec2D.Down,  Vec2D.Left] },
        { 'F', [Vec2D.Down,  Vec2D.Right] }
    };

    public override object Run(int part)
    {
        var input = GetInputLines();
        var maze = ParseMaze(input, out var start);
        
        return part switch
        {
            1 => Traverse(maze, start).MaxDepth,
            2 => CountEnclosed(maze, start),
            _ => ProblemNotSolvedString
        };
    }
    
    private static Map Traverse(Grid2D<char> maze, Vec2D start)
    {
        var queue = new Queue<Vec2D>(collection: [start]);
        var visited = new HashSet<Vec2D>(collection: [start]);
        var depth = -1;

        while (queue.Count > 0)
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                var adjacent = PipeAdjacency[maze[pos]].Select(adj => pos + adj);

                foreach (var adj in adjacent.Where(adj => visited.Add(adj)))
                {
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        return new Map(LoopPositions: visited, MaxDepth: depth);
    }

    private static int CountEnclosed(Grid2D<char> maze, Vec2D start)
    {
        var map = Traverse(maze, start);
        var loopBounds = new Aabb2D(extents: map.LoopPositions);
        var enclosed = new HashSet<Vec2D>();

        //  From the top left-most position in the loop, walk the loop in a CW direction. Any
        //  adjacent position on our right not part of the loop itself must be "inside" the loop.
        //
        var topLeft = map.LoopPositions.Where(pos => pos.Y == loopBounds.Max.Y).MinBy(pos => pos.X);
        var pose = new Pose2D(pos: topLeft, face: Vec2D.Right);
        
        while (pose.Ahead != topLeft)
        {
            //  Applying the above method requires doing the following at each position during the CW walk:
            //  1. Check
            //  2. Step
            //  3. Check
            //  4. Turn (conditionally)
            //
            if (!map.LoopPositions.Contains(pose.Right))
            {
                enclosed.Add(pose.Right);
            }
            
            pose = pose.Step();
            
            if (!map.LoopPositions.Contains(pose.Right))
            {
                enclosed.Add(pose.Right);
            }
            
            if (CornerPipes.Contains(maze[pose.Pos]))
            {
                pose = new Pose2D(
                    pos: pose.Pos,
                    face: PipeAdjacency[maze[pose.Pos]].Single(adj => pose.Pos + adj != pose.Behind));
            }
        }

        var queue = new Queue<Vec2D>(collection: enclosed);
        while (queue.Count > 0)
        {
            var pos = queue.Dequeue();
            var adjacent = pos
                .GetAdjacentSet(Metric.Taxicab)
                .Where(adj => !map.LoopPositions.Contains(adj));

            foreach (var adj in adjacent.Where(adj => enclosed.Add(adj)))
            {
                queue.Enqueue(adj);
            }
        }

        return enclosed.Count;
    }
    
    private static Grid2D<char> ParseMaze(IList<string> input, out Vec2D start)
    {
        start = Vec2D.PositiveInfinity;
        
        var maze = Grid2D<char>.MapChars(input);
        for (var y = 0; y < maze.Height; y++)
        for (var x = 0; x < maze.Width; x++)
        {
            if (maze[x, y] == 'S')
            {
                start = new Vec2D(x, y);
            }
        }
        
        var adjNaive = start.GetAdjacentSet(Metric.Taxicab);
        var adjPipes = adjNaive.Where(adj => maze.Contains(adj) && PipeAdjacency.ContainsKey(maze[adj]));
        var adjDirs = new HashSet<Vec2D>();
        
        foreach (var pos in adjPipes)
        foreach (var dir in PipeAdjacency[maze[pos]])
        {
            if (pos + dir == start)
            {
                adjDirs.Add(pos - start);
            }
        }
        
        maze[start] = PipeAdjacency.Keys.Single(type => PipeAdjacency[type].All(adjDirs.Contains));
        return maze;
    }
}