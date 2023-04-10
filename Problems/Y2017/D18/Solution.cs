using Problems.Y2017.Common;

namespace Problems.Y2017.D18;

/// <summary>
/// Duet: https://adventofcode.com/2017/day/18
/// </summary>
public class Solution : SolutionBase2017
{
    public override int Day => 18;
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => RecoverFrequency(),
            2 => CountTransmissions(),
            _ => ProblemNotSolvedString
        };
    }

    private long RecoverFrequency()
    {
        var transmitted = 0L;
        var cts = new CancellationTokenSource();
        var buffer = new Queue<long>();
        var cpu = new Cpu(program: GetInputLines())
        {
            InputBuffer = buffer,
            OutputBuffer = buffer
        };

        void OnDataTransmitted(long data)
        {
            transmitted = data;
        }
        
        void OnDataReceived(long data)
        {
            cts.Cancel();
        }

        cpu.DataTransmitted += OnDataTransmitted;
        cpu.DataReceived += OnDataReceived;
        cpu.Run(cts.Token);

        return transmitted;
    }

    private long CountTransmissions()
    {
        var program = GetInputLines();
        var buffer0 = new Queue<long>();
        var buffer1 = new Queue<long>();
        var cpu0 = new Cpu(program) { ["p"] = 0L, InputBuffer = buffer0, OutputBuffer = buffer1 };
        var cpu1 = new Cpu(program) { ["p"] = 1L, InputBuffer = buffer1, OutputBuffer = buffer0 };
        var transmissions1 = 0;

        cpu1.DataTransmitted += _ => transmissions1++;

        var done = false;
        while (!done)
        {
            var ec0 = cpu0.Run(token: default);
            var ec1 = cpu1.Run(token: default);
            var halted = ec0 == Cpu.ExitCode.Halted && ec1 == Cpu.ExitCode.Halted;
            var inputs =
                ec0 == Cpu.ExitCode.AwaitingInput && buffer0.Any() ||
                ec1 == Cpu.ExitCode.AwaitingInput && buffer1.Any();

            done = halted || !inputs;
        }
        
        return transmissions1;
    }
}