# Domain models

Persistent domain entities for operational events, Andon alerts, comments, and action items.

Current entities:

- `OperationalEvent`: base auditable record for events received from Gantt, operators, SCADA, PLC, RCA, or external integrations.
- `AndonAlert`: operational alert created from an event, task, operator action, or future plant signal.
- `AndonComment`: timestamped comment stream attached to an alert for shift, supervisor, and maintenance traceability.
- `ActionItem`: corrective or follow-up action linked to Andon, RCA, and optionally a Gantt task.

Gantt integration fields are kept as external references and snapshots, not direct foreign keys:

- `ExternalProjectId`
- `ExternalTaskId`
- `SourceSystem`
- `TaskSnapshotJson`
- `ContextSnapshotJson`

RCA integration fields are also external references:

- `ExternalRcaIncidentId`
- `RcaStatus`
- `RcaCreatedUtc`
