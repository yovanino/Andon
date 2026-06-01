using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.ActionItems;

public sealed record ListActionItemsRequest
{
    public string TenantId { get; init; } = string.Empty;

    public ActionItemStatus? Status { get; init; }

    public OperationalSeverity? Priority { get; init; }

    public long? RelatedAndonAlertId { get; init; }

    public string ExternalRcaIncidentId { get; init; } = string.Empty;

    public Guid? ExternalTaskId { get; init; }

    public string AssignedPrincipalType { get; init; } = string.Empty;

    public string AssignedPrincipalId { get; init; } = string.Empty;

    public DateOnly? DueFrom { get; init; }

    public DateOnly? DueTo { get; init; }

    public int? Limit { get; init; }
}
