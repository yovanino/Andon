using Andon.Web.Dtos.ActionItems;
using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Events;

namespace Andon.Web.Dtos.Gantt;

public sealed record GanttTaskStatusResponse
{
    public string TenantId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid ExternalTaskId { get; init; }

    public IReadOnlyList<OperationalEventResponse> Events { get; init; } = [];

    public IReadOnlyList<AndonAlertResponse> Alerts { get; init; } = [];

    public IReadOnlyList<ActionItemResponse> ActionItems { get; init; } = [];
}
