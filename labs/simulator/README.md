# Lab 02 — Coupled-tank Python simulator

Run a **Python process simulator** that:

- models a **coupled-tank** plant (two levels + valve)
- publishes telemetry on `twin/telemetry/tank`
- accepts commands on `twin/command/controller`
- publishes accept/reject events on `twin/event`
- publishes online/offline status on `twin/status/sim` (with MQTT Last Will)

**MQTTX is for testing only.** Lab 01 broker must already be running.

## Goal

- Create a venv and install `paho-mqtt`
- Start the simulator against `localhost:1883`
- Watch telemetry in MQTTX
- Send valid and invalid commands from MQTTX and see `twin/event` results

## Prerequisites

| Requirement | Notes |
|---|---|
| Lab 01 complete | [labs/docker/README.md](../docker/README.md) — `twin-mqtt` running |
| Python 3.11+ | see [required-dev-tools.md](../../assignments/required-dev-tools.md) |
| MQTTX | connected to `localhost:1883`, subscribed to `twin/#` |
| Terminal | **Windows: PowerShell 7+** · **macOS/Linux: Terminal/bash/zsh** |

Architecture reference: [assignments/system-architecture.md](../../assignments/system-architecture.md)

## What you will run

```text
labs/simulator/
├── README.md
├── requirements.txt
└── src/
    ├── main.py          ← entry point
    ├── process.py       ← coupled-tank dynamics
    ├── controller.py    ← manual / auto + command validation
    └── mqtt_client.py   ← envelope, seq, status/LWT
```

## Coupled-tank overview

```text
  inflow (valve_pct)
        │
        ▼
   ┌─────────┐   orifice    ┌─────────┐
   │ Tank 1  │─────────────▶│ Tank 2  │──▶ outflow
   │ level1  │◀─────────────│ level2  │
   └─────────┘              └─────────┘
                              ▲
                         setpoint (auto)
```

- Telemetry includes `level1_m`, `level2_m`, `valve_pct`, `controller_mode`, `setpoint_m`
- Auto mode uses a simple P-controller on **tank 2** level

## Steps

### 1. Confirm Lab 01 broker is up

**Windows (PowerShell 7+) / macOS / Linux**

```bash
cd <path-to-repo>/INC492-2026/labs/docker
docker compose ps
```

`twin-mqtt` must be **running**. If not:

```bash
docker compose up -d
```

### 2. Open a terminal in the simulator folder

**Windows (PowerShell 7+)**

```powershell
cd <path-to-repo>\INC492-2026\labs\simulator
```

**macOS / Linux**

```bash
cd <path-to-repo>/INC492-2026/labs/simulator
```

### 3. Create and activate a virtual environment

**Windows (PowerShell 7+)**

```powershell
python -m venv .venv
.\.venv\Scripts\Activate.ps1
python --version
```

If activation is blocked:

```powershell
Set-ExecutionPolicy -Scope CurrentUser RemoteSigned
.\.venv\Scripts\Activate.ps1
```

**macOS / Linux**

```bash
python3 -m venv .venv
source .venv/bin/activate
python --version
```

### 4. Install dependencies

All OS (with venv active):

```bash
python -m pip install --upgrade pip
python -m pip install -r requirements.txt
```

### 5. Start the simulator

All OS:

```bash
python src/main.py
```

Expected console lines include:

```text
[sim] connecting to localhost:1883 at 2.0 Hz
[mqtt] connected to localhost:1883, subscribed twin/command/controller
```

Leave this terminal running. Stop later with `Ctrl+C`.

Optional flags:

```bash
python src/main.py --host localhost --port 1883 --rate 2
```

### 6. MQTTX check A — telemetry

In MQTTX (still connected from Lab 01):

1. Subscribe to `twin/#` if not already
2. Confirm messages arrive on `twin/telemetry/tank` about **2 times per second**
3. Inspect one message — it must include:

| Field | Expected |
|---|---|
| `schema_version` | `"1.0"` |
| `source` | `"sim/python"` |
| `timestamp_utc` | ISO time ending with `Z` |
| `seq` | increasing integer |
| `payload.level1_m` | number |
| `payload.level2_m` | number |
| `payload.valve_pct` | 0–100 |
| `payload.controller_mode` | `manual` or `auto` |

Also confirm retained/online status on `twin/status/sim` with `"state": "online"`.

### 7. MQTTX check B — accepted command

Publish to topic **`twin/command/controller`** (QoS 1) with payload:

```json
{
  "schema_version": "1.0",
  "source": "lab02/mqttx",
  "timestamp_utc": "2026-10-28T00:00:00.000Z",
  "seq": 1,
  "payload": {
    "command_id": "cmd-lab02-001",
    "action": "set_valve",
    "params": { "valve_pct": 70 },
    "requested_by": "student"
  }
}
```

Pass when:

- `twin/event` shows `"result": "accepted"`
- Simulator console prints `[cmd] OK ...`
- Telemetry `valve_pct` moves toward ~70 (manual mode)

### 8. MQTTX check C — rejected command

Publish a bad command (missing `level_m`):

```json
{
  "schema_version": "1.0",
  "source": "lab02/mqttx",
  "timestamp_utc": "2026-10-28T00:00:00.000Z",
  "seq": 2,
  "payload": {
    "command_id": "cmd-lab02-bad",
    "action": "set_setpoint",
    "params": {},
    "requested_by": "student"
  }
}
```

Pass when `twin/event` shows `"result": "rejected"` and a clear `reason`.

### 9. Try auto mode (optional but recommended)

1. Set mode:

```json
{
  "schema_version": "1.0",
  "source": "lab02/mqttx",
  "timestamp_utc": "2026-10-28T00:00:00.000Z",
  "seq": 3,
  "payload": {
    "command_id": "cmd-lab02-mode",
    "action": "set_mode",
    "params": { "mode": "auto" },
    "requested_by": "student"
  }
}
```

2. Set setpoint:

```json
{
  "schema_version": "1.0",
  "source": "lab02/mqttx",
  "timestamp_utc": "2026-10-28T00:00:00.000Z",
  "seq": 4,
  "payload": {
    "command_id": "cmd-lab02-sp",
    "action": "set_setpoint",
    "params": { "level_m": 1.2 },
    "requested_by": "student"
  }
}
```

Watch `level2_m` move toward `1.2` and `valve_pct` change automatically.

## Supported commands

| action | params | Notes |
|---|---|---|
| `set_mode` | `mode`: `manual` \| `auto` | Required for mode changes |
| `set_setpoint` | `level_m` (0–3) | Used by auto control on tank 2 |
| `set_valve` | `valve_pct` (0–100) | **manual mode only** |

## Expected result

| Check | Pass when |
|---|---|
| Process starts | No connection error; MQTT connected line printed |
| Telemetry | `twin/telemetry/tank` ~2 Hz with valid envelope |
| Status | `twin/status/sim` is `online` |
| Accept | Valid `set_valve` → `twin/event` accepted |
| Reject | Bad command → `twin/event` rejected (simulator stays up) |

## Troubleshooting

### All platforms

| Problem | What to try |
|---|---|
| `cannot connect to broker` | Start Lab 01: `cd labs/docker` then `docker compose up -d` |
| No telemetry in MQTTX | Confirm subscribe `twin/#`; confirm simulator terminal is still running |
| `ModuleNotFoundError: paho` | Activate `.venv` and re-run `pip install -r requirements.txt` |
| Levels not moving | Send `set_valve` with a higher value, or switch to `auto` + setpoint |

### Windows (PowerShell 7+)

| Problem | What to try |
|---|---|
| `python` not found | Reinstall Python with **Add to PATH**, or use `py -3.11` |
| Cannot activate venv | `Set-ExecutionPolicy -Scope CurrentUser RemoteSigned` then activate again |
| Wrong shell | Use **PowerShell 7+** (`pwsh --version`), not Windows PowerShell 5 / CMD |

### macOS / Linux

| Problem | What to try |
|---|---|
| `python` missing | Use `python3` / `python3 -m venv .venv` |
| Permission errors on venv | Avoid `sudo pip`; stay inside `.venv` |

## Checkpoint (before Lab 03)

- [ ] Correct terminal (Windows: PowerShell 7+)
- [ ] Lab 01 broker still running
- [ ] Simulator running and publishing telemetry
- [ ] MQTTX shows envelope fields + increasing `seq`
- [ ] Accepted command works
- [ ] Rejected command works without crashing
- [ ] (Optional) Auto mode moves `level2_m` toward setpoint

When finished, keep **broker + simulator** running for Lab 03 (`labs/godot/`).
