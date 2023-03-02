namespace Problems.Attributes;

[Flags]
public enum Topics
{
    None =                0,
    Vectors =             1 << 0,
    Graphs =              1 << 1,
    Recursion =           1 << 2,
    StringParsing =       1 << 3,
    RegularExpressions =  1 << 4,
    Assembly =            1 << 5,
    Math =                1 << 6,
    IntCode =             1 << 7,
}