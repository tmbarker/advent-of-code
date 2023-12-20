using Utilities.Numerics;

namespace Solutions.Y2023.D20;

[PuzzleInfo("Pulse Propagation", Topics.Graphs|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var network = Network.Parse(input);
        
        return part switch
        {
            1 => AggregatePulses(network, b: 1000),
            2 => GetFirstRxSignal(network),
            _ => ProblemNotSolvedString
        };
    }

    private static long AggregatePulses(Network network, int b)
    {
        return network
            .Simulate(b).Pulses.Values
            .Aggregate(seed: 1L, func: (product, count) => product * count);
    }

    private static long GetFirstRxSignal(Network network)
    {
        var trace = network.Simulate(b: 25000);
        var cycles = network.ReceiverInputs
            .Select(id => trace.Watches[id][^1] - trace.Watches[id][^2])
            .ToList();

        return Numerics.Lcm(cycles);
    }
}