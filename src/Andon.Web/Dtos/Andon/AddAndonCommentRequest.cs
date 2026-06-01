namespace Andon.Web.Dtos.Andon;

public sealed record AddAndonCommentRequest
{
    public string TenantId { get; init; } = string.Empty;

    public string Comment { get; init; } = string.Empty;

    public string CreatedByPrincipalType { get; init; } = string.Empty;

    public string CreatedByPrincipalId { get; init; } = string.Empty;

    public string CreatedByDisplayName { get; init; } = string.Empty;
}
