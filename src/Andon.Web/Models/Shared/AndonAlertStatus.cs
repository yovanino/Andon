namespace Andon.Web.Models.Shared;

public enum AndonAlertStatus
{
    New = 1,
    Acknowledged = 2,
    InProgress = 3,
    Escalated = 4,
    Resolved = 5,
    Cancelled = 6
}
