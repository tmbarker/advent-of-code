using Utilities.DataStructures.Cartesian;

namespace Problems.Y2022.D17;

public abstract class Rock
{
    public abstract Grid2D<bool> Shape { get; }
}

public sealed class HorizontalLine : Rock
{
    public HorizontalLine()
    {
        Shape = Grid2D<bool>.WithDimensions(1, 4);
        Shape[0, 0] = true;
        Shape[1, 0] = true;
        Shape[2, 0] = true;
        Shape[3, 0] = true;
    }
    
    public override Grid2D<bool> Shape { get; }
}

public sealed class Plus : Rock
{
    public Plus()
    {
        Shape = Grid2D<bool>.WithDimensions(3, 3);
        Shape[1, 0] = true;
        Shape[1, 1] = true;
        Shape[1, 2] = true;
        Shape[0, 1] = true;
        Shape[2, 1] = true;
    }
    
    public override Grid2D<bool> Shape { get; }
}

public sealed class L : Rock
{
    public L()
    {
        Shape = Grid2D<bool>.WithDimensions(3, 3);
        Shape[0, 0] = true;
        Shape[1, 0] = true;
        Shape[2, 0] = true;
        Shape[2, 1] = true;
        Shape[2, 2] = true;
    }
    
    public override Grid2D<bool> Shape { get; }
}

public sealed class VerticalLine : Rock
{
    public VerticalLine()
    {
        Shape = Grid2D<bool>.WithDimensions(4, 1);
        Shape[0, 0] = true;
        Shape[0, 1] = true;
        Shape[0, 2] = true;
        Shape[0, 3] = true;
    }
    
    public override Grid2D<bool> Shape { get; }
}

public sealed class Square : Rock
{
    public Square()
    {
        Shape = Grid2D<bool>.WithDimensions(2, 2);
        Shape[0, 0] = true;
        Shape[0, 1] = true;
        Shape[1, 0] = true;
        Shape[1, 1] = true;
    }
    
    public override Grid2D<bool> Shape { get; }
}