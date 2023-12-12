namespace Solutions.Y2022.D10;

public sealed class Cpu
{
    public readonly struct State
    {
        public int Cycle { get; init; }
        public int X { get; init; }
    }
    
    public enum Opcode
    {
        // ReSharper disable IdentifierTypo
        Addx = 0,
        Noop = 1
        // ReSharper restore IdentifierTypo
    }
    
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