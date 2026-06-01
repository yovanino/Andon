using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Events;

public sealed record LiveOperationalEventsRequest
{
    public string TenantId { get; init; } = string.Empty;

    public DateTime? FromUtc { get; init; }

    public DateTime? ToUtc { get; init; }

    public OperationalSeverity? Severity { get; init; }

    public EventSourceType? Source { get; init; }

    public OperationalEventType? EventType { get; init; }

    public string MachineId { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public Guid? ExternalTaskId { get; init; }

    public int? Limit { get; init; }
}
