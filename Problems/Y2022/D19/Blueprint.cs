namespace Problems.Y2022.D19;

public struct Blueprint
{
    private Dictionary<Materials, int> _highestCostsCache;

    public int Id { get; init; }
    public Dictionary<Materials, Dictionary<Materials, int>> RobotCosts { get; init; }
    public Dictionary<Materials, int> HighestCosts => GetHighestCosts();

    private Dictionary<Materials, int> GetHighestCosts()
    {
        _highestCostsCache ??= FormHighestCosts();
        return _highestCostsCache;
    }
    
    private Dictionary<Materials, int> FormHighestCosts()
    {
        var map = RobotCosts.ToDictionary(kvp => kvp.Key, _ => 0);
        foreach (var robotCosts in RobotCosts.Values)
        {
            foreach (var (material, cost) in robotCosts)
            {
                map[material] = Math.Max(map[material], cost);
            }
        }
        return map;
    }
}