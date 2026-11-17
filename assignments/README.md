# Assignments

| Document | Description |
|---|---|
| [Required dev tools](required-dev-tools.md) | Software to install before labs: Docker, MQTTX, Python, Node.js |
| [Required knowledge](required-knowledge.md) | Self-study topics and skills before class: Python, C#, Docker, MQTT, JSON, control basics |
| [System architecture](system-architecture.md) | Python simulator ↔ MQTT (Docker) ↔ Godot digital twin reference architecture, including the AI-agent extension and guarded action path |

## AI-agent architecture and Quiz 3

The system architecture document describes how an AI agent can join the digital-twin system as another MQTT client, inspect telemetry and events, and propose actions through the same validation gateway used for operator commands. The agent must not bypass deterministic control, interlocks, authorization, or safety limits.

The related assessed course topics are:

- [Topic 14 — AI Agent Tools and Structured Actions](../topics/topic-14-ai-agent-tools.md)
- [Topic 15 — Agent-Driven Diagnosis and Testing](../topics/topic-15-agent-diagnosis-and-testing.md)

These topics are included in **Quiz 3 Part A (Theory)** together with Topics 10–13. See the [Quiz 3 preparation guide](../quizzes/quiz3-prepare.md) for the complete scope and practical preparation.
