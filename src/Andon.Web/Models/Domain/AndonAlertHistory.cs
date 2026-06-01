using Andon.Web.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace Andon.Web.Models.Domain;

public class AndonAlertHistory
{
    public long Id { get; set; }

    [MaxLength(64)]
    public string TenantId { get; set; } = string.Empty;

    public long AndonAlertId { get; set; }

    public AndonTransitionAction Action { get; set; }

    public AndonAlertStatus? FromStatus { get; set; }

    public AndonAlertStatus? ToStatus { get; set; }

    [MaxLength(2000)]
    public string Comment { get; set; } = string.Empty;

    [MaxLength(24)]
    public string ActorPrincipalType { get; set; } = "system";

    [MaxLength(80)]
    public string ActorPrincipalId { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public AndonAlert? AndonAlert { get; set; }
}
