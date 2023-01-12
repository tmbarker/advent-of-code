using Utilities.Extensions;

namespace Problems.Y2022.D07;

public static class ConsoleParser
{
    private const char ConsoleDelimiter = ' ';
    private const char PathDelimiter = '/';
    
    private const string CmdPrefix = "$";
    private const string RootCmd = "/";
    private const string ParentCmd = "..";
    private const string ListCmd = "ls";

    public const string RootDirectoryPath = RootCmd;
    
    private static Stack<string> CurrentDirectory { get; } = new ();
    private static Dictionary<string, int>? DirectorySizeIndex { get; set; }

    public static Dictionary<string, int> ConstructDirectorySizeIndex(IEnumerable<string> consoleOutput)
    {
        CurrentDirectory.Clear();
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
        switch (arg)
        {
            case RootCmd:
                CurrentDirectory.Clear();
                CurrentDirectory.Push(RootDirectoryPath);
                break;
            case ParentCmd:
                CurrentDirectory.Pop();
                break;
            default:
                CurrentDirectory.Push(arg);
                break;
        }
    }

    private static void HandleLsItem(string line)
    {
        var elements = ParseLine(line);
        if (int.TryParse(elements[0], out var filesize))
        {
            IncrementContainingDirectories(filesize);
        }
    }

    private static void IncrementContainingDirectories(int increment)
    {
        var history = new Stack<string>(CurrentDirectory.Reverse());
        while (history.Count > 0)
        {
            var directoryPath = FormDirectoryPath(history);
            
            DirectorySizeIndex!.EnsureContainsKey(directoryPath);
            DirectorySizeIndex![directoryPath] += increment;

            history.Pop();
        }
    }

    private static string FormDirectoryPath(IEnumerable<string> directories)
    {
        return string.Join(PathDelimiter, directories);
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