using Andon.Web.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Andon.Web.Models.Domain;

public class AndonAlert
{
    public long Id { get; set; }

    [MaxLength(64)]
    public string TenantId { get; set; } = string.Empty;

    public long? OperationalEventId { get; set; }

    public AndonAlertStatus Status { get; set; } = AndonAlertStatus.New;

    public OperationalSeverity Severity { get; set; } = OperationalSeverity.Medium;

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(80)]
    public string MachineId { get; set; } = string.Empty;

    [MaxLength(80)]
    public string MachineName { get; set; } = string.Empty;

    [MaxLength(80)]
    public string LineCode { get; set; } = string.Empty;

    [MaxLength(80)]
    public string WorkOrderId { get; set; } = string.Empty;

    public Guid? ExternalProjectId { get; set; }

    public Guid? ExternalTaskId { get; set; }

    [MaxLength(40)]
    public string SourceSystem { get; set; } = string.Empty;

    [MaxLength(24)]
    public string ResponsiblePrincipalType { get; set; } = string.Empty;

    [MaxLength(80)]
    public string ResponsiblePrincipalId { get; set; } = string.Empty;

    [MaxLength(160)]
    public string ResponsibleDisplayName { get; set; } = string.Empty;

    public DateTime OpenedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? AcknowledgedUtc { get; set; }

    public DateTime? AssignedUtc { get; set; }

    public DateTime? EscalatedUtc { get; set; }

    public DateTime? ResolvedUtc { get; set; }

    [MaxLength(2000)]
    public string ResolutionComment { get; set; } = string.Empty;

    [MaxLength(24)]
    public string CreatedByPrincipalType { get; set; } = "system";

    [MaxLength(80)]
    public string CreatedByPrincipalId { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;

    public OperationalEvent? OperationalEvent { get; set; }

    public ICollection<AndonComment> Comments { get; set; } = [];

    public ICollection<ActionItem> ActionItems { get; set; } = [];
}
