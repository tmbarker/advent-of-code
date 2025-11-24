using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2022.D24;

[PuzzleInfo("Blizzard Basin", Topics.Vectors|Topics.Simulation, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    private static readonly Dictionary<char, Vec2D> VelMap = new()
    {
        {'^', Vec2D.Up},
        {'v', Vec2D.Down},
        {'<', Vec2D.Left},
        {'>', Vec2D.Right}
    };

    public override object Run(int part)
    {
        ParseInput(
            input: GetInputLines(),
            out var start,
            out var end,
            out var storm);
        
        return part switch
        {
            1 => Traverse(start, end, storm),
            2 => Navigate(start, end, storm),
            _ => PuzzleNotSolvedString
        };
    }

    private static int Navigate(Vec2D start, Vec2D end, Storm storm)
    {
        var sum = 0;
        sum += Traverse(start, end, storm);
        sum += Traverse(end, start, storm);
        sum += Traverse(start, end, storm);
        return sum;
    }
    
    private static int Traverse(Vec2D start, Vec2D end, Storm storm)
    {
        var t = 0;
        var heads = new HashSet<Vec2D>(collection: [start]);

        while (heads.Count > 0)
        {
            t++;
            storm.Step();
            
            foreach (var head in heads.Freeze())
            {
                if (storm.OccupiedPositions.Contains(head))
                {
                    heads.Remove(head);
                }

                foreach (var move in storm.GetSafeMoves(head))
                {
                    if (move == end)
                    {
                        return t;
                    }
                    heads.Add(move);
                }
            }
        }

        throw new NoSolutionException();
    }
    
    private static void ParseInput(string[] input,
        out Vec2D start, 
        out Vec2D end, 
        out Storm storm)
    {
        start = new Vec2D(X: input[0].IndexOf(Terrain.Void),  Y: input.Length - 1);
        end =   new Vec2D(X: input[^1].IndexOf(Terrain.Void), Y: 0);
        
        var field = input.ToGrid();
        var blizzards = new List<Blizzard>();
        
        for (var y = 0; y < field.Height; y++)
        for (var x = 0; x < field.Width;  x++)
        {
            if (!VelMap.TryGetValue(field[x, y], out var vel))
            {
                continue;
            }

            var pos = new Vec2D(x, y);
            field[x, y] = Terrain.Void;
            blizzards.Add(item: new Blizzard(
                pos: pos,
                vel: vel,
                respawnAt: CalculateRespawnPos(pos, vel, field)));
        }

        storm = new Storm(field, blizzards);
    }

    private static Vec2D CalculateRespawnPos(Vec2D pos, Vec2D vel, Grid2D<char> field)
    {
        var target = pos;
        while (field[target] != Terrain.Wall)
        {
            target -= vel;
        }
        return target + vel;
    }
}