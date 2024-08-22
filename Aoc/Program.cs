using System.CommandLine;
using Automation.Input;
using Automation.Readme;
using Automation.Runner;
using Microsoft.Extensions.Configuration;

namespace Aoc;

internal static class Program
{
    public static async Task<int> Main(string[] args)
    {
        var configuration = new ConfigurationManager()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
        
        var inputProvider = new InputProvider(configuration);
        var solutionRunner = new SolutionRunner(configuration, inputProvider);
        
        var solveCommand = new Command(
            name: "solve", 
            description: "Run the specified puzzle solution, if it exists");
        var yearArg = new Argument<int>(
            name: "year", 
            description: "The year the puzzle belongs to");
        var dayArg = new Argument<int>(
            name: "day", 
            description: "The puzzle day");
        var inputPathOption = new Option<string>(
            aliases: ["--input", "--path"],
            description: "Manually specify the path to the input file",
            getDefaultValue: () => string.Empty);
        var logsOption = new Option<bool>(
            aliases: ["--logs"],
            description: "Some solutions emit logs as they run, print any such logs to the console",
            getDefaultValue: () => false);
        
        solveCommand.AddArgument(yearArg);
        solveCommand.AddArgument(dayArg);
        solveCommand.AddOption(logsOption);
        solveCommand.AddOption(inputPathOption);
        solveCommand.SetHandler(
            handle: async (year, day, inputPath, showLogs) => await solutionRunner.Run(year, day, inputPath, showLogs),
            symbol1: yearArg,
            symbol2: dayArg,
            symbol3: inputPathOption,
            symbol4: logsOption);

        var updateReadmeCommand = new Command(
            name: "update-readme",
            description: "Update the generated README.md content");
        
        updateReadmeCommand.SetHandler(ReadmeUtils.UpdateReadme);

        var scratchCommand = new Command(
            name: "scratch",
            description: $"Execute the {nameof(ScratchPad)}");

        scratchCommand.SetHandler(handle: ScratchPad.Execute);
        
        var rootCommand = new RootCommand(description: "CLI entry point for running AoC puzzle solutions");
        rootCommand.AddCommand(solveCommand);
        rootCommand.AddCommand(updateReadmeCommand);
        rootCommand.AddCommand(scratchCommand);
        
        return await rootCommand.InvokeAsync(args);
    }
}