namespace Problems.Y2022.D07;

public static class FileSystemParser
{
    private const char ConsoleDelimiter = ' ';
    private const char PathDelimiter = '/';
    
    private const string CmdPrefix = "$";
    private const string RootCmd = "/";
    private const string ParentCmd = "..";
    private const string ListCmd = "ls";

    public const string RootDirectoryPath = RootCmd;
    
    private static string? CurrentDirectory { get; set; }
    private static Dictionary<string, int>? DirectorySizeIndex { get; set; }

    public static Dictionary<string, int> ConstructDirectorySizeIndex(IEnumerable<string> consoleOutput)
    {
        CurrentDirectory = null;
        DirectorySizeIndex = new Dictionary<string, int>();
        
        foreach (var line in consoleOutput)
        {
            if (TryParseCommand(line, out var cmd, out var arg))
            {
                if (cmd == Command.Cd)
                {
                    HandleCdCommand(arg!);
                }
                continue;
            }
            
            HandleLsItem(line);
        }

        return DirectorySizeIndex;
    }

    private static void HandleCdCommand(string arg)
    {
        CurrentDirectory = arg switch
        {
            RootCmd => RootDirectoryPath,
            // NOTE: The below null suppression warning is incorrect, this bang is needed to compile
            // ReSharper disable once RedundantSuppressNullableWarningExpression
            ParentCmd => CurrentDirectory![..CurrentDirectory!.LastIndexOf(PathDelimiter)],
            // Only remaining uncovered case is navigating to a subdirectory
            _ => string.Join(PathDelimiter, CurrentDirectory, arg)
        };
    }

    private static void HandleLsItem(string line)
    {
        var elements = ParseLine(line);
        if (int.TryParse(elements[0], out var filesize))
        {
            IncrementDirectorySizeRecursive(CurrentDirectory!, filesize);
        }
    }

    private static void IncrementDirectorySizeRecursive(string directoryPath, int increment)
    {
        if (!DirectorySizeIndex!.ContainsKey(directoryPath))
        {
            DirectorySizeIndex.Add(directoryPath, 0);
        }

        DirectorySizeIndex[directoryPath] += increment;

        var lastPathDelimiter = directoryPath.LastIndexOf(PathDelimiter);
        if (lastPathDelimiter != 0)
        {
            // ReSharper disable once TailRecursiveCall
            IncrementDirectorySizeRecursive(directoryPath[..lastPathDelimiter], increment);
        }
    }
    
    private static bool TryParseCommand(string line, out Command? cmd, out string? arg)
    {
        cmd = null;
        arg = null;
        
        var elements = ParseLine(line);
        if (elements[0] != CmdPrefix)
        {
            return false;
        }

        if (elements[1] == ListCmd)
        {
            cmd = Command.Ls;
            return true;
        }

        cmd = Command.Cd;
        arg = elements[2];
        
        return true;
    }

    private static string[] ParseLine(string line)
    {
        return line.Split(ConsoleDelimiter, StringSplitOptions.RemoveEmptyEntries);
    }
}