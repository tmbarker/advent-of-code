namespace Problems.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class)]
public sealed class PuzzleInfoAttribute(string title, Topics topics, Difficulty difficulty, bool favourite = false)
    : Attribute
{
    public string Title { get; } = title;
    public Topics Topics { get; } = topics;
    public Difficulty Difficulty { get; } = difficulty;
    public bool Favourite { get; } = favourite;
}