using Utilities.Collections;

namespace Solutions.Y2015.D07;

using Memo = Dictionary<string, uint>;

[PuzzleInfo("Some Assembly Required", Topics.Graphs|Topics.BitwiseOperations, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private const string Buffer = "BUFFER";
    private const string And    = "AND";
    private const string Or     = "OR";
    private const string Not    = "NOT";
    private const string LShift = "LSHIFT";
    private const string RShift = "RSHIFT";

    private static readonly HashSet<string> Operators = [Buffer, And, Or, Not, LShift, RShift];

    public override object Run(int part)
    {
        var instructions = GetInputLines();
        var circuit = AssembleCircuit(instructions);
        var target = EvaluateSignal(circuit, memo: [], gate: "a");

        return part switch
        {
            1 => target,
            2 => EvaluateSignal(circuit, memo: new Memo { { "b", target } }, gate: "a"),
            _ => PuzzleNotSolvedString
        };
    }

    private static uint EvaluateSignal(Circuit circuit, Memo memo, string gate)
    {
        if (memo.TryGetValue(gate, out var cached))
        {
            return cached;
        }
        
        if (uint.TryParse(gate, out var source))
        {
            memo[gate] = source;
            return source;
        }
        
        var args = circuit.GateInputs[gate]
            .Select(input => EvaluateSignal(circuit, memo, input))
            .ToList();

        var signal = circuit.GateTypes[gate] switch
        {
            Buffer =>  args[0],
            And    =>  args[0] & args[1],
            Or     =>  args[0] | args[1],
            Not    => ~args[0],
            LShift =>  args[0] << (int)args[1],
            RShift =>  args[0] >> (int)args[1],
            _ => throw new NoSolutionException()
        } & 0xFFFF;

        memo[gate] = signal;
        return signal;
    }
    
    private static Circuit AssembleCircuit(IEnumerable<string> instructions)
    {
        var gateInputs = new DefaultDict<string, HashSet<string>>(defaultSelector: _ => []);
        var gateTypes = new Dictionary<string, string>(); 

        foreach (var line in instructions)
        {
            var tokens = line.Split(' ');
            var to = tokens[^1];
            var op = Buffer;
            
            foreach (var token in tokens[..^2])
            {
                if (Operators.Contains(token))
                {
                    op = token;
                }
                else
                {
                    gateInputs[to].Add(token);
                }
            }
            
            gateTypes.Add(to, op);
        }

        return new Circuit(gateInputs, gateTypes);
    }

    private readonly record struct Circuit(
        IDictionary<string, HashSet<string>> GateInputs,
        IDictionary<string, string> GateTypes);
}