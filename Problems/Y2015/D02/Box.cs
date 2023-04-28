namespace Problems.Y2015.D02;

public readonly struct Box
{
    private readonly long _l;
    private readonly long _w;
    private readonly long _h;

    private long Lw => _l * _w;
    private long Wh => _w * _h;
    private long Lh => _l * _h;
    private long MinFace => new[] { Lw, Wh, Lh }.Min();
    private long MinPerimeter => new[] { 2 * _l + 2 * _w, 2 * _w + 2 * _h, 2 * _l + 2 * _h }.Min();
    private long Volume => _l * _w * _h;

    public long PaperReq => 2L * Lw + 2L * Wh + 2L * Lh + MinFace;
    public long RibbonReq => MinPerimeter + Volume;
    
    public Box(long l, long w, long h)
    {
        _l = l;
        _w = w;
        _h = h;
    }
}