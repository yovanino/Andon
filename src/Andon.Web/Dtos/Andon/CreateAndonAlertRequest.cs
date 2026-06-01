using Andon.Web.Models.Shared;
using System.Text.Json;

namespace Andon.Web.Dtos.Andon;

public sealed record CreateAndonAlertRequest
{
    public string TenantId { get; init; } = string.Empty;

    public long? OperationalEventId { get; init; }

    public OperationalSeverity? Severity { get; init; }

    public string Title { get; init; } = string.Empty;

    public string Description { get; init; } = string.Empty;

    public string MachineId { get; init; } = string.Empty;

    public string MachineName { get; init; } = string.Empty;

    public string LineCode { get; init; } = string.Empty;

    public string WorkOrderId { get; init; } = string.Empty;

    public Guid? ExternalProjectId { get; init; }

    public Guid? ExternalTaskId { get; init; }

    public string SourceSystem { get; init; } = string.Empty;

    public JsonElement? TaskSnapshot { get; init; }

    public JsonElement? ContextSnapshot { get; init; }

    public string ResponsiblePrincipalType { get; init; } = string.Empty;

    public string ResponsiblePrincipalId { get; init; } = string.Empty;

    public string ResponsibleDisplayName { get; init; } = string.Empty;

    public string CreatedByPrincipalType { get; init; } = string.Empty;

    public string CreatedByPrincipalId { get; init; } = string.Empty;
}
