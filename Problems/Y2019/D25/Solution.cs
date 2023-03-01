using System.Text;
using Problems.Common;
using Problems.Y2019.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D25;

/// <summary>
/// Cryostasis: https://adventofcode.com/2019/day/25
/// </summary>
public class Solution : SolutionBase2019
{ 
    public override int Day => 25;
    public override int Parts => 1;
    
    public override object Run(int part)
    {
        return part == 0 
            ? PlayGame() 
            : throw new NoSolutionException();
    }

    private string PlayGame()
    {
        var program = LoadIntCodeProgram();
        var game = IntCodeVm.Create(program);

        game.Run();
        PrintGameOutput(game);

        while (Cheats.AutomationCommands.Any())
        {
            EnterGameCommand(
                game: game,
                command: Cheats.AutomationCommands.Dequeue());
            PrintGameOutput(game);
        }

        var numInventoryItems = Cheats.InventoryItems.Count;
        var numCombinations = (uint)Math.Pow(2, numInventoryItems);
        
        for (var combinationMask = 0U; combinationMask < numCombinations; combinationMask++)
        {
            for (var itemIndex = 0; itemIndex < numInventoryItems; itemIndex++)
            {
                var combinationIncludesItem = (combinationMask & (1U << itemIndex)) > 0U;
                var itemName = Cheats.InventoryItems[itemIndex];
                var itemCommand = combinationIncludesItem 
                    ? $"take {itemName}" 
                    : $"drop {itemName}";
                
                EnterGameCommand(game, itemCommand);
                PrintGameOutput(game);
                EnterGameCommand(game, Cheats.TestInventoryCommand);
                
                var output = ReadAsciiOutput(game);
                var match = Cheats.PasscodeRegex.Match(output);
                Console.WriteLine(output);

                if (match.Success)
                {
                    return match.Groups[0].Value;
                }
            }
        }

        throw new NoSolutionException();
    }

    private static void PrintGameOutput(IntCodeVm game)
    {
        if (game.OutputBuffer.Any())
        {
            Console.WriteLine(ReadAsciiOutput(game));
        }
    }

    private static void EnterGameCommand(IntCodeVm game, string command)
    {
        foreach (var c in Compile(command))
        {
            game.InputBuffer.Enqueue(c);
        }

        game.Run();
    }
    
    private static string ReadAsciiOutput(IntCodeVm game)
    {
        var sb = new StringBuilder();
        while (game.OutputBuffer.Any())
        {
            sb.Append((char)game.OutputBuffer.Dequeue());
        }

        return sb.ToString();
    }
    
    private static IEnumerable<long> Compile(string command)
    {
        foreach (var c in command)
        {
            yield return c;
        }

        yield return '\n';
    }
}