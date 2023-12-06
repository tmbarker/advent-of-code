using Problems.Common;
using Problems.Y2017.Common;

namespace Problems.Y2017.D18;

/// <summary>
/// Duet: https://adventofcode.com/2017/day/18
/// </summary>
public sealed class Solution : SolutionBase
{
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
        var vm = new Vm(program: GetInputLines())
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

        vm.DataTransmitted += OnDataTransmitted;
        vm.DataReceived += OnDataReceived;
        vm.Run(cts.Token);

        return transmitted;
    }

    private long CountTransmissions()
    {
        var program = GetInputLines();
        var buffer0 = new Queue<long>();
        var buffer1 = new Queue<long>();
        var vm0 = new Vm(program) { ["p"] = 0L, InputBuffer = buffer0, OutputBuffer = buffer1 };
        var vm1 = new Vm(program) { ["p"] = 1L, InputBuffer = buffer1, OutputBuffer = buffer0 };
        var transmissions1 = 0;

        vm1.DataTransmitted += _ => transmissions1++;

        var done = false;
        while (!done)
        {
            var ec0 = vm0.Run(token: default);
            var ec1 = vm1.Run(token: default);
            var halted = ec0 == Vm.ExitCode.Halted && ec1 == Vm.ExitCode.Halted;
            var inputs =
                ec0 == Vm.ExitCode.AwaitingInput && buffer0.Any() ||
                ec1 == Vm.ExitCode.AwaitingInput && buffer1.Any();

            done = halted || !inputs;
        }
        
        return transmissions1;
    }
}