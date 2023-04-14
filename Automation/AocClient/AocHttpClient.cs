using System.Globalization;
using System.Net;

namespace Automation.AocClient;

public static class AocHttpClient
{
    public const string Domain = "https://adventofcode.com";
    
    private const string LastRequestEnvVar = "last-request";
    private const string UserSessionEnvVar = "user-session";
    private const string UserSessionName = "session";
    private const string UserAgentName = "user-agent";
    private const string UserAgentValue = ".NET/6.0 (github.com/tmbarker/advent-of-code via AocHttpClient.cs)";

    public static async Task<HttpResponseMessage> SendRequest(string route)
    {
        var lastRequest = GetLastRequestTime();
        var nextRequest = lastRequest.Add(TimeSpan.FromMinutes(1));
        var now = DateTime.Now;

        if (now < nextRequest)
        {
            return new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        }

        var baseUri = new Uri(Domain);
        var cookieContainer = new CookieContainer();
        var sessionCookie = GetSessionCookie();

        SetLastRequestTime(now);
        
        //  We do not need to worry about session exhaustion for this low rate HttpClient usage
        //
        using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
        using var client = new HttpClient(handler) { BaseAddress = baseUri };

        cookieContainer.Add(baseUri, sessionCookie);
        client.DefaultRequestHeaders.Add(name: UserAgentName, value: UserAgentValue);
            
        return await client.GetAsync(route);
    }

    public static void SetSessionCookie(string sessionCookie)
    {
        if (string.IsNullOrWhiteSpace(sessionCookie))
        {
            throw new ArgumentException("Invalid session cookie", nameof(sessionCookie));
        }

        Environment.SetEnvironmentVariable(
            variable: UserSessionEnvVar,
            value: sessionCookie,
            target: EnvironmentVariableTarget.User);
    }
    
    private static Cookie GetSessionCookie()
    {
        var userSessionCookieValue = Environment.GetEnvironmentVariable(
            variable: UserSessionEnvVar,
            EnvironmentVariableTarget.User);

        if (string.IsNullOrWhiteSpace(userSessionCookieValue))
        {
            throw new Exception(message: "User session cookie missing/not set");
        }
        
        return new Cookie(
            name: UserSessionName,
            value: userSessionCookieValue);
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