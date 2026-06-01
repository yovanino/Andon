using Andon.Web.Data;
using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Events;
using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Andon.Web.Services.Events;

public sealed class OperationalEventService(AppDbContext context) : IOperationalEventService
{
    public async Task<(OperationalEvent? Event, IReadOnlyList<ApiError> Errors)> CreateAsync(
        CreateOperationalEventRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var now = DateTime.UtcNow;
        var operationalEvent = new OperationalEvent
        {
            TenantId = Normalize(request.TenantId),
            Source = request.Source,
            EventType = request.EventType,
            Severity = request.Severity,
            Title = Normalize(request.Title),
            Description = Normalize(request.Description),
            OccurredUtc = NormalizeUtc(request.OccurredUtc) ?? now,
            MachineId = Normalize(request.MachineId),
            MachineName = Normalize(request.MachineName),
            LineCode = Normalize(request.LineCode),
            WorkOrderId = Normalize(request.WorkOrderId),
            ExternalProjectId = request.ExternalProjectId,
            ExternalTaskId = request.ExternalTaskId,
            ExternalEventId = Normalize(request.ExternalEventId),
            SourceSystem = Normalize(request.SourceSystem),
            PayloadJson = SerializeJson(request.Payload),
            TaskSnapshotJson = SerializeJson(request.TaskSnapshot),
            ContextSnapshotJson = SerializeJson(request.ContextSnapshot),
            CreatedByPrincipalType = Normalize(request.CreatedByPrincipalType, fallback: "system"),
            CreatedByPrincipalId = Normalize(request.CreatedByPrincipalId),
            CreatedUtc = now
        };

        context.OperationalEvents.Add(operationalEvent);
        await context.SaveChangesAsync(cancellationToken);

        return (operationalEvent, []);
    }

    public async Task<(IReadOnlyList<OperationalEvent> Events, IReadOnlyList<ApiError> Errors)> GetLiveAsync(
        LiveOperationalEventsRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return ([], errors);
        }

        var now = DateTime.UtcNow;
        var fromUtc = NormalizeUtc(request.FromUtc) ?? now.AddHours(-24);
        var toUtc = NormalizeUtc(request.ToUtc) ?? now;
        var limit = request.Limit ?? 100;

        var query = context.OperationalEvents
            .AsNoTracking()
            .Where(item =>
                item.TenantId == Normalize(request.TenantId) &&
                item.OccurredUtc >= fromUtc &&
                item.OccurredUtc <= toUtc);

        if (request.Severity.HasValue)
        {
            query = query.Where(item => item.Severity == request.Severity.Value);
        }

        if (request.Source.HasValue)
        {
            query = query.Where(item => item.Source == request.Source.Value);
        }

        if (request.EventType.HasValue)
        {
            query = query.Where(item => item.EventType == request.EventType.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.MachineId))
        {
            query = query.Where(item => item.MachineId == Normalize(request.MachineId));
        }

        if (!string.IsNullOrWhiteSpace(request.LineCode))
        {
            query = query.Where(item => item.LineCode == Normalize(request.LineCode));
        }

        if (request.ExternalTaskId.HasValue)
        {
            query = query.Where(item => item.ExternalTaskId == request.ExternalTaskId.Value);
        }

        var events = await query
            .OrderByDescending(item => item.OccurredUtc)
            .ThenByDescending(item => item.CreatedUtc)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return (events, []);
    }

    private static List<ApiError> Validate(CreateOperationalEventRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");
        AddRequired(errors, request.Title, "title", "Title is required.");

        if (!Enum.IsDefined(request.Source))
        {
            errors.Add(ApiError.Create("Source is not valid.", "invalid_source", "source"));
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
        AddMaxLength(errors, request.Title, 200, "title");
        AddMaxLength(errors, request.Description, 2000, "description");
        AddMaxLength(errors, request.MachineId, 80, "machineId");
        AddMaxLength(errors, request.MachineName, 80, "machineName");
        AddMaxLength(errors, request.LineCode, 80, "lineCode");
        AddMaxLength(errors, request.WorkOrderId, 80, "workOrderId");
        AddMaxLength(errors, request.ExternalEventId, 80, "externalEventId");
        AddMaxLength(errors, request.SourceSystem, 40, "sourceSystem");
        AddMaxLength(errors, request.CreatedByPrincipalType, 24, "createdByPrincipalType");
        AddMaxLength(errors, request.CreatedByPrincipalId, 80, "createdByPrincipalId");

        return errors;
    }

    private static List<ApiError> Validate(LiveOperationalEventsRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");

        if (request.Severity.HasValue && !Enum.IsDefined(request.Severity.Value))
        {
            errors.Add(ApiError.Create("Severity is not valid.", "invalid_severity", "severity"));
        }

        if (request.Source.HasValue && !Enum.IsDefined(request.Source.Value))
        {
            errors.Add(ApiError.Create("Source is not valid.", "invalid_source", "source"));
        }

        if (request.EventType.HasValue && !Enum.IsDefined(request.EventType.Value))
        {
            errors.Add(ApiError.Create("EventType is not valid.", "invalid_event_type", "eventType"));
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
        AddMaxLength(errors, request.MachineId, 80, "machineId");
        AddMaxLength(errors, request.LineCode, 80, "lineCode");

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

    private static string SerializeJson(JsonElement? value)
    {
        if (value is null || value.Value.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return "{}";
        }

        return value.Value.GetRawText();
    }
}
