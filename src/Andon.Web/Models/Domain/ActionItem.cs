using Andon.Web.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Andon.Web.Models.Domain;

public class ActionItem
{
    public long Id { get; set; }

    [MaxLength(64)]
    public string TenantId { get; set; } = string.Empty;

    public long? RelatedAndonAlertId { get; set; }

    [MaxLength(80)]
    public string ExternalRcaIncidentId { get; set; } = string.Empty;

    public Guid? ExternalProjectId { get; set; }

    public Guid? ExternalTaskId { get; set; }

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public ActionItemStatus Status { get; set; } = ActionItemStatus.Open;

    public OperationalSeverity Priority { get; set; } = OperationalSeverity.Medium;

    [MaxLength(24)]
    public string AssignedPrincipalType { get; set; } = string.Empty;

    [MaxLength(80)]
    public string AssignedPrincipalId { get; set; } = string.Empty;

    [MaxLength(160)]
    public string AssignedDisplayName { get; set; } = string.Empty;

    public DateOnly? DueDate { get; set; }

    public DateTime? ClosedUtc { get; set; }

    [MaxLength(2000)]
    public string ClosureComment { get; set; } = string.Empty;

    [MaxLength(24)]
    public string CreatedByPrincipalType { get; set; } = "system";

    [MaxLength(80)]
    public string CreatedByPrincipalId { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public AndonAlert? RelatedAndonAlert { get; set; }
}
