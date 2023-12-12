namespace Solutions.Y2019.D17;

public static class RoutineBuilder
{
    private const int MinFuncLength = 2;
    private const int MaxFuncLength = 20;

    private const string VideoFeedOn = "y";
    private const string VideoFeedOff = "n";
    
    public static IEnumerable<string> Build(IEnumerable<string> commands, bool videoFeedOn)
    {
        var stream = string.Join(',', commands) + ',';
        var functions = ResolveFunctions(stream);

        var routine = stream;
        foreach (var (token, function) in functions)
        {
            routine = routine.Replace(function, token + ",");
        }

        var formatted = new List<string> { FormatLine(routine) };
        formatted.AddRange(functions.Values.Select(FormatLine));
        formatted.Add(FormatLine(videoFeedOn ? VideoFeedOn : VideoFeedOff));

        return formatted;
    }

    private static Dictionary<char, string> ResolveFunctions(string commandStream)
    {
        for (var a = MinFuncLength; a <= MaxFuncLength && a < commandStream.Length; a++)
        {
            var aFunc = commandStream[..a];
            var aRemaining = commandStream.Replace(aFunc, string.Empty);
            
            for (var b = MinFuncLength; b <= MaxFuncLength && b < aRemaining.Length; b++)
            {
                var bFunc = aRemaining[..b];
                var bRemaining = aRemaining.Replace(bFunc, string.Empty);

                for (var c = MinFuncLength; c <= MaxFuncLength && c < bRemaining.Length; c++)
                {
                    var cFunc = bRemaining[..c];
                    var cRemaining = bRemaining.Replace(cFunc, string.Empty);
                    
                    if (string.IsNullOrWhiteSpace(cRemaining))
                    {
                        return new Dictionary<char, string>
                        {
                            {'A', aFunc},
                            {'B', bFunc},
                            {'C', cFunc}
                        };
                    }
                }
            }
        }

        throw new NoSolutionException();
    }

    private static string FormatLine(string raw)
    {
        return raw.TrimEnd(',') + '\n';
    }
}