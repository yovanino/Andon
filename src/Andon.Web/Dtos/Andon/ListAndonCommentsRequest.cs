namespace Andon.Web.Dtos.Andon;

public sealed record ListAndonCommentsRequest
{
    public string TenantId { get; init; } = string.Empty;

    public int? Limit { get; init; }
}
