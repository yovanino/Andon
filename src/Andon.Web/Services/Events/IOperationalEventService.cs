using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Events;
using Andon.Web.Models.Domain;

namespace Andon.Web.Services.Events;

public interface IOperationalEventService
{
    Task<(OperationalEvent? Event, IReadOnlyList<ApiError> Errors)> CreateAsync(
        CreateOperationalEventRequest request,
        CancellationToken cancellationToken = default);

    Task<(IReadOnlyList<OperationalEvent> Events, IReadOnlyList<ApiError> Errors)> GetLiveAsync(
        LiveOperationalEventsRequest request,
        CancellationToken cancellationToken = default);
}
