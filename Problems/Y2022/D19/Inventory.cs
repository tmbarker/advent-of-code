namespace Problems.Y2022.D19;

public readonly struct Inventory
{
    public Dictionary<Materials, int> Robots { get; private init; }
    public Dictionary<Materials, int> Funds { get; private init; }

    public static Inventory GetInitial()
    {
        return new Inventory
        {
            Funds = new Dictionary<Materials, int>
            {
                { Materials.Ore, 0 },
                { Materials.Clay, 0 },
                { Materials.Obsidian, 0 },
                { Materials.Geode, 0 },
            },
            Robots = new Dictionary<Materials, int>
            {
                { Materials.Ore, 1 },
                { Materials.Clay, 0 },
                { Materials.Obsidian, 0 },
                { Materials.Geode, 0 },
            },
        };
    }
    
    public Inventory AfterWaiting()
    {
        return new Inventory
        {
            Funds = GetFundsAfterWaiting(),
            Robots = new Dictionary<Materials, int>(Robots),
        };
    }

    public Inventory AfterBuilding(Blueprint bp, Materials target)
    {
        return new Inventory
        {
            Funds = GetFundsAfterBuilding(bp, target),
            Robots = GetRobotsAfterBuilding(target),
        };
    }
    
    private Dictionary<Materials, int> GetFundsAfterWaiting()
    {
        var funds = new Dictionary<Materials, int>(Funds);
        foreach (var (robotType, numRobots) in Robots)
        {
            funds[robotType] += numRobots;
        }
        return funds;
    }
    
    private Dictionary<Materials, int> GetFundsAfterBuilding(Blueprint bp, Materials target)
    {
        var funds = GetFundsAfterWaiting();
        foreach (var (buildMaterial, cost) in bp.RobotCosts[target])
        {
            funds[buildMaterial] -= cost;
        }
        return funds;
    }

    private Dictionary<Materials, int> GetRobotsAfterBuilding(Materials target)
    {
        var robots = new Dictionary<Materials, int>(Robots);
        robots[target]++;
        return robots;
    }
}