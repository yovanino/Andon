using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.Andon;

public sealed record TransitionAndonAlertRequest
{
    public string TenantId { get; init; } = string.Empty;

    public AndonTransitionAction Action { get; init; }

    public string ResponsiblePrincipalType { get; init; } = string.Empty;

    public string ResponsiblePrincipalId { get; init; } = string.Empty;

    public string ResponsibleDisplayName { get; init; } = string.Empty;

    public string Comment { get; init; } = string.Empty;

    public string ActorPrincipalType { get; init; } = string.Empty;

    public string ActorPrincipalId { get; init; } = string.Empty;
}
