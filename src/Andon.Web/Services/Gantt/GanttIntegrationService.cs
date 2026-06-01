using Andon.Web.Data;
using Andon.Web.Dtos.ActionItems;
using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Events;
using Andon.Web.Dtos.Gantt;
using Andon.Web.Models.Shared;
using Andon.Web.Services.Andon;
using Andon.Web.Services.Events;
using Microsoft.EntityFrameworkCore;

namespace Andon.Web.Services.Gantt;

public sealed class GanttIntegrationService(
    AppDbContext context,
    IOperationalEventService operationalEventService,
    IAndonAlertService andonAlertService) : IGanttIntegrationService
{
    public async Task<(GanttTaskEventResponse? Response, IReadOnlyList<ApiError> Errors)> CreateTaskEventAsync(
        CreateGanttTaskEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var eventRequest = new CreateOperationalEventRequest
        {
            TenantId = Normalize(request.TenantId),
            Source = EventSourceType.Gantt,
            EventType = request.EventType,
            Severity = request.Severity,
            Title = Normalize(request.Title),
            Description = Normalize(request.Description),
            OccurredUtc = request.OccurredUtc,
            MachineId = Normalize(request.MachineId),
            MachineName = Normalize(request.MachineName),
            LineCode = Normalize(request.LineCode),
            WorkOrderId = Normalize(request.WorkOrderId),
            ExternalProjectId = request.ExternalProjectId,
            ExternalTaskId = request.ExternalTaskId,
            ExternalEventId = Normalize(request.ExternalEventId),
            SourceSystem = "GanttJsDemo",
            Payload = request.Payload,
            TaskSnapshot = request.TaskSnapshot,
            ContextSnapshot = request.ContextSnapshot,
            CreatedByPrincipalType = Normalize(request.CreatedByPrincipalType, fallback: "system"),
            CreatedByPrincipalId = Normalize(request.CreatedByPrincipalId)
        };

        var (operationalEvent, eventErrors) = await operationalEventService.CreateAsync(eventRequest, cancellationToken);
        if (eventErrors.Count > 0 || operationalEvent is null)
        {
            return (null, eventErrors);
        }

        AndonAlertResponse? andonAlertResponse = null;
        if (request.CreateAndonAlert)
        {
            var alertRequest = new CreateAndonAlertRequest
            {
                TenantId = eventRequest.TenantId,
                OperationalEventId = operationalEvent.Id,
                CreatedByPrincipalType = eventRequest.CreatedByPrincipalType,
                CreatedByPrincipalId = eventRequest.CreatedByPrincipalId
            };

            var (alert, alertErrors) = await andonAlertService.CreateAsync(alertRequest, cancellationToken);
            if (alertErrors.Count > 0 || alert is null)
            {
                return (null, alertErrors);
            }

            andonAlertResponse = AndonAlertResponse.FromEntity(alert);
        }

        return (new GanttTaskEventResponse
        {
            OperationalEvent = OperationalEventResponse.FromEntity(operationalEvent),
            AndonAlert = andonAlertResponse
        }, []);
    }

    public async Task<(GanttTaskStatusResponse? Response, IReadOnlyList<ApiError> Errors)> GetTaskStatusAsync(
        Guid externalTaskId,
        GanttTaskStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(externalTaskId, request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var tenantId = Normalize(request.TenantId);
        var fromUtc = NormalizeUtc(request.FromUtc);
        var toUtc = NormalizeUtc(request.ToUtc);
        var limit = request.Limit ?? 100;

        var eventsQuery = context.OperationalEvents
            .AsNoTracking()
            .Where(item =>
                item.TenantId == tenantId &&
                item.ExternalTaskId == externalTaskId);

        var alertsQuery = context.AndonAlerts
            .AsNoTracking()
            .Where(item =>
                item.TenantId == tenantId &&
                item.ExternalTaskId == externalTaskId);

        var actionItemsQuery = context.ActionItems
            .AsNoTracking()
            .Where(item =>
                item.TenantId == tenantId &&
                item.ExternalTaskId == externalTaskId);

        if (request.ExternalProjectId.HasValue)
        {
            eventsQuery = eventsQuery.Where(item => item.ExternalProjectId == request.ExternalProjectId.Value);
            alertsQuery = alertsQuery.Where(item => item.ExternalProjectId == request.ExternalProjectId.Value);
            actionItemsQuery = actionItemsQuery.Where(item => item.ExternalProjectId == request.ExternalProjectId.Value);
        }

        if (fromUtc.HasValue)
        {
            eventsQuery = eventsQuery.Where(item => item.OccurredUtc >= fromUtc.Value);
        }

        if (toUtc.HasValue)
        {
            eventsQuery = eventsQuery.Where(item => item.OccurredUtc <= toUtc.Value);
        }

        var events = await eventsQuery
            .OrderByDescending(item => item.OccurredUtc)
            .ThenByDescending(item => item.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var alerts = await alertsQuery
            .OrderBy(item => item.Status == AndonAlertStatus.Resolved || item.Status == AndonAlertStatus.Cancelled)
            .ThenByDescending(item => item.Severity)
            .ThenBy(item => item.OpenedUtc)
            .Take(limit)
            .ToListAsync(cancellationToken);

        var actionItems = await actionItemsQuery
            .OrderBy(item => item.Status == ActionItemStatus.Completed || item.Status == ActionItemStatus.Cancelled)
            .ThenBy(item => item.DueDate ?? DateOnly.MaxValue)
            .ThenByDescending(item => item.Priority)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return (new GanttTaskStatusResponse
        {
            TenantId = tenantId,
            ExternalProjectId = request.ExternalProjectId,
            ExternalTaskId = externalTaskId,
            Events = events.Select(OperationalEventResponse.FromEntity).ToList(),
            Alerts = alerts.Select(AndonAlertResponse.FromEntity).ToList(),
            ActionItems = actionItems.Select(ActionItemResponse.FromEntity).ToList()
        }, []);
    }

    private static List<ApiError> Validate(CreateGanttTaskEventRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");
        AddRequired(errors, request.Title, "title", "Title is required.");

        if (request.ExternalTaskId == Guid.Empty)
        {
            errors.Add(ApiError.Create("ExternalTaskId is required.", "required", "externalTaskId"));
        }

        if (!Enum.IsDefined(request.EventType))
        {
            errors.Add(ApiError.Create("EventType is not valid.", "invalid_event_type", "eventType"));
        }

        if (!Enum.IsDefined(request.Severity))
        {
            errors.Add(ApiError.Create("Severity is not valid.", "invalid_severity", "severity"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");
        AddMaxLength(errors, request.ExternalEventId, 80, "externalEventId");
        AddMaxLength(errors, request.Title, 200, "title");
        AddMaxLength(errors, request.Description, 2000, "description");
        AddMaxLength(errors, request.MachineId, 80, "machineId");
        AddMaxLength(errors, request.MachineName, 80, "machineName");
        AddMaxLength(errors, request.LineCode, 80, "lineCode");
        AddMaxLength(errors, request.WorkOrderId, 80, "workOrderId");
        AddMaxLength(errors, request.CreatedByPrincipalType, 24, "createdByPrincipalType");
        AddMaxLength(errors, request.CreatedByPrincipalId, 80, "createdByPrincipalId");

        return errors;
    }

    private static List<ApiError> Validate(Guid externalTaskId, GanttTaskStatusRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");

        if (externalTaskId == Guid.Empty)
        {
            errors.Add(ApiError.Create("ExternalTaskId is required.", "required", "externalTaskId"));
        }

        var fromUtc = NormalizeUtc(request.FromUtc);
        var toUtc = NormalizeUtc(request.ToUtc);
        if (fromUtc.HasValue && toUtc.HasValue && fromUtc.Value > toUtc.Value)
        {
            errors.Add(ApiError.Create("FromUtc must be earlier than ToUtc.", "invalid_date_range", "fromUtc"));
        }

        if (request.Limit is < 1 or > 500)
        {
            errors.Add(ApiError.Create("Limit must be between 1 and 500.", "invalid_limit", "limit"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");

        return errors;
    }

    private static void AddRequired(List<ApiError> errors, string value, string field, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            errors.Add(ApiError.Create(message, "required", field));
        }
    }

    private static void AddMaxLength(List<ApiError> errors, string value, int maxLength, string field)
    {
        if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
        {
            errors.Add(ApiError.Create($"{field} must be {maxLength} characters or fewer.", "max_length", field));
        }
    }

    private static string Normalize(string? value, string fallback = "")
        => string.IsNullOrWhiteSpace(value) ? fallback : value.Trim();

    private static DateTime? NormalizeUtc(DateTime? value)
    {
        if (value is null)
        {
            return null;
        }

        return value.Value.Kind switch
        {
            DateTimeKind.Utc => value.Value,
            DateTimeKind.Local => value.Value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value.Value, DateTimeKind.Utc)
        };
    }
}
