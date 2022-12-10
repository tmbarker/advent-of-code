using Problems.Common;

namespace Problems.Y2022.D10;

public class Cpu
{
    public event Action<State>? Ticked;
    
    private int Cycle { get; set; } = 1;
    private int X { get; set; } = 1;

    public void Run(IEnumerable<(Opcode opcode, int arg)> instructions)
    {
        foreach (var instruction in instructions)
        {
            switch (instruction.opcode)
            {
                case Opcode.Noop:
                    Tick();
                    break;
                case Opcode.Addx:
                    Tick();
                    Tick();
                    X += instruction.arg;
                    break;
                default:
                    throw new NoSolutionException();
            }
        }
    }

    private void Tick()
    {
        Ticked?.Invoke(GetState());
        Cycle++;
    }
    
    private State GetState()
    {
        return new State { Cycle = Cycle, X = X };
    }
}