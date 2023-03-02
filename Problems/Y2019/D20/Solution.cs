using Problems.Attributes;
using Problems.Common;
using Problems.Y2019.Common;
using Utilities.Cartesian;
using Utilities.Extensions;

namespace Problems.Y2019.D20;

using Maze = Grid2D<char>;
using AdjacencyList = Dictionary<Vector2D, HashSet<Vector2D>>;

/// <summary>
/// Donut Maze: https://adventofcode.com/2019/day/20
/// </summary>
[Favourite("Donut Maze", Topics.Graphs, Difficulty.Hard)]
public class Solution : SolutionBase2019
{ 
    private const char Traversable = '.';
    private static readonly PortalKey Entrance = new('A', 'A');
    private static readonly PortalKey Exit = new('Z', 'Z');

    public override int Day => 20;
    
    public override object Run(int part)
    {
        var maze = ParseMaze();
        return part switch
        {
            1 =>  Traverse(maze, MazeType.Static),
            2 =>  Traverse(maze, MazeType.Recursive),
            _ => ProblemNotSolvedString
        };
    }

    private static int Traverse(Maze maze, MazeType type)
    {
        var portalMap = BuildPortalMap(maze);
        var adjacencyList = BuildAdjacencyList(maze);

        var initial = new State(
            Pos: portalMap.GetEntrancePositions(Entrance).Single(),
            Depth: 0);
        var target = new State(
            Pos: portalMap.GetEntrancePositions(Exit).Single(),
            Depth: 0);
        
        var visited = new HashSet<State> { initial };
        var heap = new PriorityQueue<State, int>(new[] { (start: initial, 0) });
        var costs = new Dictionary<State, int> { { initial, 0 } };

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
                if (visited.Contains(state))
                {
                    continue;
                }

                costs.EnsureContainsKey(state, int.MaxValue);
                
                var distanceViaCurrent = costs[current] + 1;
                if (distanceViaCurrent < costs[state])
                {
                    costs[state] = distanceViaCurrent;
                }

                visited.Add(state);
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
        
        foreach (var (pos, chr) in maze)
        {
            if (chr != Traversable)
            {
                continue;
            }

            var adj = pos.GetAdjacentSet(DistanceMetric.Taxicab)
                .Where(p => PositionValid(maze, p))
                .ToHashSet();
            adjacencyList.Add(pos, adj);
        }
        
        return adjacencyList;
    }

    private static PortalMap BuildPortalMap(Maze maze)
    {
        var entrances = new Dictionary<PortalKey, IList<PortalEntrance>>();
        var directions = new HashSet<Vector2D> { Vector2D.Down, Vector2D.Right };

        foreach (var (pos, chr) in maze)
        {
            if (!char.IsLetter(chr))
            {
                continue;
            }

            foreach (var dir in directions)
            {
                if (!maze.IsInDomain(pos + dir) || !char.IsLetter(maze[pos + dir]))
                {
                    continue;
                }

                var portal = new PortalKey(maze[pos], maze[pos + dir]);
                var entranceType = !maze.IsInDomain(pos - dir) || !maze.IsInDomain(pos + dir + dir)
                    ? EntranceType.Outer
                    : EntranceType.Inner;
                var entrancePos = PositionValid(maze,pos - dir) 
                    ? pos - dir 
                    : pos + dir + dir;
                
                entrances.EnsureContainsKey(portal, new List<PortalEntrance>());
                entrances[portal].Add(new PortalEntrance(
                    Pos: entrancePos,
                    Type: entranceType));
                
                break;
            }
        }

        return new PortalMap(entrances);
    }
    
    private static bool PositionValid(Maze maze, Vector2D pos)
    {
        return maze.IsInDomain(pos) && maze[pos] == Traversable;
    }
    
    private Maze ParseMaze()
    {
        return Grid2D<char>.MapChars(
            strings: GetInputLines(),
            elementFunc: c => c);
    }
}