using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Common;
using Andon.Web.Services.Andon;
using Microsoft.AspNetCore.Mvc;

namespace Andon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/andon/alerts")]
public sealed class AndonAlertsController(IAndonAlertService andonAlertService) : ControllerBase
{
    [HttpGet("live")]
    public async Task<IActionResult> Live(
        [FromQuery] LiveAndonAlertsRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (alerts, errors) = await andonAlertService.GetLiveAsync(request, cancellationToken);

        if (errors.Count > 0)
        {
            return BadRequest(ApiResult<IReadOnlyList<AndonAlertResponse>>.Fail(
                errors,
                "Live Andon alerts were not retrieved.",
                correlationId));
        }

        var response = alerts
            .Select(AndonAlertResponse.FromEntity)
            .ToList();

        return Ok(ApiResult<IReadOnlyList<AndonAlertResponse>>.Ok(
            response,
            "Live Andon alerts retrieved.",
            correlationId));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateAndonAlertRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (alert, errors) = await andonAlertService.CreateAsync(request, cancellationToken);

        if (errors.Count > 0 || alert is null)
        {
            return BadRequest(ApiResult<AndonAlertResponse>.Fail(
                errors,
                "Andon alert was not created.",
                correlationId));
        }

        var response = AndonAlertResponse.FromEntity(alert);

        return Created(
            $"/api/v1/andon/alerts/{response.Id}",
            ApiResult<AndonAlertResponse>.Ok(response, "Andon alert created.", correlationId));
    }

    [HttpPost("{id:long}/transition")]
    public async Task<IActionResult> Transition(
        long id,
        [FromBody] TransitionAndonAlertRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (alert, errors) = await andonAlertService.TransitionAsync(id, request, cancellationToken);

        if (errors.Count > 0 || alert is null)
        {
            return BadRequest(ApiResult<AndonAlertResponse>.Fail(
                errors,
                "Andon alert was not transitioned.",
                correlationId));
        }

        return Ok(ApiResult<AndonAlertResponse>.Ok(
            AndonAlertResponse.FromEntity(alert),
            "Andon alert transitioned.",
            correlationId));
    }
}
