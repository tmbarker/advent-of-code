using System.Diagnostics;

namespace Problems.Common;

public abstract class SolutionBase 
{
    public const string DayStringFormat = "{0:D2}";
    public const string ProblemNotSolvedString = "Problem not solved!";
    
    public virtual int Parts => 2;
    
    public bool LogsEnabled { get; set; }
    public string InputFilePath { get; set; } = string.Empty;

    public abstract object Run(int part);

    protected string[] GetInputLines()
    {
        AssertInputExists();
        return File.ReadAllLines(InputFilePath);
    }

    protected string GetInputText()
    {
        AssertInputExists();
        return File.ReadAllText(InputFilePath).TrimEnd();
    }

    protected IEnumerable<T> ParseInputLines<T>(Func<string, T> parseFunc)
    {
        return GetInputLines().Select(parseFunc);
    }

    private void AssertInputExists()
    {
        Debug.Assert(InputFileExists(), $"Input file does not exist [{InputFilePath}]");
    }
    
    private bool InputFileExists()
    {
        return File.Exists(InputFilePath);
    }
}