using Andon.Web.Dtos.ActionItems;
using Andon.Web.Dtos.Common;
using Andon.Web.Models.Domain;

namespace Andon.Web.Services.ActionItems;

public interface IActionItemService
{
    Task<(ActionItem? ActionItem, IReadOnlyList<ApiError> Errors)> CreateAsync(
        CreateActionItemRequest request,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<ActionItem> ActionItems, IReadOnlyList<ApiError> Errors)> ListAsync(
        ListActionItemsRequest request,
        CancellationToken cancellationToken = default);

    Task<(ActionItem? ActionItem, IReadOnlyList<ApiError> Errors)> UpdateStatusAsync(
        long id,
        UpdateActionItemStatusRequest request,
        CancellationToken cancellationToken = default);
}
