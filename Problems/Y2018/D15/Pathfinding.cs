using Problems.Common;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2018.D15;

using Field = Grid2D<char>;

public static class Pathfinding
{
    public static bool FindNearestReachable(Field field, Vector2D start, HashSet<Vector2D> targetPositions, out Vector2D nearest)
    {
        var queue = new Queue<Vector2D>(new[] { start });
        var visited = new HashSet<Vector2D>(new[] { start });
        var candidates = new HashSet<Vector2D>();
        
        while (queue.Any())
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                if (field[pos] != GameData.Empty && pos != start)
                {
                    continue;
                }

                if (targetPositions.Contains(pos))
                {
                    candidates.Add(pos);
                }

                var adjacencies = pos
                    .GetAdjacentSet(Metric.Taxicab)
                    .Where(p => field.IsInDomain(p) && !visited.Contains(p));

                foreach (var adj in adjacencies)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            if (candidates.Any())
            {
                nearest = candidates.Min(GameData.SquareComparer);
                return true;
            }
        }

        nearest = Vector2D.Zero;
        return false;
    }
    
    public static Vector2D GetStepPos(Field field, Vector2D start, Vector2D goal)
    {
        var queue = new Queue<Vector2D>(new[] { goal });
        var visited = new HashSet<Vector2D>(new[] { goal });
        var candidates = new HashSet<Vector2D>();
        
        while (queue.Any())
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                if (Vector2D.IsAdjacent(a: pos, b: start, Metric.Taxicab))
                {
                    candidates.Add(pos);
                    continue;
                }

                var freeAdjacent = pos
                    .GetAdjacentSet(Metric.Taxicab)
                    .Where(p => field.IsInDomain(p) && field[p] == GameData.Empty)
                    .Where(p => !visited.Contains(p));
                    
                foreach (var adj in freeAdjacent)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            if (candidates.Any())
            {
                return candidates.Min(GameData.SquareComparer);
            }
        }

        throw new NoSolutionException();
    }
}