using Andon.Web.Models.Shared;

namespace Andon.Web.Dtos.ActionItems;

public sealed record UpdateActionItemStatusRequest
{
    public string TenantId { get; init; } = string.Empty;

    public ActionItemStatus Status { get; init; }

    public string ClosureComment { get; init; } = string.Empty;
}
