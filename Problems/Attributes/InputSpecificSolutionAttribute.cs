namespace Problems.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class)]
public sealed class InputSpecificSolutionAttribute(string message) : Attribute
{
    private const string DefaultMessage =
        "This solution implementation is input specific, and may not work on all inputs";

    public string Message { get; } = message;

    public InputSpecificSolutionAttribute() : this(DefaultMessage)
    {
    }
}