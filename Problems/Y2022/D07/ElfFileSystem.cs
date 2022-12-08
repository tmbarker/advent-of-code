namespace Problems.Y2022.D07;

public class ElfFileSystem
{
    private const char ConsoleDelimiter = ' ';
    private const string UserInputPrefix = "$";
    private const string RootDirectoryCmd = "/";
    private const string ParentCmd = "..";
    private const string ListCmd = "ls";

    private readonly ElfDirectory _root = ElfDirectory.Root();
    
    private ElfDirectory CurrentDirectory { get; set; }

    private ElfFileSystem(IEnumerable<string> consoleOutput)
    {
        CurrentDirectory = _root;
        
        foreach (var line in consoleOutput)
        {
            if (TryParseCommand(line, out var cmd, out var arg))
            {
                if (cmd == ElfCommand.Cd)
                {
                    HandleCdCommand(arg!);
                }
                continue;
            }
            
            HandleLsItem(line);
        }
    }
    
    public static ElfFileSystem Construct(IEnumerable<string> consoleOutput)
    {
        return new ElfFileSystem(consoleOutput);
    }

    public ElfDirectory RootDirectory => _root;
    
    public Dictionary<string, int> BuildDirectorySizeIndex()
    {
        var sizeIndex = new Dictionary<string, int>();
        GetSize(_root, sizeIndex);

        return sizeIndex;
    }

    private static int GetSize(ElfDirectory directory, IDictionary<string, int> sizeIndex)
    {
        var indexKey = directory.GetPath();
        if (sizeIndex.ContainsKey(indexKey))
        {
            return sizeIndex[indexKey];
        }

        var naiveSize = directory.GetNaiveSize();
        if (!directory.HasSubdirectories)
        {
            sizeIndex.Add(indexKey, naiveSize);
            return sizeIndex[indexKey];
        }
        
        var size = naiveSize + directory.Subdirectories.Sum(s => GetSize(s, sizeIndex));
        sizeIndex.Add(indexKey, size);
        
        return size;
    }

    private void HandleCdCommand(string arg)
    {
        CurrentDirectory = arg switch
        {
            RootDirectoryCmd => _root,
            ParentCmd => CurrentDirectory.Parent!,
            _ => CurrentDirectory.GetSubdirectory(arg)
        };
    }

    private void HandleLsItem(string line)
    {
        var elements = ParseLine(line);
        if (int.TryParse(elements[0], out var size))
        {
            CurrentDirectory.RegisterFile(elements[1], size);
        }
        else
        {
            CurrentDirectory.RegisterSubdirectory(new ElfDirectory(elements[1], CurrentDirectory));
        }
    }
    
    private static bool TryParseCommand(string line, out ElfCommand? cmd, out string? arg)
    {
        cmd = null;
        arg = null;
        
        var elements = ParseLine(line);
        if (elements[0] != UserInputPrefix)
        {
            return false;
        }

        if (elements[1] == ListCmd)
        {
            cmd = ElfCommand.Ls;
            return true;
        }

        cmd = ElfCommand.Cd;
        arg = elements[2];
        
        return true;
    }

    private static string[] ParseLine(string line)
    {
        return line.Split(ConsoleDelimiter, StringSplitOptions.RemoveEmptyEntries);
    }
}