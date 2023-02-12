using Problems.Y2019.Common;
using Utilities.Cartesian;
using Utilities.Extensions;
using Utilities.Graph;

namespace Problems.Y2019.D20;

using Maze = Grid2D<char>;

/// <summary>
/// Donut Maze: https://adventofcode.com/2019/day/20
/// </summary>
public class Solution : SolutionBase2019
{ 
    private const char Traversable = '.';

    private static readonly Portal Entrance = new('A', 'A');
    private static readonly Portal Exit = new('Z', 'Z');

    public override int Day => 20;
    
    public override object Run(int part)
    {
        var maze = ParseMaze();
        return part switch
        {
            0 => Traverse(maze, Entrance, Exit),
            _ => ProblemNotSolvedString
        };
    }

    private static int Traverse(Maze maze, Portal entrance, Portal exit)
    {
        var allAdjacencies = new Dictionary<Vector2D, HashSet<Vector2D>>();
        var portalAdjacencies = GetPortalAdjacencies(maze);

        var start = portalAdjacencies[entrance].Single();
        var end = portalAdjacencies[exit].Single();

        foreach (var (pos, chr) in maze)
        {
            if (chr != Traversable)
            {
                continue;
            }

            var adj = pos.GetAdjacentSet(DistanceMetric.Taxicab)
                .Where(p => PositionValid(maze, p))
                .ToHashSet();
            allAdjacencies.Add(pos, adj);
        }

        foreach (var pair in portalAdjacencies.Values.Where(c => c.Count == 2))
        {
            allAdjacencies[pair.First()].Add(pair.Last());
            allAdjacencies[pair.Last()].Add(pair.First());
        }

        return GraphHelper.DijkstraUnweighted(start, end, allAdjacencies);
    }

    private static Dictionary<Portal, IList<Vector2D>> GetPortalAdjacencies(Maze maze)
    {
        var lookup = new Dictionary<Portal, IList<Vector2D>>();
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

                var portal = new Portal(maze[pos], maze[pos + dir]);
                var adj = PositionValid(maze,pos - dir) 
                    ? pos - dir 
                    : pos + dir + dir;
                
                lookup.EnsureContainsKey(portal, new List<Vector2D>());
                lookup[portal].Add(adj);
                break;
            }
        }

        return lookup;
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