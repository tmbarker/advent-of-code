using System.Globalization;
using System.Net;

namespace Automation.AocClient;

public static class AocHttpClient
{
    public const string Domain = "https://adventofcode.com";
    
    private const string LastRequestEnvVar = "aoc_last_request";
    private const string UserSessionName = "session";
    private const string UserAgentName = "user-agent";
    private const string UserAgentValue = ".NET/6.0 (github.com/tmbarker/advent-of-code via AocHttpClient.cs)";

    public static async Task<HttpResponseMessage> SendRequest(string route, string userSession)
    {
        var lastRequest = GetLastRequestTime();
        var nextAllowed = lastRequest.Add(TimeSpan.FromMinutes(1));
        var now = DateTime.Now;

        if (now < nextAllowed)
        {
            return new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        }

        var baseUri = new Uri(Domain);
        var cookieContainer = new CookieContainer();

        SetLastRequestTime(now);
        
        //  We do not need to worry about session exhaustion for this low rate HttpClient usage
        //
        using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler) { BaseAddress = baseUri };

        cookieContainer.Add(baseUri, new Cookie(name: UserSessionName, value: userSession));
        client.DefaultRequestHeaders.Add(name: UserAgentName, value: UserAgentValue);
            
        return await client.GetAsync(route);
    }

    private static void SetLastRequestTime(DateTime time)
    {
        Environment.SetEnvironmentVariable(
            variable: LastRequestEnvVar, 
            value: time.ToString(CultureInfo.InvariantCulture),
            target: EnvironmentVariableTarget.User);
    }
    
    private static DateTime GetLastRequestTime()
    {
        var lastRequestString = Environment.GetEnvironmentVariable(
            variable: LastRequestEnvVar,
            target: EnvironmentVariableTarget.User);

        return !string.IsNullOrWhiteSpace(lastRequestString) && DateTime.TryParse(lastRequestString, out var time)
            ? time
            : DateTime.UnixEpoch;
    }
}