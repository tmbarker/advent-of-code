using Problems.Y2019.Common;

namespace Problems.Y2019.D23;

/// <summary>
/// Category Six: https://adventofcode.com/2019/day/23
/// </summary>
public class Solution : SolutionBase2019
{
    public override int Day => 23;
    
    public override object Run(int part)
    {
        var firmware = LoadIntCodeProgram();
        var networkAwaiter = new NetworkAwaiter(firmware);

        return part switch
        {
            0 => networkAwaiter.WaitForMessage(targetRecipient: 255).Result,
            1 => networkAwaiter.WaitForRepeatedNatMessage().Result,
            _ => ProblemNotSolvedString
        };
    }
}