using Supabase.Gotrue;

namespace affiliate_proj.Accessors.DatabaseAccessors;

public class SupabaseAccessor
{
    private readonly string _url;
    private readonly string _anonPublicKey;
    private readonly string _serviceRoleKey;
    private readonly Supabase.Client _client;

    public SupabaseAccessor(string url,  string anonPublicKey, string serviceRoleKey)
    {
        _url = url;
        _anonPublicKey = anonPublicKey;
        _serviceRoleKey = serviceRoleKey;
    }

    public SupabaseAccessor(Supabase.Client client)
    {
        _client = client;
    }
    
    public void ShowKeys()
    {
        Console.WriteLine(_url);
        Console.WriteLine(_anonPublicKey);
        Console.WriteLine(_serviceRoleKey);
    }

    public User? GetCurrentUser()
    {
        return _client.Auth.CurrentUser; 
    }
}