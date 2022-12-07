using Problems.Common;

namespace SolutionRunner;

public static class RunSolution
{
    private const string SolutionTypeName = "Solution";
    private const string QualifiedSolutionTypeNameFormat = "{0}.Y{1}.D{2}.{3}";

    public static void Do(int year, int day)
    {
        if (!TryCreateSolutionInstance(year, day, out var solution))
        {
            Log(year, day, SolutionBase.ProblemNotSolvedString);
            return;
        }

        for (var i = 0; i < solution!.Parts; i++)
        {
            TryRunSolutionPart(solution, i);
        }
    }

    private static void TryRunSolutionPart(SolutionBase solutionInstance, int part)
    {
        var year = solutionInstance.Year;
        var day = solutionInstance.Day;
        
        try
        {
            Log(year, day, $"Solution part {part + 1} => {solutionInstance.Run(part)}");
        }
        catch (Exception e)
        {
            Log(year, day, $"Error running solution:\n{e}");
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
            Log(year, day, $"Failed to create {SolutionTypeName} instance:\n{e}");
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

    private static void Log(int year, int day, string log)
    {
        Console.WriteLine($"[Year={year}, Day={day}] {log}");
    }
}