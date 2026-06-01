using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Common;
using Andon.Web.Models.Domain;

namespace Andon.Web.Services.Andon;

public interface IAndonAlertService
{
    Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> CreateAsync(
        CreateAndonAlertRequest request,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<AndonAlert> Alerts, IReadOnlyList<ApiError> Errors)> GetLiveAsync(
        LiveAndonAlertsRequest request,
        CancellationToken cancellationToken = default);

    Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> TransitionAsync(
        long id,
        TransitionAndonAlertRequest request,
        CancellationToken cancellationToken = default);
}
