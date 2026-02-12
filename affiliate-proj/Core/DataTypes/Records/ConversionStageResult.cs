using affiliate_proj.Core.Entities;

namespace affiliate_proj.Core.DataTypes.Records;

public sealed record ConversionStageResult
{
    public bool IsSuccess { get; init; } 
    public bool IsIgnored { get; init; }
    public bool IsDuplicate { get; init; }
    public Guid ConversionId { get; init; }
    public Conversion? Conversion { get; init; }
}