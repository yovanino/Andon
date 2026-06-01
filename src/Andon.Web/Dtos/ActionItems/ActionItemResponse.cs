using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.ActionItems;

public sealed record ActionItemResponse
{
    public long Id { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public long? RelatedAndonAlertId { get; init; }

    public string ExternalRcaIncidentId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid? ExternalTaskId { get; init; }

    public string SourceSystem { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public ActionItemStatus Status { get; init; }

    public OperationalSeverity Priority { get; init; }

    public string AssignedPrincipalType { get; init; } = string.Empty;

    public string AssignedPrincipalId { get; init; } = string.Empty;

    public string AssignedDisplayName { get; init; } = string.Empty;

    public DateOnly? DueDate { get; init; }

    public DateTime? ClosedUtc { get; init; }

    public string ClosureComment { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }

    public DateTime UpdatedUtc { get; init; }

    public static ActionItemResponse FromEntity(ActionItem item)
    {
        return new ActionItemResponse
        {
            Id = item.Id,
            TenantId = item.TenantId,
            RelatedAndonAlertId = item.RelatedAndonAlertId,
            ExternalRcaIncidentId = item.ExternalRcaIncidentId,
            ExternalProjectId = item.ExternalProjectId,
            ExternalTaskId = item.ExternalTaskId,
            SourceSystem = item.SourceSystem,
            Title = item.Title,
            Description = item.Description,
            Status = item.Status,
            Priority = item.Priority,
            AssignedPrincipalType = item.AssignedPrincipalType,
            AssignedPrincipalId = item.AssignedPrincipalId,
            AssignedDisplayName = item.AssignedDisplayName,
            DueDate = item.DueDate,
            ClosedUtc = item.ClosedUtc,
            ClosureComment = item.ClosureComment,
            CreatedUtc = item.CreatedUtc,
            UpdatedUtc = item.UpdatedUtc
        };
    }
}
