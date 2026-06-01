using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Gantt;
using Andon.Web.Services.Gantt;
using Microsoft.AspNetCore.Mvc;

namespace Andon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/gantt")]
public sealed class GanttIntegrationController(IGanttIntegrationService ganttIntegrationService) : ControllerBase
{
    [HttpGet("tasks/{externalTaskId:guid}/status")]
    public async Task<IActionResult> TaskStatus(
        Guid externalTaskId,
        [FromQuery] GanttTaskStatusRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (response, errors) = await ganttIntegrationService.GetTaskStatusAsync(externalTaskId, request, cancellationToken);

        if (errors.Count > 0 || response is null)
        {
            return BadRequest(ApiResult<GanttTaskStatusResponse>.Fail(
                errors,
                "Gantt task status was not retrieved.",
                correlationId));
        }

        return Ok(ApiResult<GanttTaskStatusResponse>.Ok(
            response,
            "Gantt task status retrieved.",
            correlationId));
    }

    [HttpPost("task-events")]
    public async Task<IActionResult> CreateTaskEvent(
        [FromBody] CreateGanttTaskEventRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (response, errors) = await ganttIntegrationService.CreateTaskEventAsync(request, cancellationToken);

        if (errors.Count > 0 || response is null)
        {
            return BadRequest(ApiResult<GanttTaskEventResponse>.Fail(
                errors,
                "Gantt task event was not registered.",
                correlationId));
        }

        return Created(
            $"/api/v1/events/{response.OperationalEvent.Id}",
            ApiResult<GanttTaskEventResponse>.Ok(response, "Gantt task event registered.", correlationId));
    }
}
