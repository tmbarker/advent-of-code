using Utilities.Cartesian;

namespace Problems.Y2019.D12;

public readonly struct State
{
    public State(Vector3D pos, Vector3D vel)
    {
        Pos = pos;
        Vel = vel;
    }
    
    public Vector3D Pos { get; }
    public Vector3D Vel { get; }
}