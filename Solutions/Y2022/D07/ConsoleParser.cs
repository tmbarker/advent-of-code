using Utilities.Collections;

namespace Solutions.Y2022.D07;

public class ConsoleParser
{
    private const string CmdPrefix = "$";
    private const string RootCmd = "/";
    private const string ParentCmd = "..";
    private const string ListCmd = "ls";

    public const string RootDirectoryPath = RootCmd;
    
    private Stack<string> CurrentDirectory { get; } = new ();
    private DefaultDict<string, int> DirectorySizeIndex { get; } = new(defaultValue: 0);

    public IDictionary<string, int> BuildDirectoryMap(IEnumerable<string> consoleOutput)
    {
        CurrentDirectory.Clear();
        DirectorySizeIndex.Clear();
        
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

    private void HandleCdCommand(string arg)
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

    private void HandleLsItem(string line)
    {
        var elements = ParseLine(line);
        if (int.TryParse(elements[0], out var filesize))
        {
            IncrementContainingDirectories(filesize);
        }
    }

    private void IncrementContainingDirectories(int increment)
    {
        var history = new Stack<string>(collection: CurrentDirectory.Reverse());
        while (history.Count > 0)
        {
            DirectorySizeIndex[FormDirectoryPath(history)] += increment;
            history.Pop();
        }
    }

    private static string FormDirectoryPath(IEnumerable<string> directories)
    {
        return string.Join(separator: '/', directories);
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
        return line.Split(separator: ' ', StringSplitOptions.RemoveEmptyEntries);
    }
}