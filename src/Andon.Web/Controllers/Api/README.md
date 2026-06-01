# API controllers

REST endpoints for external module integration live here.

See `docs/API_EXAMPLES.md` for request examples and an end-to-end integration flow.

Planned groups:

- Events API: operational events from Gantt, operators, SCADA, or future integrations.
- Andon API: alert lifecycle actions.
- Gantt integration API: operational status by external Gantt task public id.
- RCA integration API: create or link RCA incidents.

Implemented endpoints:

- `POST /api/v1/events`
- `GET /api/v1/events/live`
- `POST /api/v1/andon/alerts`
- `GET /api/v1/andon/alerts/live`
- `POST /api/v1/andon/alerts/{id}/transition`
- `GET /api/v1/andon/alerts/{id}/comments`
- `POST /api/v1/andon/alerts/{id}/comments`
- `GET /api/v1/andon/alerts/{id}/history`
- `GET /api/v1/action-items`
- `POST /api/v1/action-items`
- `POST /api/v1/action-items/{id}/status`
- `GET /api/v1/gantt/tasks/{externalTaskId}/status`
- `POST /api/v1/gantt/task-events`
- `POST /api/v1/rca/incidents/from-alert`
- `POST /api/v1/rca/incidents/link`
