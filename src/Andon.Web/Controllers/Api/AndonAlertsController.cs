using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Common;
using Andon.Web.Services.Andon;
using Microsoft.AspNetCore.Mvc;

namespace Andon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/andon/alerts")]
public sealed class AndonAlertsController(IAndonAlertService andonAlertService) : ControllerBase
{
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
}
