namespace Problems.Attributes;

[Flags]
public enum Topics
{
    None =                1 << 0,
    Vectors =             1 << 1,
    Graphs =              1 << 2,
    Recursion =           1 << 3,
    StringParsing =       1 << 4,
    RegularExpressions =  1 << 5,
    Assembly =            1 << 6,
    Math =                1 << 7,
}