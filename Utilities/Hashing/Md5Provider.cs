using System.Security.Cryptography;
using System.Text;

namespace Utilities.Hashing;

public sealed class Md5Provider : IDisposable
{
    private readonly MD5 _hashProvider = MD5.Create();
    
    public string GetHashHex(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = _hashProvider.ComputeHash(bytes);

        return Convert.ToHexString(hash).ToLower();
    }

    public void Dispose()
    {
        _hashProvider.Dispose();
    }
}