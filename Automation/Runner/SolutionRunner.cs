using System.Diagnostics;
using System.Text;
using Automation.Input;
using Solutions.Attributes;
using Solutions.Common;

namespace Automation.Runner;

/// <summary>
///     A reflective utility class for running puzzle solutions
/// </summary>
public static class SolutionRunner
{
    private const string QualifiedSolutionTypeNameFormat = "{0}.Y{1}.D{2}.{3}";
    private const string SolutionTypeName = "Solution";
    private const string UserSessionEnvVar = "aoc_user_session";

    /// <summary>
    ///     Instantiate the puzzle solution associated with the specified <paramref name="year"/> and
    ///     <paramref name="day"/>. If the input file path is not provided, try to get it from the cache, downloading
    ///     first if necessary. Next, run and log the puzzle solution.
    /// </summary>
    /// <param name="year">The year associated with the puzzle</param>
    /// <param name="day">The day associated with the puzzle</param>
    /// <param name="inputPath">
    ///     Used to manually specify the input file path, if unset the <see cref="SolutionRunner"/>
    ///     will attempt to get the input from the cache
    /// </param>
    /// <param name="showLogs">
    ///     Some solutions emit logs as they run, when set they will be printed to the console
    /// </param>
    public static async Task Run(int year, int day, string inputPath = "", bool showLogs = false)
    {
        if (!TryCreateSolutionInstance(year, day, out var solution))
        {
            Log(log: SolutionBase.ProblemNotSolvedString, ConsoleColor.Red);
            return;
        }
        
        if (!string.IsNullOrWhiteSpace(inputPath))
        {
            if (!File.Exists(inputPath))
            {
                Log(log: $"No input file exists at the specified path [{inputPath}]", ConsoleColor.Red);
                return;
            }
            
            RunInternal(solution!, year, day, inputPath, showLogs);
            return;
        }
        
        inputPath = InputProvider.FormCachedInputFilePath(year, day);
        if (InputProvider.CheckCacheForInput(year, day))
        {
            Log(log: $"Input found in cache [{inputPath}]", ConsoleColor.Gray);
            RunInternal(solution!, year, day, inputPath, showLogs);
            return;
        }
        
        var userSession = GetUserSession();
        if (string.IsNullOrWhiteSpace(userSession))
        {
            Log(log: "Cannot download input file, user session not set", ConsoleColor.Red);
            return;
        }
        
        var downloadSuccess = await InputProvider.TryDownloadInputToCache(year, day, userSession);
        if (downloadSuccess)
        {
            RunInternal(solution!, year, day, inputPath, showLogs);
        }
    }

    private static void RunInternal(SolutionBase solution, int year, int day, string inputPath, bool showLogs)
    {
        solution.InputPath = inputPath;
        solution.LogsEnabled = showLogs;

        if (CheckSolutionInputSpecific(solution, out var message))
        {
            Log(year, day, log: message, ConsoleColor.DarkYellow);
        }
        
        for (var i = 0; i < solution.Parts; i++)
        {
            RunPartInternal(solution, year, day, part: i + 1);
        }
    }
    
    private static void RunPartInternal(SolutionBase solutionInstance, int year, int day, int part)
    {
        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            var result = solutionInstance.Run(part);
            var elapsed = FormElapsedString(stopwatch.Elapsed);
            Log(year, day, log: $"[Elapsed: {elapsed}] Part {part} solution => {result}", color: ConsoleColor.Green);
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

    public static void SetUserSession(string userSession)
    {
        if (string.IsNullOrWhiteSpace(userSession))
        {
            Log("Invalid session cookie [NULL]", ConsoleColor.Red);
            return;
        }

        Environment.SetEnvironmentVariable(
            variable: UserSessionEnvVar,
            value: userSession,
            target: EnvironmentVariableTarget.User);
        Log($"User session set [{userSession}]", ConsoleColor.Green);
    }
    
    private static bool CheckSolutionInputSpecific(SolutionBase instance, out string message)
    {
        message = string.Empty;
        var attr = Attribute.GetCustomAttribute(
            element: instance.GetType(),
            attributeType: typeof(InputSpecificSolutionAttribute));

        if (attr != null)
        {
            message = $"[Warning] {((InputSpecificSolutionAttribute)attr).Message}";
        }

        return attr != null;
    }
    
    private static bool TryCreateSolutionInstance(int year, int day, out SolutionBase? instance)
    {
        try
        {
            var assembly = typeof(SolutionBase).Assembly;
            var type = assembly.GetType(GetQualifiedSolutionTypeName(year, day))!;

            instance = (SolutionBase)Activator.CreateInstance(type)!;
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

    private static string GetUserSession()
    {
        var userSession = Environment.GetEnvironmentVariable(
            variable: UserSessionEnvVar,
            EnvironmentVariableTarget.User);

        if (string.IsNullOrWhiteSpace(userSession))
        {
            throw new Exception(message: "User session cookie missing/not set");
        }

        return userSession;
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
        Log($"[Year: {year}, Day: {day}] {log}", color);
    }
    
    private static void Log(string log, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(log);
        Console.ResetColor();
    } 
}