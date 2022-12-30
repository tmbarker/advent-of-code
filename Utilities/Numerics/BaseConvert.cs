using System.Text;

namespace Utilities.Numerics;

public static class BaseConvert
{
    private const int BinaryRadix = 2;
    
    private static readonly Dictionary<char, string> HexCharToBinary = new() {
        { '0', "0000" },
        { '1', "0001" },
        { '2', "0010" },
        { '3', "0011" },
        { '4', "0100" },
        { '5', "0101" },
        { '6', "0110" },
        { '7', "0111" },
        { '8', "1000" },
        { '9', "1001" },
        { 'A', "1010" },
        { 'B', "1011" },
        { 'C', "1100" },
        { 'D', "1101" },
        { 'E', "1110" },
        { 'F', "1111" }
    };
    
    public static long BinToDec(string binary)
    {
        var value = 0L;
        var digits = binary.Length;
        
        for (var i = 0; i < digits; i++)
        {
            var p = (long)Math.Pow(BinaryRadix, i);
            var d = binary[digits - i - 1] - '0';
            value += d * p;
        }
        
        return value;
    }
    
    public static string HexToBin(string hex)
    {
        var sb = new StringBuilder();
        foreach (var c in hex.ToUpperInvariant())
        {
            sb.Append(HexCharToBinary[c]);
        }
        return sb.ToString();
    }
}