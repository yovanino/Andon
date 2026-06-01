using Andon.Web.Data;
using Andon.Web.Dtos.ActionItems;
using Andon.Web.Dtos.Common;
using Andon.Web.Models.Domain;
using Andon.Web.Models.Shared;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Andon.Web.Services.ActionItems;

public sealed class ActionItemService(AppDbContext context) : IActionItemService
{
    public async Task<(ActionItem? ActionItem, IReadOnlyList<ApiError> Errors)> CreateAsync(
        CreateActionItemRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        AndonAlert? alert = null;
        if (request.RelatedAndonAlertId.HasValue)
        {
            alert = await context.AndonAlerts
                .AsNoTracking()
                .FirstOrDefaultAsync(item =>
                    item.Id == request.RelatedAndonAlertId.Value &&
                    item.TenantId == Normalize(request.TenantId),
                    cancellationToken);

            if (alert is null)
            {
                return (null, [ApiError.Create("Related Andon alert was not found for this tenant.", "alert_not_found", "relatedAndonAlertId")]);
            }
        }

        var now = DateTime.UtcNow;
        var actionItem = new ActionItem
        {
            TenantId = Normalize(request.TenantId),
            RelatedAndonAlertId = alert?.Id,
            ExternalRcaIncidentId = Normalize(request.ExternalRcaIncidentId, alert?.ExternalRcaIncidentId ?? ""),
            ExternalProjectId = request.ExternalProjectId ?? alert?.ExternalProjectId,
            ExternalTaskId = request.ExternalTaskId ?? alert?.ExternalTaskId,
            SourceSystem = Normalize(request.SourceSystem, alert?.SourceSystem ?? "Andon"),
            TaskSnapshotJson = SerializeJson(request.TaskSnapshot, alert?.TaskSnapshotJson),
            ContextSnapshotJson = SerializeJson(request.ContextSnapshot, alert?.ContextSnapshotJson),
            Title = Normalize(request.Title),
            Description = Normalize(request.Description),
            Status = ActionItemStatus.Open,
            Priority = request.Priority,
            AssignedPrincipalType = Normalize(request.AssignedPrincipalType, alert?.ResponsiblePrincipalType ?? ""),
            AssignedPrincipalId = Normalize(request.AssignedPrincipalId, alert?.ResponsiblePrincipalId ?? ""),
            AssignedDisplayName = Normalize(request.AssignedDisplayName, alert?.ResponsibleDisplayName ?? ""),
            DueDate = request.DueDate,
            CreatedByPrincipalType = Normalize(request.CreatedByPrincipalType, fallback: "system"),
            CreatedByPrincipalId = Normalize(request.CreatedByPrincipalId),
            CreatedUtc = now,
            UpdatedUtc = now
        };

        context.ActionItems.Add(actionItem);
        await context.SaveChangesAsync(cancellationToken);

        return (actionItem, []);
    }

    public async Task<(IReadOnlyList<ActionItem> ActionItems, IReadOnlyList<ApiError> Errors)> ListAsync(
        ListActionItemsRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return ([], errors);
        }

        var limit = request.Limit ?? 100;
        var query = context.ActionItems
            .AsNoTracking()
            .Where(item => item.TenantId == Normalize(request.TenantId));

        if (request.Status.HasValue)
        {
            query = query.Where(item => item.Status == request.Status.Value);
        }

        if (request.Priority.HasValue)
        {
            query = query.Where(item => item.Priority == request.Priority.Value);
        }

        if (request.RelatedAndonAlertId.HasValue)
        {
            query = query.Where(item => item.RelatedAndonAlertId == request.RelatedAndonAlertId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.ExternalRcaIncidentId))
        {
            query = query.Where(item => item.ExternalRcaIncidentId == Normalize(request.ExternalRcaIncidentId));
        }

        if (request.ExternalTaskId.HasValue)
        {
            query = query.Where(item => item.ExternalTaskId == request.ExternalTaskId.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.AssignedPrincipalType))
        {
            query = query.Where(item => item.AssignedPrincipalType == Normalize(request.AssignedPrincipalType));
        }

        if (!string.IsNullOrWhiteSpace(request.AssignedPrincipalId))
        {
            query = query.Where(item => item.AssignedPrincipalId == Normalize(request.AssignedPrincipalId));
        }

        if (request.DueFrom.HasValue)
        {
            query = query.Where(item => item.DueDate >= request.DueFrom.Value);
        }

        if (request.DueTo.HasValue)
        {
            query = query.Where(item => item.DueDate <= request.DueTo.Value);
        }

        var actionItems = await query
            .OrderBy(item => item.Status == ActionItemStatus.Completed || item.Status == ActionItemStatus.Cancelled)
            .ThenBy(item => item.DueDate ?? DateOnly.MaxValue)
            .ThenByDescending(item => item.Priority)
            .ThenBy(item => item.Id)
            .Take(limit)
            .ToListAsync(cancellationToken);

        return (actionItems, []);
    }

    public async Task<(ActionItem? ActionItem, IReadOnlyList<ApiError> Errors)> UpdateStatusAsync(
        long id,
        UpdateActionItemStatusRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(id, request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var actionItem = await context.ActionItems
            .FirstOrDefaultAsync(item =>
                item.Id == id &&
                item.TenantId == Normalize(request.TenantId),
                cancellationToken);

        if (actionItem is null)
        {
            return (null, [ApiError.Create("Action item was not found for this tenant.", "action_item_not_found", "id")]);
        }

        var now = DateTime.UtcNow;
        actionItem.Status = request.Status;
        actionItem.UpdatedUtc = now;

        if (request.Status is ActionItemStatus.Completed or ActionItemStatus.Cancelled)
        {
            actionItem.ClosedUtc = now;
            actionItem.ClosureComment = Normalize(request.ClosureComment);
        }
        else
        {
            actionItem.ClosedUtc = null;
            actionItem.ClosureComment = string.Empty;
        }

        await context.SaveChangesAsync(cancellationToken);

        return (actionItem, []);
    }

    private static List<ApiError> Validate(CreateActionItemRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");
        AddRequired(errors, request.Title, "title", "Title is required.");

        if (request.RelatedAndonAlertId is <= 0)
        {
            errors.Add(ApiError.Create("RelatedAndonAlertId must be greater than zero.", "invalid_alert_id", "relatedAndonAlertId"));
        }

        if (!Enum.IsDefined(request.Priority))
        {
            errors.Add(ApiError.Create("Priority is not valid.", "invalid_priority", "priority"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");
        AddMaxLength(errors, request.ExternalRcaIncidentId, 80, "externalRcaIncidentId");
        AddMaxLength(errors, request.SourceSystem, 40, "sourceSystem");
        AddMaxLength(errors, request.Title, 200, "title");
        AddMaxLength(errors, request.Description, 2000, "description");
        AddMaxLength(errors, request.AssignedPrincipalType, 24, "assignedPrincipalType");
        AddMaxLength(errors, request.AssignedPrincipalId, 80, "assignedPrincipalId");
        AddMaxLength(errors, request.AssignedDisplayName, 160, "assignedDisplayName");
        AddMaxLength(errors, request.CreatedByPrincipalType, 24, "createdByPrincipalType");
        AddMaxLength(errors, request.CreatedByPrincipalId, 80, "createdByPrincipalId");

        return errors;
    }

    private static List<ApiError> Validate(ListActionItemsRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");

        if (request.Status.HasValue && !Enum.IsDefined(request.Status.Value))
        {
            errors.Add(ApiError.Create("Status is not valid.", "invalid_status", "status"));
        }

        if (request.Priority.HasValue && !Enum.IsDefined(request.Priority.Value))
        {
            errors.Add(ApiError.Create("Priority is not valid.", "invalid_priority", "priority"));
        }

        if (request.RelatedAndonAlertId is <= 0)
        {
            errors.Add(ApiError.Create("RelatedAndonAlertId must be greater than zero.", "invalid_alert_id", "relatedAndonAlertId"));
        }

        if (request.DueFrom.HasValue && request.DueTo.HasValue && request.DueFrom.Value > request.DueTo.Value)
        {
            errors.Add(ApiError.Create("DueFrom must be earlier than DueTo.", "invalid_date_range", "dueFrom"));
        }

        if (request.Limit is < 1 or > 500)
        {
            errors.Add(ApiError.Create("Limit must be between 1 and 500.", "invalid_limit", "limit"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");
        AddMaxLength(errors, request.ExternalRcaIncidentId, 80, "externalRcaIncidentId");
        AddMaxLength(errors, request.AssignedPrincipalType, 24, "assignedPrincipalType");
        AddMaxLength(errors, request.AssignedPrincipalId, 80, "assignedPrincipalId");

        return errors;
    }

    private static List<ApiError> Validate(long id, UpdateActionItemStatusRequest request)
    {
        var errors = new List<ApiError>();

        if (id <= 0)
        {
            errors.Add(ApiError.Create("Id must be greater than zero.", "invalid_id", "id"));
        }

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");

        if (!Enum.IsDefined(request.Status))
        {
            errors.Add(ApiError.Create("Status is not valid.", "invalid_status", "status"));
        }

        if (request.Status is ActionItemStatus.Completed or ActionItemStatus.Cancelled &&
            string.IsNullOrWhiteSpace(request.ClosureComment))
        {
            errors.Add(ApiError.Create("ClosureComment is required to close an action item.", "required", "closureComment"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");
        AddMaxLength(errors, request.ClosureComment, 2000, "closureComment");

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
