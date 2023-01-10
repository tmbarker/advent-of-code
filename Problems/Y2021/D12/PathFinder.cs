namespace Problems.Y2021.D12;

public class PathFinder
{
    private const string StartId = "start";
    private const string EndId = "end";

    private readonly Dictionary<string, HashSet<string>> _adjacencyMap;
    private readonly bool _bonusSmallCaveVisit;

    public event Action? PathFound;
    
    public PathFinder(Dictionary<string, HashSet<string>> adjacencyMap, bool bonusSmallCaveVisit)
    {
        _adjacencyMap = adjacencyMap;
        _bonusSmallCaveVisit = bonusSmallCaveVisit;
    }

    public void Run()
    {
        var visitedCounts = _adjacencyMap.Keys.ToDictionary(k => k, _ => 0);
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
        foreach (var neighbor in _adjacencyMap[current])
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

    private bool CanVisit(string target, IReadOnlyDictionary<string, int> visitedCounts)
    {
        if (target == EndId || target.All(char.IsUpper) || visitedCounts[target] == 0)
        {
            return true;
        }
        
        if (target == StartId || !_bonusSmallCaveVisit)
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