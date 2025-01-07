namespace Solutions.Y2024.D24;

using Values  = Dictionary<string, int>;
using Circuit = Dictionary<string, (string InA, string Op, string InB)>;

[PuzzleInfo("Crossed Wires", Topics.BitwiseOperations, Difficulty.Hard)]
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var cs = ChunkInputByNonEmpty();
        var cx = new Circuit();
        
        foreach (var line in cs[1])
        {
            var elements = line.Split(" ");
            cx[elements[^1]] = (InA: elements[0], Op: elements[1], InB: elements[2]);
        }
        
        return part switch
        {
            1 => Compute(cx, inputs: cs[0]),
            2 => Repair(cx),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Compute(Circuit cx, string[] inputs)
    {
        var vs = new Values();
        foreach (var line in inputs)
        {
            var elements = line.Split(": ");
            vs[elements[0]] = int.Parse(elements[1]);
        }

        return cx.Keys
            .Where(wire => wire.StartsWith('z'))
            .Select(wire => (long)Math.Pow(2, int.Parse(wire[1..])) * GetBit(wire, vs, cx))
            .Sum();
    }

    private static string Repair(Circuit cx)
    {
        var problems = new HashSet<string>();
        foreach (var (@out, (inA, op, inB)) in cx)
        {
            var isFinal = @out is "z45";
            var isFirst = inA is "x00" && inB is "y00";
            var isOutput = @out[0] is 'z';
            var isInput = inA[0] is 'x' or 'y' && inB[0] is 'x' or 'y';

            if ( isOutput && !isFinal && op != "XOR") problems.Add(@out);
            if (!isOutput && !isInput && op == "XOR") problems.Add(@out);
            if (!isFirst  &&  isInput && op == "XOR" && !FindUsages(cx, @out, "XOR")) problems.Add(@out);
            if (!isFirst              && op == "AND" && !FindUsages(cx, @out, "OR"))  problems.Add(@out);
        }

        return string.Join(',', problems.Order());
    }

    private static bool FindUsages(Circuit cx, string ofWire, string withOp)
    {
        return cx.Keys
            .Except([ofWire])
            .Any(other => cx[other].Op == withOp && (cx[other].InA == ofWire || cx[other].InB == ofWire));
    }
    
    private static int GetBit(string id, Values vals, Circuit cx)
    {
        if (vals.TryGetValue(id, out var cached))
        {
            return cached;
        }

        var (wa, op, wb) = cx[id];
        var va = GetBit(wa, vals, cx);
        var vb = GetBit(wb, vals, cx);
        var val = op switch
        {
            "AND" => va & vb,
            "XOR" => va ^ vb,
            "OR"  => va | vb,
            _ => throw new InvalidOperationException($"Op not supported: {op}")
        };
        
        vals[id] = val;
        return val;
    }
}