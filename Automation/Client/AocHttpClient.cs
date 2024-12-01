using System.Globalization;
using System.Net;

namespace Automation.Client;

/// <summary>
///     A utility class for sending HTTP requests to Advent of Code [<see cref="Domain" />]
/// </summary>
public static class AocHttpClient
{
    /// <summary>
    ///     The Advent of Code domain, all requests that this utility makes are to this domain.
    /// </summary>
    public const string Domain = "https://adventofcode.com";
    
    private const string LastRequestFileName = "last_http_request.txt";
    private const string UserSessionName = "session";
    private const string UserAgentName = "user-agent";
    private const string UserAgentValue = $".NET/9.0 (github.com/tmbarker/advent-of-code via {nameof(AocHttpClient)}.cs)";
    
    private static readonly Uri DomainUri = new (Domain);
    private static readonly TimeSpan RateLimit = TimeSpan.FromMinutes(1);

    /// <summary>
    ///     Send an HTTP request to Advent of Code [<see cref="Domain" />]
    /// </summary>
    /// <param name="route">The request route, relative to the Advent of Code <see cref="Domain" /></param>
    /// <param name="userSession">The user session cookie value</param>
    /// <returns>The request response</returns>
    public static async Task<HttpResponseMessage> SendRequest(string route, string userSession)
    {
        var lastRequest = GetLastRequestTime();
        var nextAllowed = lastRequest.Add(RateLimit);

        if (DateTime.Now < nextAllowed)
        {
            return new HttpResponseMessage(HttpStatusCode.TooManyRequests);
        }
        
        SetLastRequestTime(DateTime.Now);

        using var handler = new HttpClientHandler();
        handler.CookieContainer = new CookieContainer();
        handler.CookieContainer.Add(DomainUri, new Cookie(name: UserSessionName, value: userSession));
        
        using var client = new HttpClient(handler);
        client.BaseAddress = DomainUri;
        client.DefaultRequestHeaders.Add(name: UserAgentName, value: UserAgentValue);
            
        return await client.GetAsync(route);
    }

    private static void SetLastRequestTime(DateTime time)
    {
        File.WriteAllText(path: LastRequestFileName, contents: time.ToString(CultureInfo.InvariantCulture));
    }
    
    private static DateTime GetLastRequestTime()
    {
        if (!File.Exists(path: LastRequestFileName))
        {
            return DateTime.UnixEpoch;
        }
        
        var contents = File.ReadAllText(path: LastRequestFileName);
        if (string.IsNullOrWhiteSpace(contents) || !DateTime.TryParse(contents, out var lastRequestTime))
        {
            return DateTime.UnixEpoch;
        }

        return lastRequestTime;
    }
}