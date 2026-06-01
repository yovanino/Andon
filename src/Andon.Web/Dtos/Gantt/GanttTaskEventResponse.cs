using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Events;

namespace Andon.Web.Dtos.Gantt;

public sealed record GanttTaskEventResponse
{
    public OperationalEventResponse OperationalEvent { get; init; } = new();

    public AndonAlertResponse? AndonAlert { get; init; }
}
