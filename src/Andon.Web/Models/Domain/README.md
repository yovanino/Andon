# Domain models

Persistent domain entities for operational events, Andon alerts, comments, and action items.

Current entities:

- `OperationalEvent`: base auditable record for events received from Gantt, operators, SCADA, PLC, RCA, or external integrations.
- `AndonAlert`: operational alert created from an event, task, operator action, or future plant signal.
