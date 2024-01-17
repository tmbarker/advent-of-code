using Utilities.Geometry.Euclidean;

namespace Solutions.Y2018.D15;

using Field = Grid2D<char>;

public static class Pathfinding
{
    public static bool FindNearestReachable(Field field, Vec2D start, HashSet<Vec2D> targetPositions, out Vec2D nearest)
    {
        var queue = new Queue<Vec2D>(collection: [start]);
        var visited = new HashSet<Vec2D>(collection: [start]);
        var candidates = new HashSet<Vec2D>();
        
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
                    .Where(p => field.Contains(p) && !visited.Contains(p));

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

        nearest = Vec2D.Zero;
        return false;
    }
    
    public static Vec2D GetStepPos(Field field, Vec2D start, Vec2D goal)
    {
        var queue = new Queue<Vec2D>(collection: [goal]);
        var visited = new HashSet<Vec2D>(collection: [goal]);
        var candidates = new HashSet<Vec2D>();
        
        while (queue.Any())
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var pos = queue.Dequeue();
                if (Vec2D.IsAdjacent(a: pos, b: start, Metric.Taxicab))
                {
                    candidates.Add(pos);
                    continue;
                }

                var freeAdjacent = pos
                    .GetAdjacentSet(Metric.Taxicab)
                    .Where(p => field.Contains(p) && field[p] == GameData.Empty)
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