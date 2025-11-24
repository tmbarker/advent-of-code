using Utilities.Collections;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D20;

using Maze = Grid2D<char>;
using AdjacencyList = Dictionary<Vec2D, HashSet<Vec2D>>;

[PuzzleInfo("Donut Maze", Topics.Graphs, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{ 
    private const char Traversable = '.';
    private static readonly PortalKey Entrance = new('A', 'A');
    private static readonly PortalKey Exit = new('Z', 'Z');

    public override object Run(int part)
    {
        var maze = GetInputGrid();
        return part switch
        {
            1 => Traverse(maze, MazeType.Static),
            2 => Traverse(maze, MazeType.Recursive),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Traverse(Maze maze, MazeType type)
    {
        var portalMap = BuildPortalMap(maze);
        var adjacencyList = BuildAdjacencyList(maze);

        var initial = new State(
            Pos: portalMap.GetEntrancePositions(key: Entrance).Single(),
            Depth: 0);
        var target = new State(
            Pos: portalMap.GetEntrancePositions(key: Exit).Single(),
            Depth: 0);
        
        var visited = new HashSet<State>(collection: [initial]);
        var heap = new PriorityQueue<State, int>(items: [(initial, 0)]);
        var costs = new DefaultDict<State, int>(defaultValue: int.MaxValue / 2, items: [(initial, 0)]);

        while (heap.Count > 0)
        {
            var current = heap.Dequeue();
            if (current == target)
            {
                return costs[current];
            }

            var possible = GetNextStates(
                current: current,
                adjacencyList: adjacencyList,
                portalMap: portalMap,
                mazeType: type);
            
            foreach (var state in possible)
            {
                if (!visited.Add(state))
                {
                    continue;
                }
                
                if (costs[current] + 1 < costs[state])
                {
                    costs[state] = costs[current] + 1;
                }
                
                heap.Enqueue(state, costs[state]);
            }
        }

        throw new NoSolutionException();
    }

    private static IEnumerable<State> GetNextStates(State current, AdjacencyList adjacencyList, PortalMap portalMap,
        MazeType mazeType)
    {
        foreach (var pos in adjacencyList[current.Pos])
        {
            yield return current with { Pos = pos };
        }

        if (!portalMap.TryTakePortal(current.Pos, out var entranceType, out var exit))
        {
            yield break;
        }

        if (mazeType == MazeType.Static)
        {
            yield return current with { Pos = exit };
            yield break;
        }

        if (entranceType == EntranceType.Inner || current.Depth != 0)
        {
            yield return new State(
                Pos: exit,
                Depth: entranceType == EntranceType.Inner ? current.Depth + 1 : current.Depth - 1);   
        }
    }

    private static AdjacencyList BuildAdjacencyList(Maze maze)
    {
        var adjacencyList = new AdjacencyList();
        
        foreach (var pos in maze)
        {
            if (maze[pos] != Traversable)
            {
                continue;
            }

            var adj = pos.GetAdjacentSet(Metric.Taxicab)
                .Where(p => PositionValid(maze, p))
                .ToHashSet();
            adjacencyList.Add(pos, adj);
        }
        
        return adjacencyList;
    }

    private static PortalMap BuildPortalMap(Maze maze)
    {
        var entrances = new DefaultDict<PortalKey, List<PortalEntrance>>(defaultSelector: _ => []);
        var directions = new HashSet<Vec2D> { Vec2D.Down, Vec2D.Right };

        foreach (var pos in maze)
        {
            if (!char.IsLetter(maze[pos]))
            {
                continue;
            }

            foreach (var dir in directions)
            {
                if (!maze.Contains(pos + dir) || !char.IsLetter(maze[pos + dir]))
                {
                    continue;
                }

                var portal = new PortalKey(maze[pos], maze[pos + dir]);
                var entranceType = !maze.Contains(pos - dir) || !maze.Contains(pos + dir + dir)
                    ? EntranceType.Outer
                    : EntranceType.Inner;
                var entrancePos = PositionValid(maze,pos - dir) 
                    ? pos - dir 
                    : pos + dir + dir;
                
                entrances[portal].Add(item: new PortalEntrance(
                    Pos: entrancePos,
                    Type: entranceType));
                break;
            }
        }

        return new PortalMap(entrances);
    }
    
    private static bool PositionValid(Maze maze, Vec2D pos)
    {
        return maze.Contains(pos) && maze[pos] == Traversable;
    }
}