using Andon.Web.Models.Domain;

namespace Andon.Web.Dtos.Andon;

public sealed record AndonCommentResponse
{
    public long Id { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public long AndonAlertId { get; init; }

    public string Comment { get; init; } = string.Empty;

    public string CreatedByPrincipalType { get; init; } = string.Empty;

    public string CreatedByPrincipalId { get; init; } = string.Empty;

    public string CreatedByDisplayName { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }

    public static AndonCommentResponse FromEntity(AndonComment item)
    {
        return new AndonCommentResponse
        {
            Id = item.Id,
            TenantId = item.TenantId,
            AndonAlertId = item.AndonAlertId,
            Comment = item.Comment,
            CreatedByPrincipalType = item.CreatedByPrincipalType,
            CreatedByPrincipalId = item.CreatedByPrincipalId,
            CreatedByDisplayName = item.CreatedByDisplayName,
            CreatedUtc = item.CreatedUtc
        };
    }
}
