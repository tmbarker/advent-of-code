using Utilities.DataStructures.Cartesian;

namespace Problems.Y2021.D17;

public readonly struct Target
{
    public Target(int xMin, int xMax, int yMin, int yMax)
    {
        XMin = xMin;
        XMax = xMax;
        YMin = yMin;
        YMax = yMax;
    }
    
    // ReSharper disable MemberCanBePrivate.Global
    public int XMin { get; }
    public int XMax { get; }
    public int YMin { get; }
    public int YMax { get; }
    
    public bool ContainsPos(Vector2D pos)
    {
        return
            pos.X >= XMin && pos.X <= XMax &&
            pos.Y >= YMin && pos.Y <= YMax;
    }
}