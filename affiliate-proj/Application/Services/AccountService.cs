using affiliate_proj.Accessors.DatabaseAccessors;

namespace affiliate_proj.Application.Services;

public class AccountService
{
    private readonly SupabaseAccessor _supabaseAccessor;

    public AccountService(SupabaseAccessor supabaseAccessor)
    {
        _supabaseAccessor = supabaseAccessor;
    }
}