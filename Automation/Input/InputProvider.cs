using Automation.Client;

namespace Automation.Input;

/// <summary>
/// A utility which manages downloading, caching, and providing puzzle input files
/// </summary>
public static class InputProvider
{
    private const string InputsCacheEnvVar = "aoc_inputs_cache";
    private const string InputRequestRouteFormat = "{0}/day/{1}/input";
    private const string DefaultInputDirectoryName = "Inputs";
    private const string DefaultInputFileNameFormat = "{0:D2}.txt";
    
    public static bool CheckCacheForInput(int year, int day)
    {
        return File.Exists(FormCachedInputFilePath(year, day));
    }

    public static async Task<bool> TryDownloadInputToCache(int year, int day, string userSession)
    {
        var dirPath = FormInputDirectoryPath(year);
        var filePath = FormCachedInputFilePath(year, day);
        var requestRoute = FormDomainRelativeInputRequest(year, day);
        
        try
        {
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
                Log($"Creating cache directory [{dirPath}]", ConsoleColor.Gray);
            }
            
            Log($"Requesting input [GET {AocHttpClient.Domain}/{requestRoute}]", ConsoleColor.Gray);
            var responseMessage = await AocHttpClient.SendRequest(requestRoute, userSession);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                Log($"Response received [{responseMessage.StatusCode}]", ConsoleColor.Gray);
                await File.WriteAllTextAsync(filePath, responseContent);
                Log($"Input written to file [{filePath}]", ConsoleColor.Gray);
                return true;
            }

            Log($"Request error: {responseMessage.StatusCode}", ConsoleColor.Red);
            return false;
        }
        catch (Exception e)
        {
            Log($"Error downloading input [GET {AocHttpClient.Domain}/{requestRoute}]:\n{e}", ConsoleColor.Red);
            return false;
        }
    }
    
    public static string FormCachedInputFilePath(int year, int day)
    {
        var dirPath = FormInputDirectoryPath(year);
        var fileName = FormInputFileName(day);

        return Path.Combine(dirPath, fileName);
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

    private static string FormInputDirectoryPath(int year)
    {
        var cacheDirPath = GetCachePath();
        var yearDirName = year.ToString();

        //  Inputs are stored under the cache directory based on year: <cache directory>/<year>/<day>.txt
        //
        return Path.Combine(cacheDirPath, yearDirName);
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