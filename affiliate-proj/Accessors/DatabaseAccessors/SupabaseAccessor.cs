namespace affiliate_proj.Accessors.DatabaseAccessors;

public class SupabaseAccessor
{
    private readonly string _url;
    private readonly string _anonPublicKey;
    private readonly string _serviceRoleKey;

    public SupabaseAccessor(string url,  string anonPublicKey, string serviceRoleKey)
    {
        _url = url;
        _anonPublicKey = anonPublicKey;
        _serviceRoleKey = serviceRoleKey;
    }
    
    public void ShowKeys()
    {
        Console.WriteLine(_url);
        Console.WriteLine(_anonPublicKey);
        Console.WriteLine(_serviceRoleKey);
    }
}