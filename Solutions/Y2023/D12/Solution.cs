using Utilities.Extensions;

namespace Solutions.Y2023.D12;

[PuzzleInfo("Hot Springs", Topics.StringParsing, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Report(string Pattern, int[] Runs);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => SumArrangements(factor: 1),
            2 => SumArrangements(factor: 5),
            _ => ProblemNotSolvedString
        };
    }
    
    private long SumArrangements(int factor)
    {
        var sum = 0L;
        var reports = ParseInputLines(parseFunc: line => ParseReport(line, factor));

        Parallel.ForEach(reports, report =>
        {
            Interlocked.Add(ref sum, value: CountArrangements(report));
        });
        
        return sum;
    }
    
    private static long CountArrangements(Report report)
    {
        var memo = new Dictionary<State, long>();
        var initial = State.Initial(report.Pattern);
        
        return CountArrangements(r: report, s: initial, m: memo);
    }
    
    private static long CountArrangements(Report r, State s, IDictionary<State, long> m)
    {
        if (m.TryGetValue(s, out var cached))
        {
            return cached;
        }
            
        if (s.Pos == r.Pattern.Length)
        {
            var tailRunValid = !s.InRun || s.RunLength == r.Runs[s.RunIndex];
            var matchSuccess = tailRunValid && s.RunIndex + 1 == r.Runs.Length;

            m[s] = matchSuccess ? 1L : 0L;
            return m[s];
        }

        switch (current: s.Current, inRun: s.InRun)
        {
            case (current: '.', inRun: true)  when s.RunLength != r.Runs[s.RunIndex]:
            case (current: '#', inRun: true)  when s.RunLength >= r.Runs[s.RunIndex]:
            case (current: '#', inRun: false) when s.RunIndex + 1 >= r.Runs.Length:
                m[s] = 0L;
                break; 
            case (current: '?', inRun: _):
                m[s] = CountArrangements(r, s.Replace('.'), m) + CountArrangements(r, s.Replace('#'), m);
                break;
            case (current:  _,  inRun: _):
                m[s] = CountArrangements(r, s.Consume(), m);
                break;
        }

        return m[s];
    }
    
    private static Report ParseReport(string line, int times)
    {
        var elements = line.Split(separator: ' ');
        return new Report(
            Pattern: string.Join(separator: '?', Enumerable.Repeat(elements[0], times)).Trim('.'),
            Runs:    string.Join(separator: ',', Enumerable.Repeat(elements[1], times)).ParseInts());
    }
}