using Utilities.Cartesian;

namespace Problems.Y2018.D15;

using Field = Grid2D<char>;
using UnitMap = Dictionary<int, Unit>;
using CasualtyMap = Dictionary<char, int>;

public class GameState
{
    public int Tick { get; set; }
    public Field Field { get; }
    public UnitMap Units { get; }
    public CasualtyMap Casualties { get; }
    
    public void Print()
    {
        var tickSummary = Tick == 0
            ? "Initially:"
            : $"After {Tick} round{(Tick > 1 ? "s" : string.Empty)}";
        
        Console.WriteLine();
        Console.WriteLine(tickSummary);

        for (var y = 0; y < Field.Height; y++)
        {
            for (var x = 0; x < Field.Width; x++)
            {
                Console.Write(Field[x, y]);
            }
            
            Console.Write("\t");
            var yCache = y;
            var unitStrings = Units.Values
                .Where(unit => !unit.Dead && unit.Pos.Y == yCache)
                .OrderBy(unit => unit.Pos, GameData.SquareComparer)
                .Select(inRowUnit => $"{inRowUnit.Team}({inRowUnit.Hp})")
                .ToList();
            
            Console.Write(string.Join(",\t", unitStrings));
            Console.WriteLine();
        }
    }

    public static GameState Create(IList<string> input, IDictionary<char, int> teamDmgBuffs)
    {
        return new GameState(
            field: BuildField(input),
            units: BuildUnitMap(input, teamDmgBuffs));
    }
    
    private GameState(Field field, UnitMap units)
    {
        Field = field;
        Units = units;
        Casualties = new CasualtyMap
        {
            { GameData.Elf, 0 },
            { GameData.Goblin, 0 }
        };
    }
    
    private static Field BuildField(IList<string> input)
    {
        return Field.MapChars(
            strings: input,
            elementFunc: c => c,
            origin: Origin.Uv);
    }
    
    private static UnitMap BuildUnitMap(IList<string> input, IDictionary<char, int> teamDmgBuffs)
    {
        var rows = input.Count;
        var cols = input[0].Length;
        var unitMap = new UnitMap();

        for (var y = 0; y < rows; y++)
        for (var x = 0; x < cols; x++)
        {
            if (input[y][x] is not (GameData.Elf or GameData.Goblin))
            {
                continue;
            }
            
            var id = unitMap.Count;
            var team = input[y][x];
            var dmgBuff = teamDmgBuffs.TryGetValue(team, out var buff) ? buff : 0;
            var pos = new Vector2D(x, y);

            unitMap.Add(
                key: id,
                value: UnitFactory.Build(id, team, dmgBuff, pos));
        }

        return unitMap;
    }
}