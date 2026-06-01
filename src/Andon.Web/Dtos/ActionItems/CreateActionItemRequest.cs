using Andon.Web.Models.Shared;
using System.Text.Json;

namespace Andon.Web.Dtos.ActionItems;

public sealed record CreateActionItemRequest
{
    public string TenantId { get; init; } = string.Empty;

    public long? RelatedAndonAlertId { get; init; }

    public string ExternalRcaIncidentId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid? ExternalTaskId { get; init; }

    public string SourceSystem { get; init; } = string.Empty;

    public JsonElement? TaskSnapshot { get; init; }

    public JsonElement? ContextSnapshot { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public OperationalSeverity Priority { get; init; } = OperationalSeverity.Medium;

    public string AssignedPrincipalType { get; init; } = string.Empty;

    public string AssignedPrincipalId { get; init; } = string.Empty;

    public string AssignedDisplayName { get; init; } = string.Empty;

    public DateOnly? DueDate { get; init; }

    public string CreatedByPrincipalType { get; init; } = string.Empty;

    public string CreatedByPrincipalId { get; init; } = string.Empty;
}
