using Problems.Y2016.Common;

namespace Problems.Y2016.D14;

public class HashSequence
{
    private readonly string _salt;
    private readonly int _stretches;
    private readonly List<string> _hashes = new();
    private readonly Md5Provider _md5Provider = new();
    
    public string this[int i] => GetHashInternal(i);

    public HashSequence(string salt, int stretches, int count)
    {
        _salt = salt;
        _stretches = stretches;
        EnsureContains(count);
    }
    
    private string GetHashInternal(int i)
    {
        EnsureContains(count: i + 1);
        return _hashes[i];
    }

    private void EnsureContains(int count)
    {
        while(_hashes.Count < count)
        {
            var input = $"{_salt}{_hashes.Count}";
            var hash = _md5Provider.GetHashHexString(input);

            for (var i = 0; i < _stretches; i++)
            {
                hash = _md5Provider.GetHashHexString(hash);
            }
            
            _hashes.Add(hash);
        }
    }
}