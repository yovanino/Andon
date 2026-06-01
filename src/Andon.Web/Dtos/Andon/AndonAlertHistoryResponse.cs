using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Andon;

public sealed record AndonAlertHistoryResponse
{
    public long Id { get; init; }

    public string TenantId { get; init; } = string.Empty;

    public long AndonAlertId { get; init; }

    public AndonTransitionAction Action { get; init; }

    public AndonAlertStatus? FromStatus { get; init; }

    public AndonAlertStatus? ToStatus { get; init; }

    public string Comment { get; init; } = string.Empty;

    public string ActorPrincipalType { get; init; } = string.Empty;

    public string ActorPrincipalId { get; init; } = string.Empty;

    public DateTime CreatedUtc { get; init; }

    public static AndonAlertHistoryResponse FromEntity(AndonAlertHistory item)
    {
        return new AndonAlertHistoryResponse
        {
            Id = item.Id,
            TenantId = item.TenantId,
            AndonAlertId = item.AndonAlertId,
            Action = item.Action,
            FromStatus = item.FromStatus,
            ToStatus = item.ToStatus,
            Comment = item.Comment,
            ActorPrincipalType = item.ActorPrincipalType,
            ActorPrincipalId = item.ActorPrincipalId,
            CreatedUtc = item.CreatedUtc
        };
    }
}
