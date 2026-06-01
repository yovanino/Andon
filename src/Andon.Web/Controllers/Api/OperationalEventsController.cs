using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Events;
using Andon.Web.Services.Events;
using Microsoft.AspNetCore.Mvc;

namespace Andon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/events")]
public sealed class OperationalEventsController(IOperationalEventService operationalEventService) : ControllerBase
{
    [HttpGet("live")]
    public async Task<IActionResult> Live(
        [FromQuery] LiveOperationalEventsRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (events, errors) = await operationalEventService.GetLiveAsync(request, cancellationToken);

        if (errors.Count > 0)
        {
            return BadRequest(ApiResult<IReadOnlyList<OperationalEventResponse>>.Fail(
                errors,
                "Live operational events were not retrieved.",
                correlationId));
        }

        var response = events
            .Select(OperationalEventResponse.FromEntity)
            .ToList();

        return Ok(ApiResult<IReadOnlyList<OperationalEventResponse>>.Ok(
            response,
            "Live operational events retrieved.",
            correlationId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOperationalEventRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (operationalEvent, errors) = await operationalEventService.CreateAsync(request, cancellationToken);

        if (errors.Count > 0 || operationalEvent is null)
        {
            return BadRequest(ApiResult<OperationalEventResponse>.Fail(
                errors,
                "Operational event was not created.",
                correlationId));
        }

        var response = OperationalEventResponse.FromEntity(operationalEvent);

        return Created(
            $"/api/v1/events/{response.Id}",
            ApiResult<OperationalEventResponse>.Ok(response, "Operational event created.", correlationId));
    }
}
