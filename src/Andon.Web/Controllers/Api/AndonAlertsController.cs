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

    [HttpGet("{id:long}/comments")]
    public async Task<IActionResult> Comments(
        long id,
        [FromQuery] ListAndonCommentsRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (comments, errors) = await andonAlertService.GetCommentsAsync(id, request, cancellationToken);

        if (errors.Count > 0)
        {
            return BadRequest(ApiResult<IReadOnlyList<AndonCommentResponse>>.Fail(
                errors,
                "Andon alert comments were not retrieved.",
                correlationId));
        }

        var response = comments
            .Select(AndonCommentResponse.FromEntity)
            .ToList();

        return Ok(ApiResult<IReadOnlyList<AndonCommentResponse>>.Ok(
            response,
            "Andon alert comments retrieved.",
            correlationId));
    }

    [HttpPost("{id:long}/comments")]
    public async Task<IActionResult> AddComment(
        long id,
        [FromBody] AddAndonCommentRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (comment, errors) = await andonAlertService.AddCommentAsync(id, request, cancellationToken);

        if (errors.Count > 0 || comment is null)
        {
            return BadRequest(ApiResult<AndonCommentResponse>.Fail(
                errors,
                "Andon alert comment was not created.",
                correlationId));
        }

        var response = AndonCommentResponse.FromEntity(comment);

        return Created(
            $"/api/v1/andon/alerts/{id}/comments/{response.Id}",
            ApiResult<AndonCommentResponse>.Ok(response, "Andon alert comment created.", correlationId));
    }
}
