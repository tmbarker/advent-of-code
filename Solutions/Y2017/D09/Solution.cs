namespace Solutions.Y2017.D09;

[PuzzleInfo("Stream Processing", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Summary(int Score, int Garbage);
    
    public override object Run(int part)
    {
        var input = GetInputText();
        var stream = new Queue<char>(input);
        
        return part switch
        {
            1 => Process(stream).Score,
            2 => Process(stream).Garbage,
            _ => ProblemNotSolvedString
        };
    }

    private static Summary Process(Queue<char> stream)
    {
        var score = 0;
        var garbage = 0;
        var depth = 0;
        var scope = default(Scope);

        while (stream.Count > 0)
        {
            switch (scope, stream.Dequeue())
            {
                case (Scope.Garbage, '>'):
                    scope = Scope.Group;
                    break;
                case (Scope.Garbage, '!'):
                    stream.Dequeue();
                    break;
                case (Scope.Garbage, _):
                    garbage++;
                    break;
                case (Scope.Group, '<'):
                    scope = Scope.Garbage;
                    break;
                case (Scope.Group, '{'):
                    depth++;
                    break;
                case (Scope.Group, '}'):
                    score += depth--;
                    break;
            }
        }

        return new Summary(score, garbage);
    }
}