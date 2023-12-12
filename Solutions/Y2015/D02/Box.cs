namespace Solutions.Y2015.D02;

public readonly struct Box(long l, long w, long h)
{
    private long Lw => l * w;
    private long Wh => w * h;
    private long Lh => l * h;
    private long MinFace => new[] { Lw, Wh, Lh }.Min();
    private long MinPerimeter => new[] { 2 * l + 2 * w, 2 * w + 2 * h, 2 * l + 2 * h }.Min();
    private long Volume => l * w * h;

    public long PaperReq => 2L * Lw + 2L * Wh + 2L * Lh + MinFace;
    public long RibbonReq => MinPerimeter + Volume;
}