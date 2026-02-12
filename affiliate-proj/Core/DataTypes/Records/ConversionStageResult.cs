using affiliate_proj.Core.Entities;

namespace affiliate_proj.Core.DataTypes.Records;

public sealed record ConversionStageResult
{
    public ConversionStagingStatus Status { get; init; }
    public Guid ConversionId { get; init; }
    public Conversion? Conversion { get; init; }
}

public enum ConversionStagingStatus
{
    Created,
    Duplicate,
    Ignored
}