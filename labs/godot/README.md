# Lab 03 — Godot digital twin (C#)

Turn live MQTT telemetry from Lab 02 into a **3D coupled-tank twin** plus an operator HMI.

Godot **does not simulate the process**. It only:

- displays `level1_m`, `level2_m`, `valve_pct`, mode, setpoint
- sends commands on `twin/command/controller`

**MQTTX remains test-only.**

## Goal

- Open the Godot .NET project
- Connect to `localhost:1883`
- See liquids and valve move with telemetry
- Send valve / setpoint / mode commands from the HMI
- Confirm `twin/event` accept/reject in the HMI (and optionally MQTTX)

## Prerequisites

| Requirement | Notes |
|---|---|
| Lab 01 | Broker running — [labs/docker](../docker/README.md) |
| Lab 02 | Simulator running — [labs/simulator](../simulator/README.md) |
| Lab 03A (optional) | MQTT→Godot warmup — [labs/godot-simple](../godot-simple/README.md) |
| Godot **4.x .NET** (C#) | Same major line as this project (created as 4.7) |
| .NET SDK 8+ | Required for Godot C# builds |
| Terminal | **Windows: PowerShell 7+** · macOS/Linux: Terminal |

If you are new to Godot C# and MQTT, complete **Lab 03A** first. It covers subscribe, JSON, main-thread marshalling, and smooth `lerp`/`slerp` on two demo boxes.

## What the scene contains

About **8** primary 3D objects:

| Object | Driven by |
|---|---|
| Floor | static |
| Tank 1 shell | static |
| Tank 1 liquid | `level1_m` → height |
| Tank 2 shell | static |
| Tank 2 liquid | `level2_m` → height |
| Coupling pipe | static |
| Inlet valve | `valve_pct` → rotation + color |
| Setpoint marker (tank 2) | `setpoint_m` → height |

Plus orbit camera, lights, and a 2D HMI panel.

**Camera controls:** left-drag orbit · middle/right-drag pan · mouse wheel zoom

```text
fill_ratio = clamp(level_m / 3.0, 0, 1)
liquid height ∝ fill_ratio
valve angle = lerp(0°, 90°, valve_pct / 100)
```

If telemetry stops for ~2 s, liquids turn grey (**stale**).

## Project layout

```text
labs/godot/
├── project.godot
├── godot1.csproj          ← includes MQTTnet
├── scenes/main.tscn
├── scripts/
│   ├── Main.cs            ← wires MQTT + world + HMI
│   ├── MqttTwinClient.cs
│   ├── TwinWorld.cs       ← builds/updates 3D
│   └── TwinHmi.cs         ← operator UI
└── README.md
```

## Steps

### 1. Start Lab 01 and Lab 02

**Windows (PowerShell 7+)**

```powershell
cd <repo>\INC492-2026\labs\docker
docker compose up -d

cd ..\simulator
.\.venv\Scripts\Activate.ps1
python src\main.py
```

**macOS / Linux**

```bash
cd <repo>/INC492-2026/labs/docker
docker compose up -d

cd ../simulator
source .venv/bin/activate
python src/main.py
```

Keep both running. Optional: MQTTX subscribed to `twin/#`.

### 2. Open the Godot project

1. Launch **Godot .NET** (not the non-.NET build)
2. **Import** / open `labs/godot`
3. Wait for C# solution restore (MQTTnet downloads via NuGet)
4. If prompted to install/create C# solution, accept
5. Open `scenes/main.tscn` (also set as main scene)
6. Press **Play** (F5)

### 3. Verify visuals

When connected you should see:

- HMI status **Broker: connected**
- L1 / L2 numbers updating
- Liquid columns moving slowly
- Valve angle matching `valve_pct`

### 4. Send commands from the HMI

1. Set valve to `80` → **Set Valve (manual)**  
   - Valve rotates; Tank 1 fills faster  
   - Last event should show `accepted`
2. Click **Mode: AUTO**, set setpoint `1.2` → **Set Setpoint**  
   - Yellow marker moves; Tank 2 trends toward setpoint  
   - Valve moves automatically
3. Send an invalid idea via MQTTX (optional): missing params → HMI last event `rejected`

### 5. Stale / offline check

Stop the Lab 02 simulator (`Ctrl+C`). Within ~2 seconds:

- liquids grey out
- HMI shows stale / offline-related status

Start Lab 02 again — visuals recover.

## Expected result

| Check | Pass when |
|---|---|
| Connect | HMI shows connected (Lab 01+02 up) |
| Telemetry | Numbers + liquids update continuously |
| Command | HMI button changes process; event accepted |
| Ownership | Stopping Godot does **not** stop Python levels |
| Stale | Stopping Python greys the twin |

## Troubleshooting

### All platforms

| Problem | What to try |
|---|---|
| Cannot connect | Lab 01 `docker compose ps`; Lab 02 running; host `localhost` port `1883` |
| No C# build | Install .NET SDK 8+; use Godot **.NET** editor |
| MQTTnet missing | Open project once so NuGet restores; check `godot1.csproj` PackageReference |
| Scene empty / script errors | Build the C# project (editor Build button), read Output panel |
| Liquids not moving | Confirm MQTTX still receives `twin/telemetry/tank` |

### Windows (PowerShell 7+)

| Problem | What to try |
|---|---|
| NuGet / firewall | Allow network for first restore |
| Wrong Godot | Uninstall non-.NET Godot if both installed; open `.csproj`-aware editor |

### macOS / Linux

| Problem | What to try |
|---|---|
| `dotnet` not found | Install .NET SDK from https://dotnet.microsoft.com/download |
| Godot can’t find SDK | Restart editor after installing SDK |

## Checkpoint

- [ ] Lab 01 + Lab 02 running
- [ ] Twin opens and connects
- [ ] Liquids track `level1_m` / `level2_m`
- [ ] Valve tracks `valve_pct`
- [ ] HMI commands accepted
- [ ] Stale behaviour verified

## End-to-end stack (Labs 01–03)

```text
Docker Mosquitto  →  Python coupled-tank sim  →  Godot twin + HMI
         ↑                                         |
         └──────── commands / events ──────────────┘
                      MQTTX (inspect only)
```
