namespace Solutions.Y2020.D08;

using Instructions = List<(string Op, int Arg)>;

[PuzzleInfo("Handheld Halting", Topics.Assembly|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var instructions = ParseInstructions(input);
        
        return part switch
        {
            1 => Machine.Run(instructions).Acc,
            2 => GetTerminatedResult(instructions),
            _ => PuzzleNotSolvedString
        };
    }
    
    private static int GetTerminatedResult(Instructions instructions)
    {
        for (var i = 0; i < instructions.Count; i++)
        {
            var modified = ModifyProgram(i, instructions);
            var result = Machine.Run(modified);

            if (!result.Looped)
            {
                return result.Acc;
            }
        }

        throw new NoSolutionException();
    }
    
    private static Instructions ModifyProgram(int i, Instructions instructions)
    {
        var modified = new Instructions(instructions);
        var target = modified[i];

        modified[i] = target.Op switch
        {
            Machine.Jmp => (Machine.Nop, target.Arg),
            Machine.Nop => (Machine.Jmp, target.Arg),
            _ => modified[i]
        };

        return modified;
    }
    
    private static Instructions ParseInstructions(IEnumerable<string> program)
    {
        var instructions = new Instructions();
        
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