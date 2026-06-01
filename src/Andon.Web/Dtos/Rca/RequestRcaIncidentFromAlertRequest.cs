namespace Andon.Web.Dtos.Rca;

public sealed record RequestRcaIncidentFromAlertRequest
{
    public string TenantId { get; init; } = string.Empty;

    public long AndonAlertId { get; init; }

    public string ExternalRcaIncidentId { get; init; } = string.Empty;

    public string RcaStatus { get; init; } = string.Empty;

    public string RequestedByPrincipalType { get; init; } = string.Empty;

    public string RequestedByPrincipalId { get; init; } = string.Empty;
}
