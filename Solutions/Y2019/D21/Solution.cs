using Solutions.Y2019.IntCode;

namespace Solutions.Y2019.D21;

[PuzzleInfo("Springdroid Adventure", Topics.IntCode, Difficulty.Hard)]
public sealed class Solution : IntCodeSolution
{
    // The springdroid jumps 4 steps, in WALK mode it's sensors can see 4 steps ahead. The best strategy we can employ
    // is as follows:
    //     1. If there is a hole 1 step in front of the droid, we must jump
    //     2. If there is a hole 3 steps in front of the droid, and ground at 4 steps, we should bias towards jumping
    //     3. Don't jump straight into a hole
    private static readonly List<string> WalkScript =
    [
        "NOT A J", // (1)

        "NOT C T", // (2)
        "OR T J",

        "AND D J", // (3)

        "WALK"
    ];
    
    // The springdroid jumps 4 steps, in RUN mode it's sensors can see 9 steps ahead. A possible strategy we can employ
    // is as follows:
    //     1. If there is a hole 2 or 3 steps in front of the droid, and there is ground 4 steps ahead, we should
    //        bias towards jumping "early", however
    //     2. We do not want to jump early if there is ground 5 steps away, as we could jump one step later, meaning
    //        that we would land "closer" the hole
    //     3. If there is a hole 1 step in front of the droid, we must jump
    private static readonly List<string> RunScript =
    [
        "NOT B J", // (1)
        "NOT C T",
        "OR T J",
        "AND D J",

        "AND H J", // (2)

        "NOT A T", // (3)
        "OR T J",

        "RUN"
    ];

    public override object Run(int part)
    {
        return part switch
        {
            1 => RunSpringdroid(WalkScript),
            2 => RunSpringdroid(RunScript),
            _ => PuzzleNotSolvedString
        };
    }

    private string RunSpringdroid(IEnumerable<string> script)
    {
        var firmware = LoadIntCodeProgram();
        var success = Springdroid.Run(firmware, script, out var output);

        if (success)
        {
            return output;
        }


        Log($"Failure:\n{output}");
        return PuzzleNotSolvedString;
    }
}