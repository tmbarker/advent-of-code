using Problems.Attributes;
using Problems.Common;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Problems.Y2017.D20;

/// <summary>
/// Particle Swarm: https://adventofcode.com/2017/day/20
/// </summary>
[Favourite("Particle Swarm", Topics.Math|Topics.Vectors, Difficulty.Hard)]
public class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var input = GetInputLines();
        var particles = new Dictionary<int, Particle>();

        for (var i = 0; i < input.Length; i++)
        {
            particles[i] = ParseParticle(input[i]);
        }
        
        return part switch
        {
            1 => GetLongTermClosest(particles),
            2 => CountCollisions(particles),
            _ => ProblemNotSolvedString
        };
    }

    private static int GetLongTermClosest(IDictionary<int, Particle> particles)
    {
        return particles.Keys
            .GroupBy(id => particles[id].Acc.Magnitude(metric: Metric.Taxicab))
            .MinBy(group => group.Key)!
            .MinBy(id => particles[id].Vel.Magnitude(metric: Metric.Taxicab));
    }

    private int CountCollisions(IDictionary<int, Particle> particles)
    {
        if (LogsEnabled)
        {
            Console.WriteLine("Computing collisions");
        }
        
        var collisions = new Dictionary<Collision, HashSet<int>>();
        for (var i = 0; i < particles.Count; i++)
        for (var j = 0; j < particles.Count; j++)
        {
            if (i == j || !ComputeCollision(p1: particles[i], p2: particles[j], out var collision))
            {
                continue;
            }

            collisions.TryAdd(collision, new HashSet<int>());
            collisions[collision].Add(i);
            collisions[collision].Add(j);
        }

        if (LogsEnabled)
        {
            Console.WriteLine("Collisions computed");
            Console.WriteLine("Destroying particles");
        }
        
        foreach (var collision in collisions.Keys.OrderBy(c => c.Tick))
        {
            var presentAtTick = particles.Keys.Freeze();
            var involved = collisions[collision];

            if (involved.Count(presentAtTick.Contains) >= 2)
            {
                particles.RemoveMany(involved);
            }
        }
        
        return particles.Count;
    }
    
    private static bool ComputeCollision(Particle p1, Particle p2, out Collision collision)
    {
        collision = default;
        
        //  In discrete time, the position of a particle is given by the following quadratic kinematic
        //  equation:
        //
        //  p(t) = (1/2)*a*t*(t+1) + v*t + p0 
        //
        var a = p1.Acc - p2.Acc;
        var b = p1.Acc - p2.Acc + 2 * (p1.Vel - p2.Vel);
        var c = 2 * (p1.Pos - p2.Pos);
        
        var sx = TrySolveQuadratic(a: a.X, b: b.X, c: c.X, out var txf);
        var sy = TrySolveQuadratic(a: a.Y, b: b.Y, c: c.Y, out var tyf);
        var sz = TrySolveQuadratic(a: a.Z, b: b.Z, c: c.Z, out var tzf);

        if (!sx || !sy || !sz)
        {
            return false;
        }
        
        var candidates = new List<int>();
        candidates.AddRange(txf.Where(t => t >= 0f).Select(t => (int)Math.Round(t)));
        candidates.AddRange(tyf.Where(t => t >= 0f).Select(t => (int)Math.Round(t)));
        candidates.AddRange(tzf.Where(t => t >= 0f).Select(t => (int)Math.Round(t)));

        foreach (var tick in candidates)
        {
            var pt1 = ScaledPosAtTick(p1, tick);
            var pt2 = ScaledPosAtTick(p2, tick);
            
            if (pt1 == pt2)
            {
                collision = new Collision(Tick: tick, Pos: pt1);
                return true;
            }
        }
        
        return false;
    }

    private static Vector3D ScaledPosAtTick(Particle p, int t)
    {
        //  To avoid integer division loss of fraction from the quadratic term, we can return a 2x scaled vector
        //
        return t * t * p.Acc + t * p.Acc + 2 * t * p.Vel + 2 * p.Pos;
    }
    
    private static bool TrySolveQuadratic(int a, int b, int c, out List<float> x)
    {
        x = new List<float>();
        if (a == 0)
        {
            if (b == 0)
            {
                return c == 0;
            }

            x.Add(-1f * c / b);
            return true;
        }

        var discriminant = b * b - 4f * a * c;
        if (discriminant < 0f)
        {
            return false;
        }

        var x1 = (float)(-b + Math.Sqrt(discriminant)) / (2f * a);
        var x2 = (float)(-b - Math.Sqrt(discriminant)) / (2f * a);

        x.Add(x1);
        x.Add(x2);
        return true;
    }
    
    private static Particle ParseParticle(string line)
    {
        var numbers = line.ParseInts();
        var pos = new Vector3D(x: numbers[0], y: numbers[1], z: numbers[2]);
        var vel = new Vector3D(x: numbers[3], y: numbers[4], z: numbers[5]);
        var acc = new Vector3D(x: numbers[6], y: numbers[7], z: numbers[8]);
        
        return new Particle(pos, vel, acc);
    }
}