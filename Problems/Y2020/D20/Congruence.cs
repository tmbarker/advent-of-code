namespace Problems.Y2020.D20;

public readonly struct Congruence
{
    public Congruence(int toTile, Tile.EdgeId fromEdge, Tile.EdgeId toEdge)
    {
        FromEdge = fromEdge;
        ToEdge = toEdge;
        ToTile = toTile;
    }
    
    public Tile.EdgeId FromEdge { get; }
    public Tile.EdgeId ToEdge { get; }
    public int ToTile { get; }
}