# System Architecture

Reference architecture for the course project. The system has three runtime components:

1. **Python application** — simulates the physical process, sensors, actuators, and the deterministic controller.
2. **MQTT broker** — runs in Docker; carries all messages between components.
3. **Godot application (C#)** — the digital twin: 3D visualization of the process and the operator interface (HMI).

```text
┌──────────────────────┐        ┌──────────────────────┐        ┌──────────────────────┐
│   Python simulator   │        │    MQTT broker       │        │   Godot digital twin │
│                      │        │    (Docker)          │        │        (C#)          │
│  process model       │  pub   │                      │  sub   │  3D process scene    │
│  sensors/actuators   │───────▶│  twin/telemetry      │───────▶│  HMI / operator UI   │
│  deterministic       │        │  twin/command        │        │  alarms, trends      │
│  control + safety    │◀───────│  twin/event          │◀───────│  connection status   │
│                      │  sub   │  twin/status         │  pub   │                      │
└──────────────────────┘        └──────────┬───────────┘        └──────────────────────┘
                                           │
                                           │  observe (any topic)
                                    ┌──────▼───────────┐
                                    │  MQTTX client    │
                                    │  (debug/inspect) │
                                    └──────────────────┘
```

## Components

| Component | Technology | Responsibilities |
|---|---|---|
| Process simulator | Python 3, `paho-mqtt` | Process dynamics, sensor models (noise, bias, quantization), actuator models, PID/state feedback, safety limits. Publishes telemetry; executes validated commands. |
| MQTT broker | Eclipse Mosquitto in Docker | Message routing only. No application logic in the broker. |
| Digital twin / HMI | Godot 4 (.NET build), C#, MQTTnet | 3D visualization driven by telemetry, operator displays (values, units, setpoints, alarms, connection status), sends operator commands. |
| Inspection tool | MQTTX | Manual publish/subscribe for debugging and evidence capture. |

### Ownership rules

- **Python owns the process state.** Godot never modifies simulated variables directly — it may only send commands, which Python validates against interlocks and limits.
- **Safety is deterministic.** Limits, interlocks, and emergency behavior live in the Python controller, not in the UI and not in any AI agent.
- **The broker is dumb transport.** If the broker restarts, components must reconnect and resume without manual steps.

## MQTT broker in Docker

Run a local Mosquitto broker. Example `docker-compose.yml`:

```yaml
services:
  mosquitto:
    image: eclipse-mosquitto:2
    container_name: twin-mqtt
    ports:
      - "1883:1883"   # MQTT over TCP
    volumes:
      - ./mosquitto/mosquitto.conf:/mosquitto/config/mosquitto.conf
    restart: unless-stopped
```

Minimal `mosquitto/mosquitto.conf` (local development only — no authentication):

```conf
listener 1883 0.0.0.0
allow_anonymous true
persistence true
persistence_location /mosquitto/data/
```

Start it:

```bash
docker compose up -d
docker compose ps        # broker should be "running"
```

Both clients connect to `localhost:1883` (or the host's LAN IP if run on different machines).

## Communication contract

### Topic namespace

| Topic | Direction | QoS | Retained | Purpose |
|---|---|---|---|---|
| `twin/telemetry/<group>` | Python → Godot | 0 | no | Periodic process data (e.g. `twin/telemetry/tank`) |
| `twin/command/<target>` | Godot → Python | 1 | no | Operator commands and setpoints |
| `twin/event` | Python → Godot | 1 | yes | Alarms, mode changes, fault flags |
| `twin/status/<component>` | any → any | 1 | yes | Online/offline state; use MQTT Last Will for `offline` |

Telemetry uses QoS 0 because a missed sample is replaced by the next one within one period. Commands and events use QoS 1 because they must not be lost.

### Message envelope

Every message — telemetry, command, or event — uses this envelope:

```json
{
  "schema_version": "1.0",
  "source": "sim/python",
  "timestamp_utc": "2026-10-14T09:30:00.123Z",
  "seq": 1042,
  "payload": { }
}
```

| Field | Rule |
|---|---|
| `schema_version` | Bump on any breaking payload change. Receivers must reject unknown versions. |
| `source` | Stable sender identifier, e.g. `sim/python`, `hmi/godot`. |
| `timestamp_utc` | ISO 8601 UTC, millisecond precision — the time the data was *produced*. |
| `seq` | Per-source increasing counter. Receivers use it to detect drops, duplicates, and reordering. |
| `payload` | Message-type-specific content (see below). |

### Telemetry payload example

Published to `twin/telemetry/tank` at a fixed rate (e.g. 2–10 Hz, defined in the signal dictionary):

```json
{
  "schema_version": "1.0",
  "source": "sim/python",
  "timestamp_utc": "2026-10-14T09:30:00.123Z",
  "seq": 1042,
  "payload": {
    "level_m": 1.42,
    "inflow_lpm": 12.5,
    "valve_pct": 45.0,
    "controller_mode": "auto",
    "quality": "good"
  }
}
```

### Command payload example

Published to `twin/command/controller` when the operator changes a setpoint:

```json
{
  "schema_version": "1.0",
  "source": "hmi/godot",
  "timestamp_utc": "2026-10-14T09:30:05.000Z",
  "seq": 7,
  "payload": {
    "command_id": "cmd-20261014-0007",
    "action": "set_setpoint",
    "params": { "level_m": 1.8 },
    "requested_by": "operator"
  }
}
```

Python validates the command, applies it if legal, and publishes the outcome on `twin/event` (accepted/rejected + reason). An HMI must treat a command as unconfirmed until the event arrives.

## Signal dictionary (excerpt)

Every signal in telemetry must appear in the project's signal dictionary with these fields — full requirements are in Topic 2:

| Signal | Unit | Range | Update rate | Owner | Stale behavior |
|---|---|---|---|---|---|
| `level_m` | m | 0.0–3.0 | 2 Hz | sim/python | mark `quality="stale"`, show greyed-out value |
| `valve_pct` | % | 0–100 | 2 Hz | sim/python | hold last valid, flag stale |
| `controller_mode` | enum | `manual`, `auto` | on change | sim/python | event, retained |

## Sequence of operation

1. `docker compose up -d` — broker starts.
2. Python simulator starts → connects → publishes `twin/status/sim = online` (retained, with LWT set to `offline`) → begins telemetry loop.
3. Godot starts → connects → subscribes `twin/telemetry/#`, `twin/event`, `twin/status/#` → scene enters "connected" state.
4. Operator interacts with the HMI → Godot publishes to `twin/command/<target>` → Python validates → result on `twin/event`.
5. If telemetry stops arriving within 2–3× the expected period, Godot marks data stale and shows the disconnected/stale visual state — this is assessed in Topic 11.

## Suggested project layout

```text
project/
├── docker-compose.yml
├── mosquitto/
│   └── mosquitto.conf
├── sim/                     ← Python application
│   ├── pyproject.toml
│   └── src/
│       ├── main.py          ← entry point, MQTT loop
│       ├── process.py       ← dynamic model
│       ├── controller.py    ← PID + safety limits
│       └── mqtt_client.py   ← envelope, seq, validation
└── twin/                    ← Godot project (.NET)
    ├── project.godot
    └── scripts/             ← C#: MQTT client, state mapping, HMI
```

## Testing and evidence

- Use **MQTTX** to subscribe to `twin/#` and confirm envelope fields, units, and rates.
- Publish a malformed message to `twin/command/#` from MQTTX — the simulator must reject it without crashing.
- Stop and restart the broker — both clients must reconnect on their own.
- Kill the simulator (no graceful disconnect) — `twin/status/sim` must flip to `offline` via the Last Will.

## Later extensions (Topics 12–15)

The AI agent joins as another MQTT client: it subscribes to telemetry and events, and proposes actions on `twin/agent/action` through the same validation gateway as operator commands. No changes to this base architecture are required.
