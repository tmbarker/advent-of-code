using Automation.Client;
using Microsoft.Extensions.Configuration;

namespace Automation.Input;

/// <summary>
///     A utility which manages downloading, caching, and providing puzzle input files
/// </summary>
public class InputProvider(IConfiguration configuration)
{
    private const string InputCachePathKey = "InputCachePath";
    private const string InputRequestRouteFormat = "{0}/day/{1}/input";
    private const string DefaultInputDirectoryName = "Inputs";
    private const string DefaultInputFileNameFormat = "{0:D2}.txt";
    
    public bool CheckCacheForInput(int year, int day)
    {
        return File.Exists(FormCachedInputFilePath(year, day));
    }

    public async Task<bool> TryDownloadInputToCache(int year, int day, string userSession)
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

    public string FormCachedInputFilePath(int year, int day)
    {
        var dirPath = FormInputDirectoryPath(year);
        var fileName = FormInputFileName(day);

        return Path.Combine(dirPath, fileName);
    }

    private string GetCachePath()
    {
        var fullPath = configuration[InputCachePathKey];
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

    private string FormInputDirectoryPath(int year)
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