using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Rca;

public sealed record RcaIncidentLinkResponse
{
    public long AndonAlertId { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public string ExternalRcaIncidentId { get; init; } = string.Empty;

    public string RcaStatus { get; init; } = string.Empty;

    public DateTime? RcaCreatedUtc { get; init; }

    public string AlertTitle { get; init; } = string.Empty;

    public OperationalSeverity Severity { get; init; }

    public Guid? ExternalProjectId { get; init; }

    public Guid? ExternalTaskId { get; init; }

    public string SourceSystem { get; init; } = string.Empty;

    public static RcaIncidentLinkResponse FromAlert(AndonAlert alert)
    {
        return new RcaIncidentLinkResponse
        {
            AndonAlertId = alert.Id,
            TenantId = alert.TenantId,
            ExternalRcaIncidentId = alert.ExternalRcaIncidentId,
            RcaStatus = alert.RcaStatus,
            RcaCreatedUtc = alert.RcaCreatedUtc,
            AlertTitle = alert.Title,
            Severity = alert.Severity,
            ExternalProjectId = alert.ExternalProjectId,
            ExternalTaskId = alert.ExternalTaskId,
            SourceSystem = alert.SourceSystem
        };
    }
}
