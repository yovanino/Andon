using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Rca;
using Andon.Web.Services.Rca;
using Microsoft.AspNetCore.Mvc;

namespace Andon.Web.Controllers.Api;

[ApiController]
[Route("api/v1/rca/incidents")]
public sealed class RcaIncidentsController(IRcaIntegrationService rcaIntegrationService) : ControllerBase
{
    [HttpPost("from-alert")]
    public async Task<IActionResult> RequestFromAlert(
        [FromBody] RequestRcaIncidentFromAlertRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (alert, errors) = await rcaIntegrationService.RequestIncidentFromAlertAsync(request, cancellationToken);

        if (errors.Count > 0 || alert is null)
        {
            return BadRequest(ApiResult<RcaIncidentLinkResponse>.Fail(
                errors,
                "RCA incident request was not registered.",
                correlationId));
        }

        return Ok(ApiResult<RcaIncidentLinkResponse>.Ok(
            RcaIncidentLinkResponse.FromAlert(alert),
            "RCA incident request registered.",
            correlationId));
    }

    [HttpPost("link")]
    public async Task<IActionResult> Link(
        [FromBody] LinkRcaIncidentRequest request,
        CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;
        var (alert, errors) = await rcaIntegrationService.LinkIncidentAsync(request, cancellationToken);

        if (errors.Count > 0 || alert is null)
        {
            return BadRequest(ApiResult<RcaIncidentLinkResponse>.Fail(
                errors,
                "RCA incident was not linked.",
                correlationId));
        }

        return Ok(ApiResult<RcaIncidentLinkResponse>.Ok(
            RcaIncidentLinkResponse.FromAlert(alert),
            "RCA incident linked.",
            correlationId));
    }
}
