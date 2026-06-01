using Andon.Web.Models.Shared;
using System.Text.Json;

namespace Andon.Web.Dtos.Gantt;

public sealed record CreateGanttTaskEventRequest
{
    public string TenantId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid ExternalTaskId { get; init; }

    public string ExternalEventId { get; init; } = string.Empty;

    public OperationalEventType EventType { get; init; } = OperationalEventType.DelayDetected;

    public OperationalSeverity Severity { get; init; } = OperationalSeverity.Medium;

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public DateTime? OccurredUtc { get; init; }

    public string MachineId { get; init; } = string.Empty;

    public string MachineName { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public string WorkOrderId { get; init; } = string.Empty;

    public bool CreateAndonAlert { get; init; }

    public JsonElement? Payload { get; init; }

    public JsonElement? TaskSnapshot { get; init; }

    public JsonElement? ContextSnapshot { get; init; }

    public string CreatedByPrincipalType { get; init; } = string.Empty;

    public string CreatedByPrincipalId { get; init; } = string.Empty;
}
