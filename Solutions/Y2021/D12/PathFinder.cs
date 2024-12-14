namespace Solutions.Y2021.D12;

public sealed class PathFinder(IDictionary<string, HashSet<string>> adjacencyMap, bool bonusCave)
{
    private const string StartId = "start";
    private const string EndId = "end";

    public event Action? PathFound;

    public void Run()
    {
        var visitedCounts = adjacencyMap.Keys.ToDictionary(k => k, _ => 0);
        var pathStack = new Stack<string>();
        
        pathStack.Push(StartId);
        FindPaths(visitedCounts, pathStack);
    }
    
    private void FindPaths(Dictionary<string, int> visitedCounts, Stack<string> pathStack)
    {
        var current = pathStack.Peek();
        if (current == EndId)
        {
            RaisePathFound();
            return;
        }
        
        visitedCounts[current]++;
        foreach (var neighbor in adjacencyMap[current])
        {
            if (CanVisit(neighbor, visitedCounts))
            {
                pathStack.Push(neighbor);
                FindPaths(visitedCounts, pathStack);
                pathStack.Pop();
            }
        }

        visitedCounts[current]--;
    }

    private bool CanVisit(string target, Dictionary<string, int> visitedCounts)
    {
        if (target == EndId || target.All(char.IsUpper) || visitedCounts[target] == 0)
        {
            return true;
        }
        
        if (target == StartId || !bonusCave)
        {
            return false;
        }
        
        return !visitedCounts.Any(kvp => kvp.Key.All(char.IsLower) && kvp.Value > 1);
    }

    private void RaisePathFound()
    {
        PathFound?.Invoke();
    }
}