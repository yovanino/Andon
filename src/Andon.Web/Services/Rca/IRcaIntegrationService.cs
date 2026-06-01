using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Rca;
using Andon.Web.Models.Domain;

namespace Andon.Web.Services.Rca;

public interface IRcaIntegrationService
{
    Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> RequestIncidentFromAlertAsync(
        RequestRcaIncidentFromAlertRequest request,
        CancellationToken cancellationToken = default);

    Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> LinkIncidentAsync(
        LinkRcaIncidentRequest request,
        CancellationToken cancellationToken = default);
}
