using System.Diagnostics;

namespace Problems.Common;

public abstract class SolutionBase
{
    private const string InputsDirectoryName = "Inputs";
    private const string DayStringFormat = "{0:D2}";
    private const string InputFilenameFormat = "{0}_{1}.txt";
    
    protected const string ProblemNotSolvedString = "Problem not solved!";

    private string FormattedDayString => string.Format(DayStringFormat, Day);
    private string InputFilename => string.Format(InputFilenameFormat, Year, FormattedDayString);

    protected abstract int Year { get; }
    protected abstract int Day { get; }

    public abstract string Run(int part = 0);
    
    private bool InputFileExists()
    {
        return File.Exists(GetInputFilePath());
    }
    
    protected void AssertInputExists()
    {
        Debug.Assert(InputFileExists(), $"Input file does not exist: {Year}-{Day}");
    }
    
    protected string GetInputFilePath()
    {
        return Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            InputsDirectoryName, 
            InputFilename);
    }
}