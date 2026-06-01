# Andon

Andon operational event module for the Smart Factory MVP.

This repository receives operational events from Gantt or plant integrations, manages Andon alerts, tracks comments and audit history, links RCA/Ishikawa incidents, and keeps corrective action items tied back to Gantt task public ids.

## Development rule

Each module is closed with the same workflow:

1. Implement the scoped block.
2. Validate locally.
3. Commit.
4. Push to the remote.

See [docs/DEVELOPMENT_WORKFLOW.md](docs/DEVELOPMENT_WORKFLOW.md).

## API examples

Integration examples for Gantt, Andon, RCA, and ActionItems are in [docs/API_EXAMPLES.md](docs/API_EXAMPLES.md).

## Validation

```powershell
dotnet build Andon.slnx
```
