using System.CommandLine;
using Automation.Input;
using Automation.Readme;
using Automation.Runner;

namespace Aoc;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var solveCommand = new Command(
            name: "solve", 
            description: "Run the specified problem solution, if it exists");
        var yearArg = new Argument<int>(
            name: "year", 
            description: "The year the problem belongs to");
        var dayArg = new Argument<int>(
            name: "day", 
            description: "The problem day");
        var inputPathOption = new Option<string>(
            aliases: new[] { "--input", "--path" },
            description: "Manually specify the path to the input file",
            getDefaultValue: () => string.Empty);
        var logsOption = new Option<bool>(
            aliases: new[] { "--logs" },
            description: "Some problems emit logs as they run, print any such logs to the console",
            getDefaultValue: () => false);
        
        solveCommand.AddArgument(yearArg);
        solveCommand.AddArgument(dayArg);
        solveCommand.AddOption(logsOption);
        solveCommand.AddOption(inputPathOption);
        solveCommand.SetHandler(
            handle: async (year, day, inputPath, showLogs) => await SolutionRunner.Run(year, day, inputPath, showLogs),
            symbol1: yearArg,
            symbol2: dayArg,
            symbol3: inputPathOption,
            symbol4: logsOption);

        var updateReadmeCommand = new Command(
            name: "update-readme",
            description: "Update the generated README.md content");
        
        updateReadmeCommand.SetHandler(ReadmeUtils.UpdateReadme);

        var setUserSessionCommand = new Command(
            name: "set-session",
            description: "Set the user session cookie, needed to fetch inputs");
        var sessionArg = new Argument<string>(
            name: "user-session-cookie", 
            description: "The user session cookie string");
        
        setUserSessionCommand.AddArgument(sessionArg);
        setUserSessionCommand.SetHandler(
            handle: SolutionRunner.SetUserSession,
            symbol: sessionArg);
        
        var setInputCacheCommand = new Command(
            name: "set-cache",
            description: "Set the input files cache directory, where downloaded inputs will be cached");
        var cacheArg = new Argument<string>(
            name: "inputs-cache-path",
            description: "The directory path to store input files");
        
        setInputCacheCommand.AddArgument(cacheArg);
        setInputCacheCommand.SetHandler(
            handle: InputProvider.SetCachePath,
            symbol: cacheArg);

        var scratchCommand = new Command(
            name: "scratch",
            description: $"Execute the {nameof(ScratchPad)}");

        scratchCommand.SetHandler(handle: ScratchPad.Execute);
        
        var rootCommand = new RootCommand(description: "CLI entry point for running AoC puzzle solutions");
        rootCommand.AddCommand(solveCommand);
        rootCommand.AddCommand(updateReadmeCommand);
        rootCommand.AddCommand(setUserSessionCommand);
        rootCommand.AddCommand(setInputCacheCommand);
        rootCommand.AddCommand(scratchCommand);
        
        return await rootCommand.InvokeAsync(args);
    }
}