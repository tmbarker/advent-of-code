namespace Problems.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class)]
public class InputSpecificAttribute : Attribute
{
    private const string DefaultMessage =
        "This solution implementation is input specific, and may not work on all inputs";

    public string Message { get; }
    
    public InputSpecificAttribute(string message)
    {
        Message = message;
    }
    
    public InputSpecificAttribute() : this(DefaultMessage)
    {
    }
}