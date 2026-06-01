using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Andon;

public sealed record AndonAlertResponse
{
    public long Id { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public long? OperationalEventId { get; init; }

    public AndonAlertStatus Status { get; init; }

    public OperationalSeverity Severity { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string MachineId { get; init; } = string.Empty;

    public string MachineName { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public string WorkOrderId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid? ExternalTaskId { get; init; }

    public string SourceSystem { get; init; } = string.Empty;

    public string ResponsiblePrincipalType { get; init; } = string.Empty;

    public string ResponsiblePrincipalId { get; init; } = string.Empty;

    public string ResponsibleDisplayName { get; init; } = string.Empty;

    public DateTime OpenedUtc { get; init; }

    public DateTime CreatedUtc { get; init; }

    public DateTime UpdatedUtc { get; init; }

    public static AndonAlertResponse FromEntity(AndonAlert item)
    {
        return new AndonAlertResponse
        {
            Id = item.Id,
            TenantId = item.TenantId,
            OperationalEventId = item.OperationalEventId,
            Status = item.Status,
            Severity = item.Severity,
            Title = item.Title,
            Description = item.Description,
            MachineId = item.MachineId,
            MachineName = item.MachineName,
            LineCode = item.LineCode,
            WorkOrderId = item.WorkOrderId,
            ExternalProjectId = item.ExternalProjectId,
            ExternalTaskId = item.ExternalTaskId,
            SourceSystem = item.SourceSystem,
            ResponsiblePrincipalType = item.ResponsiblePrincipalType,
            ResponsiblePrincipalId = item.ResponsiblePrincipalId,
            ResponsibleDisplayName = item.ResponsibleDisplayName,
            OpenedUtc = item.OpenedUtc,
            CreatedUtc = item.CreatedUtc,
            UpdatedUtc = item.UpdatedUtc
        };
    }
}
