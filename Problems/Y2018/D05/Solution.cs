namespace Problems.Y2018.D05;

[PuzzleInfo("Alchemical Reduction", Topics.StringParsing|Topics.Simulation, Difficulty.Easy, favourite: true)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        return part switch
        {
            1 => ReactSimple(),
            2 => ReactFindShortest(),
            _ => ProblemNotSolvedString
        };
    }

    private int ReactSimple()
    {
        var input = GetInputText();
        var chain = BuildChain(input, omitInvariant: null);

        return React(chain);
    }

    private int ReactFindShortest()
    {
        var input = GetInputText();
        var units = Enumerable.Range('A', 26);

        return units.Min(c => React(BuildChain(input, omitInvariant: (char)c)));
    }
     
    private static LinkedList<char> BuildChain(string input, char? omitInvariant)
    {
        var filtered = omitInvariant.HasValue 
            ? input.Where(c => char.ToUpperInvariant(c) != char.ToUpperInvariant(omitInvariant.Value)) 
            : input;
        
        return new LinkedList<char>(filtered);
    }
    
    private static int React(LinkedList<char> chain)
    {
        var node = chain.First!;
        while (node?.Next != null)
        {
            char c1 = node.Value,       c2 = node.Next.Value;
            bool u1 = char.IsUpper(c1), u2 = char.IsUpper(c2);

            if (char.ToUpperInvariant(c1) != char.ToUpperInvariant(c2) || u1 == u2)
            {
                node = node.Next;
                continue;
            }

            var tmp = node.Previous ?? node.Next.Next;
            chain.Remove(node.Next);
            chain.Remove(node);
            node = tmp;
        }
        
        return chain.Count;
    }
}