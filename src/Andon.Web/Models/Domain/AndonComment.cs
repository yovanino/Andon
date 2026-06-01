using System.ComponentModel.DataAnnotations;

namespace Andon.Web.Models.Domain;

public class AndonComment
{
    public long Id { get; set; }

    [MaxLength(64)]
    public string TenantId { get; set; } = string.Empty;

    public long AndonAlertId { get; set; }

    [MaxLength(2000)]
    public string Comment { get; set; } = string.Empty;

    [MaxLength(24)]
    public string CreatedByPrincipalType { get; set; } = "system";

    [MaxLength(80)]
    public string CreatedByPrincipalId { get; set; } = string.Empty;

    [MaxLength(160)]
    public string CreatedByDisplayName { get; set; } = string.Empty;

    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

    public AndonAlert? AndonAlert { get; set; }
}
