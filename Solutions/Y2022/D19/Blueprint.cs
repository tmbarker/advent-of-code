using Utilities.Extensions;

namespace Solutions.Y2022.D19;

public struct Blueprint
{
    private Dictionary<Materials, int> _highestCostsCache;

    public int Id { get; private init; }
    public Dictionary<Materials, Dictionary<Materials, int>> RobotCosts { get; private init; }
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
    
    public static Blueprint Parse(string line)
    {
        var numbers = line.ParseInts();
        return new Blueprint
        {
            Id = numbers[0],
            RobotCosts = new Dictionary<Materials, Dictionary<Materials, int>>
            {
                { Materials.Ore , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[1]}
                }},
                { Materials.Clay , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[2]}
                }},
                { Materials.Obsidian , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[3]},
                    { Materials.Clay, numbers[4]}
                }},
                { Materials.Geode , new Dictionary<Materials, int>
                {
                    { Materials.Ore, numbers[5]},
                    { Materials.Obsidian, numbers[6]}
                }}
            }
        };
    }
}