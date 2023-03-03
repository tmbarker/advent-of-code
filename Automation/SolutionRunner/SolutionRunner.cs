using System.Diagnostics;
using System.Text;
using Problems.Common;

namespace Automation.SolutionRunner;

public static class SolutionRunner
{
    private const string SolutionTypeName = "Solution";
    private const string QualifiedSolutionTypeNameFormat = "{0}.Y{1}.D{2}.{3}";

    public static void Run(int year, int day)
    {
        if (!TryCreateSolutionInstance(year, day, out var solution))
        {
            Log(year, day, SolutionBase.ProblemNotSolvedString);
            return;
        }

        for (var i = 0; i < solution!.Parts; i++)
        {
            TryRunSolutionPart(solution, part: i + 1);
        }
    }

    private static void TryRunSolutionPart(SolutionBase solutionInstance, int part)
    {
        var year = solutionInstance.Year;
        var day = solutionInstance.Day;
        var stopwatch = new Stopwatch();

        try
        {
            stopwatch.Start();
            var result = solutionInstance.Run(part);
            var elapsed = FormElapsedString(stopwatch.Elapsed);

            Log(
                year: year,
                day: day,
                log: $"[Elapsed: {elapsed}] Solution part {part} => {result}",
                color: ConsoleColor.Green);
        }
        catch (Exception e)
        {
            Log(
                year: year,
                day: day,
                log: $"Error running solution:\n{e}",
                color: ConsoleColor.Red);
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
            Log(
                year: year,
                day: day,
                log: $"Failed to create {SolutionTypeName} instance:\n{e}",
                color: ConsoleColor.Red);
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