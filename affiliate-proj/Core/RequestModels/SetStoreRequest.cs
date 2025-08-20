namespace affiliate_proj.Core.RequestModels;

public class SetStoreRequest
{
    public Guid StoreId { get; set; }
    public string? StoreName { get; set; }
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }
}