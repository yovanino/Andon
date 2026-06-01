using Andon.Web.Models.Shared;
using System.Text.Json;

namespace Andon.Web.Dtos.Events;

public sealed class CreateOperationalEventRequest
{
    public string TenantId { get; set; } = string.Empty;

    public EventSourceType Source { get; set; } = EventSourceType.ExternalIntegration;

    public OperationalEventType EventType { get; set; } = OperationalEventType.IntegrationSignal;

    public OperationalSeverity Severity { get; set; } = OperationalSeverity.Medium;

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? OccurredUtc { get; set; }

    public string MachineId { get; set; } = string.Empty;

    public string MachineName { get; set; } = string.Empty;

    public string LineCode { get; set; } = string.Empty;

    public string WorkOrderId { get; set; } = string.Empty;

    public Guid? ExternalProjectId { get; set; }

    public Guid? ExternalTaskId { get; set; }

    public string ExternalEventId { get; set; } = string.Empty;

    public string SourceSystem { get; set; } = string.Empty;

    public JsonElement? Payload { get; set; }

    public JsonElement? TaskSnapshot { get; set; }

    public JsonElement? ContextSnapshot { get; set; }

    public string CreatedByPrincipalType { get; set; } = "system";

    public string CreatedByPrincipalId { get; set; } = string.Empty;
}
