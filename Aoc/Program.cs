using System.CommandLine;
using Automation.Input;
using Automation.Readme;
using Automation.SolutionRunner;

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
        var logsOption = new Option<bool>(
            aliases: new[] { "-l", "--logs" },
            description: "Some problems emit logs as they run, print any such logs to the console",
            getDefaultValue: () => false);
        
        solveCommand.AddArgument(yearArg);
        solveCommand.AddArgument(dayArg);
        solveCommand.AddOption(logsOption);
        solveCommand.SetHandler(
            handle: async (year, day, showLogs) => await SolutionRunner.Run(year, day, showLogs),
            symbol1: yearArg,
            symbol2: dayArg,
            symbol3: logsOption);
        
        var updateReadmeCommand = new Command(
            name: "update-readme", 
            description: "Generate the README.md favourite problem tables");
        
        updateReadmeCommand.SetHandler(ReadmeUtils.UpdateFavouritePuzzles);

        var setUserSessionCommand = new Command(
            name: "set-session",
            description: "Set the user session cookie, needed to fetch inputs");
        var sessionArg = new Argument<string>(
            name: "user-session-cookie", 
            description: "The user session cookie string");
        
        setUserSessionCommand.AddArgument(sessionArg);
        setUserSessionCommand.SetHandler(
            handle: InputProvider.SetUserSession,
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

        var rootCommand = new RootCommand(description: "CLI entry point for running AoC problem solutions");
        rootCommand.AddCommand(solveCommand);
        rootCommand.AddCommand(updateReadmeCommand);
        rootCommand.AddCommand(setUserSessionCommand);
        rootCommand.AddCommand(setInputCacheCommand);
        
        return await rootCommand.InvokeAsync(args);
    }
}