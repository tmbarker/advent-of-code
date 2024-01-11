using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2016.D11;

[PuzzleInfo("Radioisotope Thermoelectric Generators", Topics.Graphs, Difficulty.Hard)]
public sealed class Solution : SolutionBase
{
    private static readonly Regex MicrochipRegex = new(pattern: @"([a-z]+)-compatible microchip");
    private static readonly Regex GeneratorRegex = new(pattern: @"([a-z]+) generator");

    private static readonly List<Device> ExtraDevices =
    [
        new Device(Type: DeviceType.Generator, Element: "elerium",   Floor: 1),
        new Device(Type: DeviceType.Microchip, Element: "elerium",   Floor: 1),
        new Device(Type: DeviceType.Generator, Element: "dilithium", Floor: 1),
        new Device(Type: DeviceType.Microchip, Element: "dilithium", Floor: 1)
    ];
    
    public override object Run(int part)
    {
        var input = GetInputLines();
        var devices = ParseDevices(input);
        
        return part switch
        {
            1 => Search(devices, extra: false),
            2 => Search(devices, extra: true),
            _ => ProblemNotSolvedString
        };
    }

    private static int Search(List<Device> devices, bool extra)
    {
        if (extra)
        {
            devices.AddRange(ExtraDevices);
        }

        var initial = new State(elevator: 1, devices); 
        var queue = new Queue<State>(collection: [initial]);
        var visited = new HashSet<State>(collection: [initial]);
        var depth = 0;

        while (queue.Any())
        {
            var nodesAtDepth = queue.Count;
            while (nodesAtDepth-- > 0)
            {
                var state = queue.Dequeue();
                if (state.IsFinished)
                {
                    return depth;
                }

                var adjacent = GetValidAdjacent(state);
                var unvisited = adjacent.Where(adj => !visited.Contains(adj));

                foreach (var adj in unvisited)
                {
                    visited.Add(adj);
                    queue.Enqueue(adj);
                }
            }

            depth++;
        }

        throw new NoSolutionException();
    }

    private static IEnumerable<State> GetValidAdjacent(State state)
    {
        var elevatorSteps = new List<int>();
        if (state.Elevator < 4) elevatorSteps.Add(item:  1);
        if (state.Elevator > 1) elevatorSteps.Add(item: -1);
        
        var options = state.Devices.Where(device => device.Floor == state.Elevator);
        var combinations = GetCombinations(options.ToList());
        
        foreach (var combination in combinations)
        foreach (var step in elevatorSteps)
        {
            var adjacent = BuildAdjacentState(state, step, combination);
            if (CheckDevicesSafe(adjacent.Devices))
            {
                yield return adjacent;
            }
        }
    }

    private static IEnumerable<IList<Device>> GetCombinations(IList<Device> devices)
    {
        if (devices.Count == 1)
        {
            return Enumerable.Repeat(devices, 1);
        }

        var choices = new List<IList<Device>>();
        choices.AddRange(devices.Combinations(k: 1).Select(c => c.ToList()));
        choices.AddRange(devices.Combinations(k: 2).Select(c => c.ToList()));
        return choices;
    }

    private static State BuildAdjacentState(State current, int step, IList<Device> move)
    {
        var elevator = current.Elevator + step;
        var devices = new List<Device>();

        foreach (var device in current.Devices)
        {
            if (move.Any(candidate => candidate.Type == device.Type && candidate.Element == device.Element))
            {
                devices.Add(device with { Floor = device.Floor + step });
            }
            else
            {
                devices.Add(device);
            }
        }
        
        return new State(elevator, devices);
    }
    
    private static bool CheckDevicesSafe(IList<Device> devices)
    {
        foreach (var microchip in devices.Where(device => device.Type == DeviceType.Microchip))
        {
            var generatorsOnFloor = devices
                .Where(device => device.Type == DeviceType.Generator && device.Floor == microchip.Floor)
                .ToList();

            if (generatorsOnFloor.Any() && generatorsOnFloor.All(generator => generator.Element != microchip.Element))
            {
                return false;
            }
        }

        return true;
    }
    
    private static List<Device> ParseDevices(IList<string> input)
    {
        var devices = new List<Device>();
        for (var i = 0; i < input.Count; i++)
        {
            var microchips = MicrochipRegex.Matches(input[i]).SelectCaptures(group: 1);
            var generators = GeneratorRegex.Matches(input[i]).SelectCaptures(group: 1);

            devices.AddRange(microchips.Select(element => new Device(Type: DeviceType.Microchip, Element: element, Floor: i + 1)));
            devices.AddRange(generators.Select(element => new Device(Type: DeviceType.Generator, Element: element, Floor: i + 1)));
        }
        
        return devices;
    }
}