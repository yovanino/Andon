using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Andon;

public sealed record LiveAndonAlertsRequest
{
    public string TenantId { get; init; } = string.Empty;

    public AndonAlertStatus? Status { get; init; }

    public OperationalSeverity? Severity { get; init; }

    public string MachineId { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public Guid? ExternalTaskId { get; init; }

    public string ResponsiblePrincipalType { get; init; } = string.Empty;

    public string ResponsiblePrincipalId { get; init; } = string.Empty;

    public DateTime? OpenedFromUtc { get; init; }

    public DateTime? OpenedToUtc { get; init; }

    public int? Limit { get; init; }
}
