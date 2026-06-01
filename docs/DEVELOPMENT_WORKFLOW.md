# Development workflow

This repository follows the same delivery rule as the Gantt and RCA modules.

For each closed module or development block:

1. Implement the scoped change.
2. Validate locally with the relevant build, tests, or checklist.
3. Commit with a clear message.
4. Push and sync with the configured remote.
5. Start the next block only after the previous one is clean.

Current baseline validation:

```powershell
dotnet build Andon.slnx
```
