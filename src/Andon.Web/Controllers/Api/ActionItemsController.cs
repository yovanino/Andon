using Andon.Web.Dtos.ActionItems;
using Andon.Web.Dtos.Common;
using Andon.Web.Services.ActionItems;
using Microsoft.AspNetCore.Mvc;

namespace Andon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/action-items")]
public sealed class ActionItemsController(IActionItemService actionItemService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] ListActionItemsRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (actionItems, errors) = await actionItemService.ListAsync(request, cancellationToken);

        if (errors.Count > 0)
        {
            return BadRequest(ApiResult<IReadOnlyList<ActionItemResponse>>.Fail(
                errors,
                "Action items were not retrieved.",
                correlationId));
        }

        var response = actionItems
            .Select(ActionItemResponse.FromEntity)
            .ToList();

        return Ok(ApiResult<IReadOnlyList<ActionItemResponse>>.Ok(
            response,
            "Action items retrieved.",
            correlationId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateActionItemRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (actionItem, errors) = await actionItemService.CreateAsync(request, cancellationToken);

        if (errors.Count > 0 || actionItem is null)
        {
            return BadRequest(ApiResult<ActionItemResponse>.Fail(
                errors,
                "Action item was not created.",
                correlationId));
        }

        var response = ActionItemResponse.FromEntity(actionItem);

        return Created(
            $"/api/v1/action-items/{response.Id}",
            ApiResult<ActionItemResponse>.Ok(response, "Action item created.", correlationId));
    }

    [HttpPost("{id:long}/status")]
    public async Task<IActionResult> UpdateStatus(
        long id,
        [FromBody] UpdateActionItemStatusRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (actionItem, errors) = await actionItemService.UpdateStatusAsync(id, request, cancellationToken);

        if (errors.Count > 0 || actionItem is null)
        {
            return BadRequest(ApiResult<ActionItemResponse>.Fail(
                errors,
                "Action item status was not updated.",
                correlationId));
        }

        return Ok(ApiResult<ActionItemResponse>.Ok(
            ActionItemResponse.FromEntity(actionItem),
            "Action item status updated.",
            correlationId));
    }
}
