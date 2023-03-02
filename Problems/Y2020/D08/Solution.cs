using Problems.Y2020.Common;

namespace Problems.Y2020.D08;

using Instructions = IList<(string Op, int Arg)>;

/// <summary>
/// Handheld Halting: https://adventofcode.com/2020/day/8
/// </summary>
public class Solution : SolutionBase2020
{
    public override int Day => 8;
    
    public override object Run(int part)
    {
        var instructions = ParseInstructions(GetInputLines());
        return part switch
        {
            1 =>  GetAccumulatorBeforeFirstLoop(instructions).Result,
            2 =>  GetResultAfterInstructionFixup(instructions).Result,
            _ => ProblemNotSolvedString
        };
    }

    private static async Task<int> GetAccumulatorBeforeFirstLoop(Instructions instructions)
    {
        var machine = new Machine();
        var accAtLoop = 0;
        
        var cts = new CancellationTokenSource();
        void OnLoopDetected(int acc)
        {
            cts.Cancel();
            accAtLoop = acc;
        }
        
        machine.LoopDetected += OnLoopDetected;
        await machine.Run(instructions, cts.Token);
        
        return accAtLoop;
    }

    private static async Task<int> GetResultAfterInstructionFixup(Instructions instructions)
    {
        var machine = new Machine();
        var result = 0;
        var loopDetected = true;

        for (var i = 0; i < instructions.Count && loopDetected; i++)
        {
            var cts = new CancellationTokenSource();
            void OnLoopDetected(int acc)
            {
                loopDetected = true;
                cts.Cancel();
            }

            loopDetected = false;
            machine.LoopDetected -= OnLoopDetected;
            machine.LoopDetected += OnLoopDetected;
            
            result = await machine.Run(ModifyInstructions(i, instructions), cts.Token);
        }

        return result;
    }
    
    private static Instructions ModifyInstructions(int at, Instructions instructions)
    {
        var modifiedInstructions = new List<(string Op, int Arg)>(instructions);
        var targetInstruction = modifiedInstructions[at];

        modifiedInstructions[at] = targetInstruction.Op switch
        {
            Machine.Jmp => (Machine.Nop, targetInstruction.Arg),
            Machine.Nop => (Machine.Jmp, targetInstruction.Arg),
            _ => modifiedInstructions[at]
        };

        return modifiedInstructions;
    }
    
    private static Instructions ParseInstructions(IEnumerable<string> program)
    {
        var instructions = new List<(string, int)>();
        foreach (var line in program)
        {
            var elements = line.Split(' ');
            var op = elements[0];
            var arg = int.Parse(elements[1]);

            instructions.Add((op, arg));
        }

        return instructions;
    }
}