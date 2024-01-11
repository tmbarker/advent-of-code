namespace Utilities.Geometry.Hexagonal;

/// <summary>
/// A readonly integral hexagon value type. Represents a 2D hexagon using a 3D cubic coordinate system 
/// </summary>
public readonly record struct Hex
{
    public static readonly Hex Zero = new(q: 0, r: 0, s: 0);
    public static readonly IReadOnlyDictionary<Enum, Hex> Directions = new Dictionary<Enum, Hex>
    {
        { Flat.N,  new(q:  0, r: -1, s:  1) },
        { Flat.Ne, new(q:  1, r: -1, s:  0) },
        { Flat.Se, new(q:  1, r:  0, s: -1) },
        { Flat.S,  new(q:  0, r:  1, s: -1) },
        { Flat.Sw, new(q: -1, r:  1, s:  0) },
        { Flat.Nw, new(q: -1, r:  0, s:  1) },

        { Pointy.Nw, new(q:  0, r: -1, s:  1) },
        { Pointy.Ne, new(q:  1, r: -1, s:  0) },
        { Pointy.E,  new(q:  1, r:  0, s: -1) },
        { Pointy.Se, new(q:  0, r:  1, s: -1) },
        { Pointy.Sw, new(q: -1, r:  1, s:  0) },
        { Pointy.W,  new(q: -1, r:  0, s:  1) }
    };
    
    public int Q { get; }
    public int R { get; }
    public int S { get; }

    public int Magnitude => Distance(a: this, b: Zero);

    public Hex(int q, int r, int s)
    {
        if (q + r + s != 0)
        {
            throw new ArgumentException(
                message: $"Coordinates do not meet the Hex constraint that Q + R + S = 0: [q={Q},r={R},s={S}]");
        }

        Q = q;
        R = r;
        S = s;
    }
    
    public ISet<Hex> GetAdjacentSet()
    {
        return new HashSet<Hex>
        {
            this + Directions[Flat.N],
            this + Directions[Flat.Ne],
            this + Directions[Flat.Se],
            this + Directions[Flat.S],
            this + Directions[Flat.Sw],
            this + Directions[Flat.Nw]
        };
    }
    
    public static int Distance(Hex a, Hex b)
    {
        var dq = Math.Abs(a.Q - b.Q);
        var dr = Math.Abs(a.R - b.R);
        var ds = Math.Abs(a.S - b.S);

        return (dq + dr + ds) / 2;
    }
    
    public static Hex operator +(Hex lhs, Hex rhs)
    {
        return new Hex(lhs.Q + rhs.Q, lhs.R + rhs.R, lhs.S + rhs.S);
    }

    public static Hex operator -(Hex lhs, Hex rhs)
    {
        return new Hex(lhs.Q - rhs.Q, lhs.R - rhs.R, lhs.S - rhs.S);
    }
    
    public override string ToString()
    {
        return $"[q={Q},r={R},s={S}]";
    }
}