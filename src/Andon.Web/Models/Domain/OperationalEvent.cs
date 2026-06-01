using Andon.Web.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Andon.Web.Models.Domain;

public class OperationalEvent
{
    public long Id { get; set; }

    [MaxLength(64)]
    public string TenantId { get; set; } = string.Empty;

    public EventSourceType Source { get; set; }

    public OperationalEventType EventType { get; set; }

    public OperationalSeverity Severity { get; set; }

    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    public DateTime OccurredUtc { get; set; } = DateTime.UtcNow;

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

    [MaxLength(80)]
    public string ExternalEventId { get; set; } = string.Empty;

    [MaxLength(40)]
    public string SourceSystem { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = "{}";

    public string TaskSnapshotJson { get; set; } = "{}";

    public string ContextSnapshotJson { get; set; } = "{}";

    [MaxLength(24)]
    public string CreatedByPrincipalType { get; set; } = "system";

    [MaxLength(80)]
    public string CreatedByPrincipalId { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
