using Andon.Web.Data;
using Andon.Web.Dtos.Andon;
using Andon.Web.Dtos.Common;
using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Andon.Web.Services.Andon;

public sealed class AndonAlertService(AppDbContext context) : IAndonAlertService
{
    public async Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> CreateAsync(
        CreateAndonAlertRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        OperationalEvent? operationalEvent = null;
        if (request.OperationalEventId.HasValue)
        {
            operationalEvent = await context.OperationalEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(item =>
                    item.Id == request.OperationalEventId.Value &&
                    item.TenantId == Normalize(request.TenantId),
                    cancellationToken);

            if (operationalEvent is null)
            {
                return (null, [ApiError.Create("Operational event was not found for this tenant.", "event_not_found", "operationalEventId")]);
            }
        }

        var title = Normalize(request.Title, operationalEvent?.Title ?? "");
        if (string.IsNullOrWhiteSpace(title))
        {
            return (null, [ApiError.Create("Title is required when an alert is not created from an event.", "required", "title")]);
        }

        var now = DateTime.UtcNow;
        var alert = new AndonAlert
        {
            TenantId = Normalize(request.TenantId),
            OperationalEventId = operationalEvent?.Id,
            Status = AndonAlertStatus.New,
            Severity = request.Severity ?? operationalEvent?.Severity ?? OperationalSeverity.Medium,
            Title = title,
            Description = Normalize(request.Description, operationalEvent?.Description ?? ""),
            MachineId = Normalize(request.MachineId, operationalEvent?.MachineId ?? ""),
            MachineName = Normalize(request.MachineName, operationalEvent?.MachineName ?? ""),
            LineCode = Normalize(request.LineCode, operationalEvent?.LineCode ?? ""),
            WorkOrderId = Normalize(request.WorkOrderId, operationalEvent?.WorkOrderId ?? ""),
            ExternalProjectId = request.ExternalProjectId ?? operationalEvent?.ExternalProjectId,
            ExternalTaskId = request.ExternalTaskId ?? operationalEvent?.ExternalTaskId,
            SourceSystem = Normalize(request.SourceSystem, operationalEvent?.SourceSystem ?? "Andon"),
            TaskSnapshotJson = SerializeJson(request.TaskSnapshot, operationalEvent?.TaskSnapshotJson),
            ContextSnapshotJson = SerializeJson(request.ContextSnapshot, operationalEvent?.ContextSnapshotJson),
            ResponsiblePrincipalType = Normalize(request.ResponsiblePrincipalType),
            ResponsiblePrincipalId = Normalize(request.ResponsiblePrincipalId),
            ResponsibleDisplayName = Normalize(request.ResponsibleDisplayName),
            OpenedUtc = now,
            CreatedByPrincipalType = Normalize(request.CreatedByPrincipalType, fallback: "system"),
            CreatedByPrincipalId = Normalize(request.CreatedByPrincipalId),
            CreatedUtc = now,
            UpdatedUtc = now
        };

        context.AndonAlerts.Add(alert);
        await context.SaveChangesAsync(cancellationToken);

        return (alert, []);
    }

    private static List<ApiError> Validate(CreateAndonAlertRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");

        if (request.OperationalEventId is <= 0)
        {
            errors.Add(ApiError.Create("OperationalEventId must be greater than zero.", "invalid_event_id", "operationalEventId"));
        }

        if (request.Severity.HasValue && !Enum.IsDefined(request.Severity.Value))
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
        AddMaxLength(errors, request.SourceSystem, 40, "sourceSystem");
        AddMaxLength(errors, request.ResponsiblePrincipalType, 24, "responsiblePrincipalType");
        AddMaxLength(errors, request.ResponsiblePrincipalId, 80, "responsiblePrincipalId");
        AddMaxLength(errors, request.ResponsibleDisplayName, 160, "responsibleDisplayName");
        AddMaxLength(errors, request.CreatedByPrincipalType, 24, "createdByPrincipalType");
        AddMaxLength(errors, request.CreatedByPrincipalId, 80, "createdByPrincipalId");

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

    private static string SerializeJson(JsonElement? value, string? fallback = null)
    {
        if (value is null || value.Value.ValueKind is JsonValueKind.Undefined or JsonValueKind.Null)
        {
            return string.IsNullOrWhiteSpace(fallback) ? "{}" : fallback;
        }

        return value.Value.GetRawText();
    }
}
