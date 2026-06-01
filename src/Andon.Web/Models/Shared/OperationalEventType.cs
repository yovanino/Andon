namespace Andon.Web.Models.Shared;

public enum OperationalEventType
{
    TaskIssueReported = 1,
    MachineStopped = 2,
    QualityIssue = 3,
    MaterialShortage = 4,
    SafetyIssue = 5,
    DelayDetected = 6,
    MaintenanceRequired = 7,
    ManualObservation = 8,
    IntegrationSignal = 9
}
