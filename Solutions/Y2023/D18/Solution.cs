using System.Text.RegularExpressions;
using Utilities.Extensions;

namespace Solutions.Y2023.D18;

[PuzzleInfo("Lavaduct Lagoon", Topics.Math, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private readonly record struct Point(long X, long Y);
    private readonly record struct Instruction(long N, char D);
    
    public override object Run(int part)
    {
        return part switch
        {
            1 => Measure(hex: false),
            2 => Measure(hex: true),
            _ => ProblemNotSolvedString
        };
    }

    private long Measure(bool hex)
    {
        var vertex = new Point(X: 0, Y: 0);
        var vertices = new List<Point>();
        var perimeter = 0L;

        foreach (var instr in ParseInputLines(parseFunc: line => ParseInstruction(line, hex)))
        {
            vertex = instr.D switch
            {
                'U' => vertex with { Y = vertex.Y + instr.N },
                'D' => vertex with { Y = vertex.Y - instr.N },
                'R' => vertex with { X = vertex.X + instr.N },
                'L' => vertex with { X = vertex.X - instr.N },
                _ => throw new NoSolutionException()
            };
            
            vertices.Add(vertex);
            perimeter += instr.N;
        }
        
        //  The interior area of a polygon can be calculated using Pick's Theorem:
        //  I = A - B/2 + 1
        //
        var area = PolygonArea(vertices);
        var interior = area - perimeter / 2 + 1;
        
        return interior + perimeter;
    }

    private static long PolygonArea(IReadOnlyList<Point> v)
    {
        //  The area of a polygon bounded by vertices 'v' can be calculated using the Shoelace Formula:
        //  A = 1/2 * |Sum v[i] ^ v[i + 1]|
        //
        var a = 0L;
        var n = v.Count;
        
        for (var i = 0; i < v.Count; i++)
        {
            a += v[i].X * v[(i + 1) % n].Y - v[(i + 1) % n].X * v[i].Y;
        }
        
        return Math.Abs(a) / 2;
    }
    
    private static Instruction ParseInstruction(string line, bool hex)
    {
        var match = Regex.Match(line, pattern: @"(?<D>[UDLR]) (?<N>\d+) \(#(?<H>[0-9a-f]+)\)");
        if (!hex)
        {
            return new Instruction(N: match.Groups["N"].ParseLong(), D: match.Groups["D"].Value[0]);
        }
        
        var hexStr = match.Groups["H"].Value;
        var hexVal = Convert.ToInt32(hexStr[..^1], fromBase: 16);
        
        return new Instruction(N: hexVal, D: hexStr[^1] switch
        {
            '0' => 'R',
            '1' => 'D',
            '2' => 'L',
            '3' => 'U',
            _ => throw new NoSolutionException()
        });
    }
}