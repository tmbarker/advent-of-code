using Utilities.Collections;
using Utilities.Extensions;
using Utilities.Geometry.Euclidean;

namespace Solutions.Y2017.D20;

[PuzzleInfo("Particle Swarm", Topics.Math|Topics.Vectors, Difficulty.Hard, favourite: true)]
public sealed class Solution : SolutionBase
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
            _ => PuzzleNotSolvedString
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
        Log("Computing collisions");
        
        var collisions = new DefaultDict<Collision, HashSet<int>>(defaultSelector: _ => []);
        var surviving = new HashSet<int>(collection: particles.Keys);
        
        for (var i = 0; i < particles.Count - 1; i++)
        for (var j = i + 1; j < particles.Count; j++)
        {
            if (ComputeCollision(p1: particles[i], p2: particles[j], out var collision))
            {
                collisions[collision].Add(i);
                collisions[collision].Add(j);
            }
        }

        Log("Collisions computed");
        Log("Destroying particles");
        
        foreach (var collision in collisions.Keys.OrderBy(c => c.Tick))
        {
            var involved = collisions[collision];
            if (involved.Count(surviving.Contains) >= 2)
            {
                surviving.ExceptWith(involved);
            }
        }
        
        return surviving.Count;
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

    private static Vec3D ScaledPosAtTick(Particle p, int t)
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
        var pos = new Vec3D(X: numbers[0], Y: numbers[1], Z: numbers[2]);
        var vel = new Vec3D(X: numbers[3], Y: numbers[4], Z: numbers[5]);
        var acc = new Vec3D(X: numbers[6], Y: numbers[7], Z: numbers[8]);
        
        return new Particle(pos, vel, acc);
    }
}