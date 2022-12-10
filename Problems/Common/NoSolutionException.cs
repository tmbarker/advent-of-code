namespace Problems.Common;

public class NoSolutionException : Exception
{
    private const string NoSolutionErrorText = "No solution exists";

    public NoSolutionException() : base(NoSolutionErrorText)
    {
    }

    public NoSolutionException(Exception innerException) : base(NoSolutionErrorText, innerException)
    {
    }
}