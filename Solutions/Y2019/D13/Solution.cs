using Solutions.Y2019.IntCode;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2019.D13;

[PuzzleInfo("Care Package", Topics.IntCode, Difficulty.Medium, favourite: true)]
public sealed class Solution : IntCodeSolution
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => CountGameObjects(type: GameObject.Block),
            2 => GetWinningScore(print: LogsEnabled),
            _ => PuzzleNotSolvedString
        };
    }

    private int CountGameObjects(GameObject type)
    {
        var vm = IntCodeVm.Create(LoadIntCodeProgram());
        vm.Run();

        return new Screen(vm.OutputBuffer).GetCount(type);
    }

    private long GetWinningScore(bool print)
    {
        var drawAt = (0, 0);
        if (print)
        {
            drawAt = Console.GetCursorPosition();
            Console.CursorVisible = false;
        }
        
        var program = LoadFreeToPlayProgram();
        var arcadeMachine = IntCodeVm.Create(program);
        
        arcadeMachine.InputBuffer.Enqueue(Joystick.Neutral);
        arcadeMachine.Run();

        var screen = new Screen(arcadeMachine.OutputBuffer);
        while (screen.GetCount(GameObject.Block) > 0)
        {
            arcadeMachine.InputBuffer.Enqueue(ComputeJoystickInput(screen.Ball, screen.Paddle));
            arcadeMachine.Run();
            screen.UpdatePixels(arcadeMachine.OutputBuffer);

            if (print)
            {
                screen.Print(drawAt);
            }
        }

        Console.CursorVisible = true;
        return screen.Score;
    }

    private static long ComputeJoystickInput(Vec2D ball, Vec2D paddle)
    {
        if (ball.X == paddle.X)
        {
            return Joystick.Neutral;
        }
        
        return ball.X > paddle.X 
            ? Joystick.Right 
            : Joystick.Left;
    }

    private IList<long> LoadFreeToPlayProgram()
    {
        var program = LoadIntCodeProgram();
        program[0] = 2;
        return program;
    }
}