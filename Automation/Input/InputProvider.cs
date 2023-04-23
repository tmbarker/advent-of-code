using Automation.Client;

namespace Automation.Input;

public static class InputProvider
{
    private const string UserSessionEnvVar = "aoc_user_session";
    private const string InputsCacheEnvVar = "aoc_inputs_cache";
    private const string InputRequestRouteFormat = "/{0}/day/{1}/input";
    private const string DefaultInputDirectoryName = "Inputs";
    private const string DefaultInputFileNameFormat = "{0:D2}.txt";
    
    public static async Task<string> GetInputFilePath(int year, int day)
    {
        //  Inputs are stored under the cache directory based on year: <cache directory>/<year>/<day>.txt
        //
        var directory = GetCachePath();
        var fileName = FormInputFileName(day);
        var filePath = Path.Combine(
            directory,
            year.ToString(),
            fileName);
        
        if (File.Exists(filePath))
        {
            Log($"Input found in cache [{filePath}]", ConsoleColor.Gray);
            return filePath;
        }
        
        var success = await TryDownloadInput(year, day, filePath);
        return success ? filePath : string.Empty;
    }

    private static async Task<bool> TryDownloadInput(int year, int day, string filePath)
    {
        var requestRoute = FormDomainRelativeInputRequest(year, day);
        var userSession = GetUserSession();
        
        try
        {
            Log($"Requesting input [GET {AocHttpClient.Domain}{requestRoute}]", ConsoleColor.Gray);
            var responseMessage = await AocHttpClient.SendRequest(requestRoute, userSession);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                Log($"Response received [{responseMessage.StatusCode}]", ConsoleColor.Gray);
                await File.WriteAllTextAsync(filePath, responseContent);
                Log($"Input written to file [{filePath}]", ConsoleColor.Gray);
                return true;
            }

            Log($"Input request response error code: {responseMessage.StatusCode}", ConsoleColor.Red);
            return false;
        }
        catch (Exception e)
        {
            Log($"Error downloading input [GET {requestRoute}]:\n{e}", ConsoleColor.Red);
            return false;
        }
    }

    public static void SetUserSession(string sessionCookie)
    {
        if (string.IsNullOrWhiteSpace(sessionCookie))
        {
            Log($"Invalid session cookie [{sessionCookie}]", ConsoleColor.Red);
        }

        Environment.SetEnvironmentVariable(
            variable: UserSessionEnvVar,
            value: sessionCookie,
            target: EnvironmentVariableTarget.User);
        Log($"User session set [{sessionCookie}]", ConsoleColor.Green);
    }
    
    public static void SetCachePath(string path)
    {
        try
        {
            var fullPath = Path.GetFullPath(path);
            Environment.SetEnvironmentVariable(
                variable: InputsCacheEnvVar,
                value: fullPath,
                target: EnvironmentVariableTarget.User);
            Log($"Input cache set [{fullPath}]", ConsoleColor.Green);
        }
        catch (Exception e)
        {
            Log($"Error setting input cache, invalid path:\n{e}", ConsoleColor.Red);
        }
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

    private static string GetCachePath()
    {
        var fullPath = Environment.GetEnvironmentVariable(
            variable: InputsCacheEnvVar,
            EnvironmentVariableTarget.User);

        return string.IsNullOrWhiteSpace(fullPath)
            ? GetDefaultCachePath()
            : fullPath;
    }

    private static string GetDefaultCachePath()
    {
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            DefaultInputDirectoryName);
    }
    
    private static string FormInputFileName(int day)
    {
        return string.Format(DefaultInputFileNameFormat, day);
    }
    
    private static string FormDomainRelativeInputRequest(int year, int day)
    {
        return string.Format(InputRequestRouteFormat, year, day);
    }

    private static void Log(string log, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(log);
        Console.ResetColor();
    } 
}