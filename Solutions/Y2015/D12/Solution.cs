using System.Text.Json;

namespace Solutions.Y2015.D12;

[PuzzleInfo("JSAbacusFramework.io", Topics.StringParsing, Difficulty.Medium)]
public sealed class Solution : SolutionBase
{
    public override object Run(int part)
    {
        var json = GetInputText();
        return part switch
        {
            1 => SumJsonOmitPropertyValue(json, value: null),
            2 => SumJsonOmitPropertyValue(json, value: "red"),
            _ => ProblemNotSolvedString
        };
    }
    
    private static int SumJsonOmitPropertyValue(string json, string? value)
    {
        var document = JsonDocument.Parse(json);
        var root = document.RootElement;

        return SumJson(root, omit: value);
    }

    private static int SumJson(JsonElement node, string? omit)
    {
        switch (node.ValueKind)
        {
            case JsonValueKind.Number:
                return node
                    .GetInt32();
            case JsonValueKind.Array:
                return node
                    .EnumerateArray()
                    .Sum(element => SumJson(element, omit));
            case JsonValueKind.Object when omit == null || !HasValue(node, omit):
                return node
                    .EnumerateObject()
                    .Sum(property => SumJson(property.Value, omit));
            case JsonValueKind.Undefined:
            case JsonValueKind.String:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
            default:
                return 0;
        }
    }

    private static bool HasValue(JsonElement node, string value)
    {
        return node
            .EnumerateObject()
            .Any(prop => prop.Value.ValueKind == JsonValueKind.String && prop.Value.GetString() == value);
    }
}