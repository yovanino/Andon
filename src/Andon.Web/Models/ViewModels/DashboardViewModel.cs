using Andon.Web.Models.Shared;

namespace Andon.Web.Models.ViewModels;

public sealed record DashboardViewModel
{
    public string TenantId { get; init; } = string.Empty;

    public int OpenAlerts { get; init; }

    public int CriticalAlerts { get; init; }

    public int OpenActionItems { get; init; }

    public int RcaPending { get; init; }

    public IReadOnlyList<DashboardAlertItem> Alerts { get; init; } = [];

    public IReadOnlyList<DashboardEventItem> Events { get; init; } = [];

    public IReadOnlyList<DashboardActionItem> ActionItems { get; init; } = [];
}

public sealed record DashboardAlertItem
{
    public long Id { get; init; }

    public AndonAlertStatus Status { get; init; }

    public OperationalSeverity Severity { get; init; }

    public string Title { get; init; } = string.Empty;

    public string MachineId { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public string ResponsibleDisplayName { get; init; } = string.Empty;

    public DateTime OpenedUtc { get; init; }

    public string RcaStatus { get; init; } = string.Empty;
}

public sealed record DashboardEventItem
{
    public long Id { get; init; }

    public EventSourceType Source { get; init; }

    public OperationalEventType EventType { get; init; }

    public OperationalSeverity Severity { get; init; }

    public string Title { get; init; } = string.Empty;

    public string MachineId { get; init; } = string.Empty;

    public DateTime OccurredUtc { get; init; }
}

public sealed record DashboardActionItem
{
    public long Id { get; init; }

    public ActionItemStatus Status { get; init; }

    public OperationalSeverity Priority { get; init; }

    public string Title { get; init; } = string.Empty;

    public string AssignedDisplayName { get; init; } = string.Empty;

    public DateOnly? DueDate { get; init; }
}
