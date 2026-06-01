namespace Andon.Web.Dtos.Rca;

public sealed record LinkRcaIncidentRequest
{
    public string TenantId { get; init; } = string.Empty;

    public long AndonAlertId { get; init; }

    public string ExternalRcaIncidentId { get; init; } = string.Empty;

    public string RcaStatus { get; init; } = string.Empty;
}
