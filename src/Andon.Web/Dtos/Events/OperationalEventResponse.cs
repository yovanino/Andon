using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Events;

public sealed record OperationalEventResponse
{
    public long Id { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public EventSourceType Source { get; init; }

    public OperationalEventType EventType { get; init; }

    public OperationalSeverity Severity { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public DateTime OccurredUtc { get; init; }

    public string MachineId { get; init; } = string.Empty;

    public string MachineName { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public string WorkOrderId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid? ExternalTaskId { get; init; }

    public string ExternalEventId { get; init; } = string.Empty;

    public string SourceSystem { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }

    public static OperationalEventResponse FromEntity(OperationalEvent item)
    {
        return new OperationalEventResponse
        {
            Id = item.Id,
            TenantId = item.TenantId,
            Source = item.Source,
            EventType = item.EventType,
            Severity = item.Severity,
            Title = item.Title,
            Description = item.Description,
            OccurredUtc = item.OccurredUtc,
            MachineId = item.MachineId,
            MachineName = item.MachineName,
            LineCode = item.LineCode,
            WorkOrderId = item.WorkOrderId,
            ExternalProjectId = item.ExternalProjectId,
            ExternalTaskId = item.ExternalTaskId,
            ExternalEventId = item.ExternalEventId,
            SourceSystem = item.SourceSystem,
            CreatedUtc = item.CreatedUtc
        };
    }
}
