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
            1 => Output(cx, inputs: cs[0]),
            2 => Repair(cx),
            _ => PuzzleNotSolvedString
        };
    }

    private static long Output(Circuit cx, string[] inputs)
    {
        var vs = new Values();
        foreach (var line in inputs)
        {
            var elements = line.Split(": ");
            vs[elements[0]] = int.Parse(elements[1]);
        }
        return Compute(cx, vs);
    }

    private static long Compute(Circuit cx, Values vs)
    {
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
            var hasXIn  = inA.StartsWith('x') || inB.StartsWith('x');
            var hasYIn  = inA.StartsWith('y') || inB.StartsWith('y');
            var hasZOut = @out.StartsWith('z');

            var first = inA == "x00" && inB == "y00";
            var last  = @out == "z45";
            
            if (hasZOut && !last && op != "XOR")
            {
                problems.Add(@out);
            }
            
            if (!hasZOut && (!hasXIn || !hasYIn) && op == "XOR")
            {
                problems.Add(@out);
            }

            if (!first && op == "XOR" && hasXIn  && hasYIn)
            {
                if (!cx.Keys.Except([@out]).Any(other => 
                        cx[other].Op == "XOR" && 
                        (cx[other].InA == @out || cx[other].InB == @out)))
                {
                    problems.Add(@out);
                }
            }
            
            if (!first && op == "AND")
            {
                if (!cx.Keys.Except([@out]).Any(other =>
                        cx[other].Op == "OR" &&
                        (cx[other].InA == @out || cx[other].InB == @out)))
                {
                    problems.Add(@out);
                }
            }
        }

        return string.Join(',', problems.Order());
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