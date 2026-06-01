using Andon.Web.Data;
using Andon.Web.Dtos.Common;
using Andon.Web.Dtos.Rca;
using Andon.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Andon.Web.Services.Rca;

public sealed class RcaIntegrationService(AppDbContext context) : IRcaIntegrationService
{
    public async Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> RequestIncidentFromAlertAsync(
        RequestRcaIncidentFromAlertRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var alert = await FindAlertAsync(request.TenantId, request.AndonAlertId, cancellationToken);
        if (alert is null)
        {
            return (null, [ApiError.Create("Andon alert was not found for this tenant.", "alert_not_found", "andonAlertId")]);
        }

        if (!string.IsNullOrWhiteSpace(alert.ExternalRcaIncidentId) &&
            string.IsNullOrWhiteSpace(request.ExternalRcaIncidentId))
        {
            return (null, [ApiError.Create("Andon alert is already linked to an RCA incident.", "rca_already_linked", "externalRcaIncidentId")]);
        }

        var now = DateTime.UtcNow;
        alert.ExternalRcaIncidentId = Normalize(request.ExternalRcaIncidentId, alert.ExternalRcaIncidentId);
        alert.RcaStatus = Normalize(request.RcaStatus, string.IsNullOrWhiteSpace(alert.ExternalRcaIncidentId) ? "Requested" : "Linked");
        alert.RcaCreatedUtc ??= now;
        alert.UpdatedUtc = now;

        await context.SaveChangesAsync(cancellationToken);

        return (alert, []);
    }

    public async Task<(AndonAlert? Alert, IReadOnlyList<ApiError> Errors)> LinkIncidentAsync(
        LinkRcaIncidentRequest request,
        CancellationToken cancellationToken = default)
    {
        var errors = Validate(request);
        if (errors.Count > 0)
        {
            return (null, errors);
        }

        var alert = await FindAlertAsync(request.TenantId, request.AndonAlertId, cancellationToken);
        if (alert is null)
        {
            return (null, [ApiError.Create("Andon alert was not found for this tenant.", "alert_not_found", "andonAlertId")]);
        }

        var now = DateTime.UtcNow;
        alert.ExternalRcaIncidentId = Normalize(request.ExternalRcaIncidentId);
        alert.RcaStatus = Normalize(request.RcaStatus, "Linked");
        alert.RcaCreatedUtc ??= now;
        alert.UpdatedUtc = now;

        await context.SaveChangesAsync(cancellationToken);

        return (alert, []);
    }

    private Task<AndonAlert?> FindAlertAsync(
        string tenantId,
        long andonAlertId,
        CancellationToken cancellationToken)
    {
        var normalizedTenantId = Normalize(tenantId);

        return context.AndonAlerts
            .FirstOrDefaultAsync(item =>
                item.Id == andonAlertId &&
                item.TenantId == normalizedTenantId,
                cancellationToken);
    }

    private static List<ApiError> Validate(RequestRcaIncidentFromAlertRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");

        if (request.AndonAlertId <= 0)
        {
            errors.Add(ApiError.Create("AndonAlertId must be greater than zero.", "invalid_alert_id", "andonAlertId"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");
        AddMaxLength(errors, request.ExternalRcaIncidentId, 80, "externalRcaIncidentId");
        AddMaxLength(errors, request.RcaStatus, 40, "rcaStatus");
        AddMaxLength(errors, request.RequestedByPrincipalType, 24, "requestedByPrincipalType");
        AddMaxLength(errors, request.RequestedByPrincipalId, 80, "requestedByPrincipalId");

        return errors;
    }

    private static List<ApiError> Validate(LinkRcaIncidentRequest request)
    {
        var errors = new List<ApiError>();

        AddRequired(errors, request.TenantId, "tenantId", "TenantId is required.");
        AddRequired(errors, request.ExternalRcaIncidentId, "externalRcaIncidentId", "ExternalRcaIncidentId is required.");

        if (request.AndonAlertId <= 0)
        {
            errors.Add(ApiError.Create("AndonAlertId must be greater than zero.", "invalid_alert_id", "andonAlertId"));
        }

        AddMaxLength(errors, request.TenantId, 64, "tenantId");
        AddMaxLength(errors, request.ExternalRcaIncidentId, 80, "externalRcaIncidentId");
        AddMaxLength(errors, request.RcaStatus, 40, "rcaStatus");

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
}
