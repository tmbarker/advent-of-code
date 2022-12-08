namespace Problems.Y2022.D07;

public class ElfDirectory
{
    private const char Delimiter = '/';
    private const char RootDirName = '~';

    private readonly Dictionary<string, int> _fileSizeMap = new();
    private readonly Dictionary<string, ElfDirectory> _subdirectoryMap = new();

    public ElfDirectory(string name, ElfDirectory? parent)
    {
        Name = name;
        Parent = parent;
    }

    public static ElfDirectory Root()
    {
        return new ElfDirectory(RootDirName.ToString(), null);
    }

    private string Name { get; }
    private bool IsRoot => Parent == null;
    
    public ElfDirectory? Parent { get; }
    public bool HasSubdirectories => _subdirectoryMap.Count > 0;
    public IEnumerable<ElfDirectory> Subdirectories => _subdirectoryMap.Values;

    public string GetPath()
    {
        return IsRoot ? Name : string.Join(Delimiter, Parent!.GetPath(), Name);
    }

    public int GetNaiveSize()
    {
        return _fileSizeMap.Values.Sum();
    }

    public ElfDirectory GetSubdirectory(string name)
    {
        return _subdirectoryMap[name];
    }
    
    public void RegisterFile(string filename, int size)
    {
        if (!_fileSizeMap.ContainsKey(filename))
        {
            _fileSizeMap.Add(filename, size);
        }
    }

    public void RegisterSubdirectory(ElfDirectory subdirectory)
    {
        if (!_subdirectoryMap.ContainsKey(subdirectory.Name))
        {
            _subdirectoryMap.Add(subdirectory.Name, subdirectory);
        }
    }

    public override string ToString()
    {
        return GetPath();
    }
}