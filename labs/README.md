# Labs

Step-by-step labs that build the course digital-twin stack from [assignments/system-architecture.md](../assignments/system-architecture.md).

```
┌──────────────────┐      ┌─────────────────┐      ┌──────────────────────┐
│ Python simulator │ ◄──► │ MQTT (Docker)   │ ◄──► │ Godot twin (C#)      │
│ Lab 02           │      │ Lab 01          │      │ Lab 03 / 03A         │
└──────────────────┘      └────────┬────────┘      └──────────────────────┘
                                   │
                                   ▼
                          ┌─────────────────┐
                          │ MQTTX           │
                          │ (test only)     │
                          └─────────────────┘
```

## Supported platforms

| OS | Terminal for lab commands | Docker |
|---|---|---|
| **Windows** | **PowerShell 7+** (`pwsh`) — required | Docker Desktop (WSL 2) |
| **macOS** | Terminal.app or iTerm2 | Docker Desktop |
| **Linux** | bash or zsh | Docker Engine or Docker Desktop |

Install software from [assignments/required-dev-tools.md](../assignments/required-dev-tools.md) before starting.

## Lab order

| Lab | Folder | Status | Goal |
|---:|---|---|---|
| 01 | [docker/](docker/) | Ready | Start Mosquitto with Docker; verify with MQTTX |
| 02 | [simulator/](simulator/) | Ready | Coupled-tank Python simulator (telemetry + commands) |
| 03A | [godot-simple/](godot-simple/) | Ready | Warmup: MQTT → Godot smooth rotate / transform |
| 03 | [godot/](godot/) | Ready | Godot C# twin / HMI (subscribe + send commands) |

**Suggested path:** 01 → 02 → **03A** → 03. Lab 03A is optional but recommended if you are new to Godot C# + MQTT.

## How to work

1. Finish [assignments/required-dev-tools.md](../assignments/required-dev-tools.md) first.
2. Use the **correct terminal for your OS** (Windows students: PowerShell 7+ only).
3. Do labs **in order** — each lab assumes the previous one still works.
4. Use **MQTTX only for inspection and evidence**, not as the operator UI.
5. Keep a short lab log: date, lab number, OS, what worked, blockers.

## Start here

Open **[Lab 01 — Docker MQTT](docker/README.md)**.
