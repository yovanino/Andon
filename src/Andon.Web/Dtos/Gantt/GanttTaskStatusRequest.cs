namespace Andon.Web.Dtos.Gantt;

public sealed record GanttTaskStatusRequest
{
    public string TenantId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public DateTime? FromUtc { get; init; }

    public DateTime? ToUtc { get; init; }

    public int? Limit { get; init; }
}
