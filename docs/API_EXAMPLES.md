# Andon API examples

Base path:

```text
https://{host}/api/v1
```

All examples use `tenantId` as the plant/module boundary. Keep the same `tenantId` across Gantt, Andon, RCA, and ActionItems when testing one operational flow.

## 1. Receive an event from Gantt and open Andon

```http
POST /api/v1/gantt/task-events
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "externalProjectId": "11111111-1111-1111-1111-111111111111",
  "externalTaskId": "22222222-2222-2222-2222-222222222222",
  "externalEventId": "gantt-delay-0001",
  "eventType": "DelayDetected",
  "severity": "High",
  "title": "Demora en tarea de montaje",
  "description": "La tarea supero la ventana planificada por falta de material.",
  "machineId": "L2-MOLINO-01",
  "machineName": "Molino 01",
  "lineCode": "L2",
  "workOrderId": "WO-10045",
  "createAndonAlert": true,
  "taskSnapshot": {
    "name": "Montaje de subconjunto",
    "progress": 65,
    "plannedStart": "2026-06-01T10:00:00Z",
    "plannedEnd": "2026-06-01T12:00:00Z"
  },
  "contextSnapshot": {
    "shift": "A",
    "area": "Laminacion"
  },
  "createdByPrincipalType": "system",
  "createdByPrincipalId": "gantt-js-demo"
}
```

Use the response `andonAlert.id` in the next Andon examples.

## 2. Query Gantt task status

```http
GET /api/v1/gantt/tasks/22222222-2222-2222-2222-222222222222/status?tenantId=ternium-dev&externalProjectId=11111111-1111-1111-1111-111111111111&limit=50
```

Returns operational events, Andon alerts, and action items linked to the Gantt task public id.

## 3. Transition an Andon alert

```http
POST /api/v1/andon/alerts/{alertId}/transition
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "action": "Assign",
  "responsiblePrincipalType": "user",
  "responsiblePrincipalId": "supervisor.l2",
  "responsibleDisplayName": "Supervisor L2",
  "comment": "Asignado a supervisor de linea.",
  "actorPrincipalType": "user",
  "actorPrincipalId": "operador.01"
}
```

Supported `action` values for this endpoint:

- `Acknowledge`
- `Assign`
- `Escalate`
- `Resolve`
- `Cancel`

`Resolve` and `Cancel` require `comment`.

## 4. Add and read Andon comments

```http
POST /api/v1/andon/alerts/{alertId}/comments
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "comment": "Mantenimiento confirma inspeccion en sitio.",
  "createdByPrincipalType": "user",
  "createdByPrincipalId": "mantenimiento.03",
  "createdByDisplayName": "Mantenimiento 03"
}
```

```http
GET /api/v1/andon/alerts/{alertId}/comments?tenantId=ternium-dev&limit=100
```

## 5. Read Andon audit history

```http
GET /api/v1/andon/alerts/{alertId}/history?tenantId=ternium-dev&limit=100
```

History is written automatically for alert creation, transitions, and comments.

## 6. Create an action item

```http
POST /api/v1/action-items
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "relatedAndonAlertId": 1,
  "externalRcaIncidentId": "",
  "externalProjectId": "11111111-1111-1111-1111-111111111111",
  "externalTaskId": "22222222-2222-2222-2222-222222222222",
  "sourceSystem": "Andon",
  "title": "Regularizar abastecimiento de material",
  "description": "Coordinar reposicion y registrar causa de demora.",
  "priority": "High",
  "assignedPrincipalType": "team",
  "assignedPrincipalId": "logistica",
  "assignedDisplayName": "Logistica",
  "dueDate": "2026-06-02",
  "createdByPrincipalType": "user",
  "createdByPrincipalId": "supervisor.l2"
}
```

## 7. Close an action item

```http
POST /api/v1/action-items/{actionItemId}/status
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "status": "Completed",
  "closureComment": "Material repuesto y tarea reprogramada."
}
```

`Completed` and `Cancelled` require `closureComment`.

## 8. Request or link RCA/Ishikawa

Register that an RCA was requested from an Andon alert:

```http
POST /api/v1/rca/incidents/from-alert
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "andonAlertId": 1,
  "rcaStatus": "Requested",
  "requestedByPrincipalType": "user",
  "requestedByPrincipalId": "supervisor.l2"
}
```

Link an existing RCA incident:

```http
POST /api/v1/rca/incidents/link
Content-Type: application/json
```

```json
{
  "tenantId": "ternium-dev",
  "andonAlertId": 1,
  "externalRcaIncidentId": "RCA-2026-0007",
  "rcaStatus": "Linked"
}
```

The integration remains decoupled: Andon stores the external RCA incident id but does not require a direct foreign key to the RCA/Ishikawa repository.
