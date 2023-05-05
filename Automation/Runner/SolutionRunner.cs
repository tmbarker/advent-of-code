using System.Diagnostics;
using System.Text;
using Automation.Input;
using Problems.Attributes;
using Problems.Common;

namespace Automation.Runner;

public static class SolutionRunner
{
    private const string QualifiedSolutionTypeNameFormat = "{0}.Y{1}.D{2}.{3}";
    private const string SolutionTypeName = "Solution";

    public static async Task Run(int year, int day, bool showLogs = false)
    {
        if (!TryCreateSolutionInstance(year, day, out var solution))
        {
            Log(year, day, log: SolutionBase.ProblemNotSolvedString, ConsoleColor.Red);
            return;
        }

        var inputPath = await InputProvider.GetInputFilePath(year, day);
        if (!File.Exists(inputPath))
        {
            Log(year, day, log: "Unable to load or fetch input", ConsoleColor.Red);
            return;
        }

        solution!.InputFilePath = inputPath;
        solution.LogsEnabled = showLogs;

        if (CheckSolutionInputSpecific(solution, out var message))
        {
            Log(year, day, log: message, ConsoleColor.DarkYellow);
        }
        
        for (var i = 0; i < solution.Parts; i++)
        {
            TryRunSolutionPart(solution, year, day, part: i + 1);
        }
    }

    private static bool CheckSolutionInputSpecific(SolutionBase instance, out string message)
    {
        message = string.Empty;
        var attr = Attribute.GetCustomAttribute(
            element: instance.GetType(),
            attributeType: typeof(InputSpecificAttribute));

        if (attr != null)
        {
            message = $"[Warning] {((InputSpecificAttribute)attr).Message}";
        }

        return attr != null;
    }

    private static void TryRunSolutionPart(SolutionBase solutionInstance, int year, int day, int part)
    {
        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            var result = solutionInstance.Run(part);
            var elapsed = FormElapsedString(stopwatch.Elapsed);
            Log(year, day, log: $"[Elapsed: {elapsed}] Solution part {part} => {result}", color: ConsoleColor.Green);
        }
        catch (Exception e)
        {
            Log(year, day, log: $"Error running solution:\n{e}", color: ConsoleColor.Red);
        }
        finally
        {
            stopwatch.Stop();
        }
    }
    
    private static bool TryCreateSolutionInstance(int year, int day, out SolutionBase? instance)
    {
        try
        {
            var type = typeof(SolutionBase)
                .Assembly
                .GetType(GetQualifiedSolutionTypeName(year, day));

            instance = (SolutionBase)Activator.CreateInstance(type!)!;
            return true;
        }
        catch (Exception e)
        {
            Log(year, day, log: $"Failed to create {SolutionTypeName} instance:\n{e}", color: ConsoleColor.Red);
        }

        instance = null;
        return false;
    }

    private static string GetQualifiedSolutionTypeName(int year, int day)
    {
        var owningAssemblyName = typeof(SolutionBase).Assembly.GetName().Name;
        var formattedDayString = string.Format(SolutionBase.DayStringFormat, day);

        return string.Format(QualifiedSolutionTypeNameFormat,
            owningAssemblyName,
            year,
            formattedDayString,
            SolutionTypeName);
    }

    private static string FormElapsedString(TimeSpan elapsed)
    {
        var sb = new StringBuilder();
        var overASecond = false;
        
        if (elapsed.TotalSeconds >= 1f)
        {
            sb.Append($"{(int)elapsed.TotalSeconds}.");
            overASecond = true;
        }

        sb.Append(overASecond ? $"{elapsed.Milliseconds:D3}" : $"{elapsed.Milliseconds}");
        sb.Append(overASecond ? "s" : "ms");
        
        return sb.ToString();
    }
    
    private static void Log(int year, int day, string log, ConsoleColor color = default)
    {
        Console.ForegroundColor = color;
        Console.WriteLine($"[Year: {year}, Day: {day}] {log}");
        Console.ResetColor();
    }
}