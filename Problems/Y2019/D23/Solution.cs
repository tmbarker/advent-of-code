using Problems.Y2019.IntCode;

namespace Problems.Y2019.D23;

[PuzzleInfo("Category Six", Topics.IntCode, Difficulty.Hard)]
public sealed class Solution : IntCodeSolution
{
    public override object Run(int part)
    {
        var firmware = LoadIntCodeProgram();
        var networkAwaiter = new NetworkAwaiter(firmware);

        return part switch
        {
            1 => networkAwaiter.WaitForMessage(targetRecipient: 255).Result,
            2 => networkAwaiter.WaitForRepeatedNatMessage().Result,
            _ => ProblemNotSolvedString
        };
    }
}