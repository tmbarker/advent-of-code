using System.Text;
using Problems.Common;
using Problems.Y2019.IntCode;

namespace Problems.Y2019.D25;

/// <summary>
/// Cryostasis: https://adventofcode.com/2019/day/25
/// </summary>
public class Solution : IntCodeSolution
{
    public override int Parts => 1;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => PlayGame(),
            _ => ProblemNotSolvedString
        };
    }

    private string PlayGame()
    {
        var program = LoadIntCodeProgram();
        var game = IntCodeVm.Create(program);

        game.Run();
        PrintOutput(game);

        while (Cheats.AutomationCommands.Any())
        {
            EnterGameCommand(
                game: game,
                command: Cheats.AutomationCommands.Dequeue());
            PrintOutput(game);
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
                PrintOutput(game);
                EnterGameCommand(game, Cheats.TestInventoryCommand);
                
                var output = ReadAsciiOutput(game);
                var match = Cheats.PasscodeRegex.Match(output);

                if (LogsEnabled)
                {
                    Console.WriteLine(output);   
                }

                if (match.Success)
                {
                    return match.Groups[0].Value;
                }
            }
        }

        throw new NoSolutionException();
    }

    private void PrintOutput(IntCodeVm game)
    {
        if (!game.OutputBuffer.Any())
        {
            return;
        }
        
        var output = ReadAsciiOutput(game);
        if (LogsEnabled)
        {
            Console.WriteLine(output);   
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