using Utilities.Cartesian;

namespace Problems.Y2021.D23;

public readonly struct State
{
    public int Cost { get; }
    public Dictionary<Vector2D, char> AmphipodsMap { get; }

    private State (int cost, IDictionary<Vector2D, char> positions)
    {
        Cost = cost;
        AmphipodsMap = new Dictionary<Vector2D, char>(positions);
    }

    public static State FromInitialPositions(IDictionary<Vector2D, char> positions)
    {
        return new State(0, positions);
    }
    
    public State AfterMove(Move move)
    {
        var nextPositions = new Dictionary<Vector2D, char>();
        foreach (var (pos, amphipod) in AmphipodsMap)
        {
            nextPositions.Add(pos == move.From ? move.To : pos, amphipod);
        }

        return new State(Cost + move.Cost, nextPositions);
    }
}