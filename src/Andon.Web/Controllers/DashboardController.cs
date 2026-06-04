using Andon.Web.Data;
using Andon.Web.Models.Shared;
using Andon.Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Andon.Web.Controllers;

public sealed class DashboardController(AppDbContext context) : Controller
{
    private static readonly AndonAlertStatus[] OpenAlertStatuses =
    [
        AndonAlertStatus.New,
        AndonAlertStatus.Acknowledged,
        AndonAlertStatus.InProgress,
        AndonAlertStatus.Escalated
    ];

    private static readonly ActionItemStatus[] OpenActionStatuses =
    [
        ActionItemStatus.Open,
        ActionItemStatus.InProgress,
        ActionItemStatus.Blocked
    ];

    [HttpGet]
    public async Task<IActionResult> Index(string tenantId = "ternium-dev", CancellationToken cancellationToken = default)
    {
        var normalizedTenantId = string.IsNullOrWhiteSpace(tenantId) ? "ternium-dev" : tenantId.Trim();

        var openAlertsQuery = context.AndonAlerts
            .AsNoTracking()
            .Where(item => item.TenantId == normalizedTenantId && OpenAlertStatuses.Contains(item.Status));

        var openActionsQuery = context.ActionItems
            .AsNoTracking()
            .Where(item => item.TenantId == normalizedTenantId && OpenActionStatuses.Contains(item.Status));

        var alerts = await openAlertsQuery
            .OrderByDescending(item => item.Severity)
            .ThenBy(item => item.OpenedUtc)
            .Take(12)
            .Select(item => new DashboardAlertItem
            {
                Id = item.Id,
                Status = item.Status,
                Severity = item.Severity,
                Title = item.Title,
                MachineId = item.MachineId,
                LineCode = item.LineCode,
                ResponsibleDisplayName = item.ResponsibleDisplayName,
                OpenedUtc = item.OpenedUtc,
                RcaStatus = item.RcaStatus
            })
            .ToListAsync(cancellationToken);

        var events = await context.OperationalEvents
            .AsNoTracking()
            .Where(item => item.TenantId == normalizedTenantId)
            .OrderByDescending(item => item.OccurredUtc)
            .Take(12)
            .Select(item => new DashboardEventItem
            {
                Id = item.Id,
                Source = item.Source,
                EventType = item.EventType,
                Severity = item.Severity,
                Title = item.Title,
                MachineId = item.MachineId,
                OccurredUtc = item.OccurredUtc
            })
            .ToListAsync(cancellationToken);

        var actionItems = await openActionsQuery
            .OrderBy(item => item.Status == ActionItemStatus.Blocked ? 0 : 1)
            .ThenBy(item => item.DueDate ?? DateOnly.MaxValue)
            .ThenByDescending(item => item.Priority)
            .Take(12)
            .Select(item => new DashboardActionItem
            {
                Id = item.Id,
                Status = item.Status,
                Priority = item.Priority,
                Title = item.Title,
                AssignedDisplayName = item.AssignedDisplayName,
                DueDate = item.DueDate
            })
            .ToListAsync(cancellationToken);

        var model = new DashboardViewModel
        {
            TenantId = normalizedTenantId,
            OpenAlerts = await openAlertsQuery.CountAsync(cancellationToken),
            CriticalAlerts = await openAlertsQuery.CountAsync(item => item.Severity == OperationalSeverity.Critical, cancellationToken),
            OpenActionItems = await openActionsQuery.CountAsync(cancellationToken),
            RcaPending = await openAlertsQuery.CountAsync(item => item.RcaStatus == string.Empty || item.RcaStatus == "Requested", cancellationToken),
            Alerts = alerts,
            Events = events,
            ActionItems = actionItems
        };

        return View(model);
    }
}
