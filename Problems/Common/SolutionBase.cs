using System.Diagnostics;

namespace Problems.Common;

public abstract class SolutionBase 
{
    private const string InputsDirectoryName = "Inputs";
    private const string InputFilenameFormat = "{0}_{1}.txt";
    
    public const string DayStringFormat = "{0:D2}";
    public const string ProblemNotSolvedString = "Problem not solved!";

    private string FormattedDayString => string.Format(DayStringFormat, Day);
    private string InputFilename => string.Format(InputFilenameFormat, Year, FormattedDayString);

    public abstract int Year { get; }
    public abstract int Day { get; }
    public virtual int Parts => 2;

    public abstract object Run(int part);

    protected void AssertInputExists()
    {
        Debug.Assert(InputFileExists(), $"Input file does not exist: {Year}-{Day}");
    }

    protected string[] GetInput()
    {
        AssertInputExists();
        return File.ReadAllLines(GetInputFilePath());
    }
    
    protected string GetInputFilePath()
    {
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            InputsDirectoryName, 
            InputFilename);
    }
    
    private bool InputFileExists()
    {
        return File.Exists(GetInputFilePath());
    }
}