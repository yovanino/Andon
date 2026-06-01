using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Gantt;

namespace Andon.Web.Services.Gantt;

public interface IGanttIntegrationService
{
    Task<(GanttTaskEventResponse? Response, IReadOnlyList<ApiError> Errors)> CreateTaskEventAsync(
        CreateGanttTaskEventRequest request,
        CancellationToken cancellationToken = default);

    Task<(GanttTaskStatusResponse? Response, IReadOnlyList<ApiError> Errors)> GetTaskStatusAsync(
        Guid externalTaskId,
        GanttTaskStatusRequest request,
        CancellationToken cancellationToken = default);
}
