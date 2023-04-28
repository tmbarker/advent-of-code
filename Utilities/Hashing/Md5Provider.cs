using System.Security.Cryptography;
using System.Text;

namespace Utilities.Hashing;

public class Md5Provider
{
    private readonly MD5 _hashProvider;

    public Md5Provider()
    {
        _hashProvider = MD5.Create();
    }
    
    ~Md5Provider()
    {
        _hashProvider.Dispose();
    }

    public string GetHashHex(string input)
    {
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = _hashProvider.ComputeHash(bytes);
        
        return Convert.ToHexString(hash).ToLower();
    }
}