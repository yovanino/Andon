# Shared models

Shared value objects, enums, and audit or tenant abstractions used by multiple Andon areas.

Current enum contracts:

- `OperationalSeverity`: common severity scale for events, alerts, Gantt status, and RCA handoff.
- `EventSourceType`: source module or integration that created the event.
- `OperationalEventType`: normalized operational event classification.
- `AndonAlertStatus`: alert lifecycle state.
- `AndonTransitionAction`: auditable action names for alert lifecycle changes.
- `ActionItemStatus`: execution state for corrective and follow-up actions.
