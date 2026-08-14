# Topic 12: Supervisory Control and Safety

## Theme

Send operator and supervisory commands through a guarded control path with acknowledgment and audit history.

## Learning outcomes

- implement manual, automatic, and AI-supervisory modes;
- validate commands against permissions, state, and engineering limits;
- acknowledge, reject, and log command execution;
- design deterministic interlocks independent of the AI agent.

## Preparation

- Define permitted actions and safety limits from the Topic 2 specification.
- Review interlocks, permissives, and operating modes.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Supervisory versus regulatory control |
| 00:30-01:10 | Command schema, validation gateway, and acknowledgments |
| 01:10-01:30 | Interlocks and safe-state behavior |
| 01:30-02:30 | Implement operator commands end to end |
| 02:30-02:50 | Attempt invalid, duplicate, and unsafe commands |
| 02:50-03:00 | Safety review |

## Laboratory

Implement commands for setpoint change, mode change, start/stop, and one project-specific action. Route every command through the same validation gateway. Show accepted and rejected commands in an event log with a reason.

## Deliverable

Closed-loop twin with guarded bidirectional control and a documented command-state diagram.

## Evidence and assessment

- The process, not Godot, owns authoritative state
- Every action is bounded, acknowledged, and logged
- Emergency and interlock logic remains deterministic
- Manual control remains available when the agent is unavailable

## Instructor notes

Treat this gateway as the boundary the AI agent must use in Weeks 14 and 15. Do not give the agent unrestricted access to simulation internals or arbitrary code execution.
