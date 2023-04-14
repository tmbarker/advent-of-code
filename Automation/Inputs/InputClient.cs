using Automation.AocClient;

namespace Automation.Inputs;

public static class InputClient
{
    public static async Task<bool> TryDownloadInput(int year, int day, string filePath)
    {
        var requestRoute = FormBaseRelativeInputRequest(year, day);
        try
        {
            Log($"Requesting input [GET {AocHttpClient.Domain}{requestRoute}]", ConsoleColor.Gray);
            var responseMessage = await AocHttpClient.SendRequest(requestRoute);
            var responseContent = await responseMessage.Content.ReadAsStringAsync();

            if (responseMessage.IsSuccessStatusCode)
            {
                Log($"Response received [{responseMessage.StatusCode}]", ConsoleColor.Gray);
                await File.WriteAllTextAsync(filePath, responseContent);
                Log($"Input saved to file [{filePath}]", ConsoleColor.Gray);
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

    private static string FormBaseRelativeInputRequest(int year, int day)
    {
        return $"/{year}/day/{day}/input";
    }

    private static void Log(string log, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(log);
        Console.ResetColor();
    }
}