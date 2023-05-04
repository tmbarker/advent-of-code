using System.Text.RegularExpressions;

namespace Problems.Y2019.D25;

public static class Cheats
{
    public const string TestInventoryCommand = "north";
    public static readonly Regex PasscodeRegex = new(@"(\d+)");
    public static readonly Queue<string> AutomationCommands = new(new[]
    { 
        "west",
        "take semiconductor",
        "west",
        "take planetoid",
        "west",
        "take food ration",
        "west",
        "take fixed point",
        "west",
        "take klein bottle",
        "east",
        "south",
        "west",
        "take weather machine",
        "east",
        "north",
        "east",
        "east",
        "south",
        "south",
        "south",
        "take pointer",
        "north",
        "north",
        "east",
        "take coin",
        "east",
        "north",
        "east",
        "inv"
    });
    public static readonly IReadOnlyList<string> InventoryItems = new List<string>
    {
        "semiconductor",
        "planetoid",
        "food ration",
        "fixed point",
        "klein bottle",
        "weather machine",
        "coin",
        "pointer"
    };
}