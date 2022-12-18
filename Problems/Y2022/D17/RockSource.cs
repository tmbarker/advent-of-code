namespace Problems.Y2022.D17;

public static class RockSource
{
    private static readonly List<Rock> List = new();

    public static int CycleLength => List.Count; 
    
    static RockSource()
    {
        List.Add(new HorizontalLine());
        List.Add(new Plus());
        List.Add(new L());
        List.Add(new VerticalLine());
        List.Add(new Square());
    }

    public static Rock Get(int index)
    {
        return List[index % CycleLength];
    }
}