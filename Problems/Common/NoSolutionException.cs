namespace Problems.Common;

public sealed class NoSolutionException : Exception
{
    private const string NoSolutionErrorText = "No solution exists";

    public NoSolutionException() : base(NoSolutionErrorText)
    {
    }

    public NoSolutionException(string message) : base(message)
    {
    }
    
    public NoSolutionException(Exception innerException) : base(NoSolutionErrorText, innerException)
    {
    }
}