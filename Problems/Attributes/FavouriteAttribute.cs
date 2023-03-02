namespace Problems.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class)]
public class FavouriteAttribute : Attribute
{
    public FavouriteAttribute(string title, Topics topics, Difficulty difficulty)
    {
        Title = title;
        Topics = topics;
        Difficulty = difficulty;
    }
    
    public string Title { get; }
    public Topics Topics { get; }
    public Difficulty Difficulty { get; }
}