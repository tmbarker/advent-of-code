namespace Problems.Y2018.D12;

public static class Input
{
    public static void Parse(IList<string> input, out HashSet<long> state, out Dictionary<uint, bool> rules)
    {
        state = new HashSet<long>();
        rules = new Dictionary<uint, bool>();

        for (var i = 2; i < input.Count; i++)
        {
            var mask = 0U;
            for (var j = 0; j < 5; j++)
            {
                if (input[i][j] == '#')
                {
                    mask |= 1U << j;
                }
            }

            rules.Add(mask, input[i].Last() == '#');
        }

        var initialStr = input[0]
            .Where(c => c is '#' or '.')
            .ToList();
        
        for (var i = 0; i < initialStr.Count; i++)
        {
            if (initialStr[i] == '#')
            {
                state.Add(i);
            }
        }
    }
}