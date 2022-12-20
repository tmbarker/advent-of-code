namespace Problems.Y2022.D19;

[Flags]
public enum Materials
{
    Ore =      1 << 0,
    Clay =     1 << 1,
    Obsidian = 1 << 2,
    Geode =    1 << 3,
    All =          ~0,
}