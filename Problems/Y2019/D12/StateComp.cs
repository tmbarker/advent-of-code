using Utilities.Cartesian;

namespace Problems.Y2019.D12;

public readonly struct StateComp
{
    public StateComp(Axis component, State state)
    {
        Pos = state.Pos.GetComponent(component);
        Vel = state.Vel.GetComponent(component);
    }

    // ReSharper disable UnusedAutoPropertyAccessor.Local
    private int Pos { get; }
    private int Vel { get; }
}