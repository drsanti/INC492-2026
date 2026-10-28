# Lab 01 — Start MQTT with Docker

Bring up the course MQTT broker so later labs (Python simulator, Godot twin) can talk through `localhost:1883`.

**MQTTX is for testing only** — it is not part of the running system.

This lab is written for **Windows**, **macOS**, and **Linux**. Use the terminal for your OS (see below). The `docker compose` commands are the same on all three platforms.

## Goal

- Run Eclipse Mosquitto in Docker
- Connect from MQTTX
- Publish and subscribe on a test topic
- Subscribe to `twin/#` (ready for Lab 02)

## Prerequisites

Complete [assignments/required-dev-tools.md](../../assignments/required-dev-tools.md):

| Requirement | Windows | macOS | Linux |
|---|---|---|---|
| Docker | Docker Desktop (WSL 2) | Docker Desktop (Apple silicon or Intel) | Docker Engine or Docker Desktop |
| MQTTX | Desktop app | Desktop app | Desktop app |
| Terminal for this course | **PowerShell 7+** (`pwsh`) | Terminal.app or iTerm | bash / zsh |

Reference architecture: [assignments/system-architecture.md](../../assignments/system-architecture.md)

### Which terminal to open

| OS | Open this | Verify |
|---|---|---|
| **Windows** | **PowerShell 7+** (not Windows PowerShell 5.x, not CMD) | `pwsh --version` → 7.x or newer |
| **macOS** | Terminal (or iTerm2) | `uname -s` → `Darwin` |
| **Linux** | Your normal shell (bash/zsh) | `uname -s` → `Linux` |

**Windows — install PowerShell 7+ if needed**

- Download: <https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-windows>
- Or with winget: `winget install --id Microsoft.PowerShell -e`
- After install, open **PowerShell 7** from the Start menu (icon often labeled “PowerShell 7”).

## Files in this folder

| File | Purpose |
|---|---|
| `docker-compose.yml` | Starts Mosquitto and maps port 1883 |
| `mosquitto/mosquitto.conf` | Local broker config (anonymous access) |

## Steps

### 1. Open a terminal and go to this folder

Clone or open the course repo first, then:

**Windows (PowerShell 7+)**

```powershell
cd <path-to-repo>\INC492-2026\labs\docker
```

Example if the repo is on drive D:

```powershell
cd D:\temp\INC492-2026\labs\docker
```

**macOS / Linux**

```bash
cd <path-to-repo>/INC492-2026/labs/docker
```

Confirm you are in the right place (you should see `docker-compose.yml`):

| OS | Command |
|---|---|
| Windows (PowerShell 7+) | `Get-ChildItem` or `ls` |
| macOS / Linux | `ls` |

### 2. Start Docker

| OS | What to do |
|---|---|
| Windows | Start **Docker Desktop** and wait until it shows Running |
| macOS | Start **Docker Desktop** and wait until the whale icon is idle/running |
| Linux | Ensure the Docker service is running (`sudo systemctl start docker` if needed; your user may need the `docker` group) |

Quick check (all OS):

```bash
docker version
docker run hello-world
```

On Linux, if you see a permission error, either use `sudo` as your distro docs require or fix Docker group membership.

### 3. Start the broker

All OS — same command:

```bash
docker compose up -d
```

First run downloads `eclipse-mosquitto:2` (needs network).

### 4. Confirm the container is running

```bash
docker compose ps
```

Expected: service `mosquitto` / container `twin-mqtt` with state **running** (or `Up`), ports `0.0.0.0:1883->1883/tcp`.

Optional logs:

```bash
docker compose logs -f mosquitto
```

Press `Ctrl+C` to stop following logs (the broker keeps running).

### 5. Connect with MQTTX

Works the same on Windows, macOS, and Linux:

1. Open **MQTTX**
2. New connection:
   - **Name:** `INC492 local`
   - **Host:** `localhost` (or `127.0.0.1`)
   - **Port:** `1883`
   - **Protocol:** `mqtt://`
3. Connect — status should show connected

### 6. MQTTX check A — manual pub/sub

1. Subscribe to topic: `lab01/hello` (QoS 0 is fine)
2. Publish to `lab01/hello` with payload: `ping from MQTTX`
3. You must see the message arrive in the subscription panel

### 7. MQTTX check B — course topic namespace

1. Subscribe to: `twin/#`
2. Leave this subscription open for Lab 02 (Python telemetry will appear here later)
3. Optional: publish a one-off test message to `twin/status/lab01` with payload:

```json
{"schema_version":"1.0","source":"lab01/mqttx","payload":{"note":"broker ok"}}
```

You should see it under the `twin/#` subscription.

## Expected result

| Check | Pass when |
|---|---|
| Terminal | Windows: PowerShell 7+ · macOS/Linux: normal shell |
| Docker | `docker compose ps` shows `twin-mqtt` running |
| Port | MQTTX connects to `localhost:1883` without error |
| Pub/sub | Message on `lab01/hello` is received |
| Namespace | Subscription to `twin/#` is active |

## Useful commands

Same on Windows (PowerShell 7+), macOS, and Linux:

| Action | Command |
|---|---|
| Start | `docker compose up -d` |
| Status | `docker compose ps` |
| Logs | `docker compose logs -f mosquitto` |
| Stop | `docker compose down` |
| Stop and remove volume | `docker compose down -v` |

## Troubleshooting by OS

### All platforms

| Problem | What to try |
|---|---|
| `docker compose` not found | Update Docker Desktop / Engine; try `docker-compose` (hyphen) only if Compose V2 is missing |
| Port 1883 already in use | Stop the other MQTT/broker process, or change the left side of `"1883:1883"` in `docker-compose.yml` (e.g. `"1884:1883"`) and use that host port in MQTTX |
| MQTTX connection refused | Confirm Docker is running and `docker compose ps` shows Up; wait a few seconds after `up -d` |
| Container exits immediately | `docker compose logs mosquitto` — usually a bad path; make sure your terminal’s current directory is `labs/docker` |

### Windows

| Problem | What to try |
|---|---|
| Using Windows PowerShell 5.x by mistake | Open **PowerShell 7** (`pwsh --version` must be 7+) |
| Docker Desktop not starting | Enable WSL 2; update WSL (`wsl --update`); restart PC |
| Volume / file share errors | In Docker Desktop → Settings → Resources → File sharing, allow the drive that holds the repo |
| `docker` not recognized in PowerShell 7 | Restart the terminal after installing Docker Desktop; confirm Docker Desktop is running |

### macOS

| Problem | What to try |
|---|---|
| Wrong Docker build | Install the Apple silicon or Intel build that matches your Mac |
| Permission / VirtioFS issues | Update Docker Desktop; reset file sharing if the compose mount fails |

### Linux

| Problem | What to try |
|---|---|
| Permission denied talking to Docker | Add your user to the `docker` group, then log out/in — or follow your distro’s Docker Engine docs |
| Firewall blocking localhost | Usually not needed for `localhost:1883`; if MQTTX is on another machine, open port 1883 carefully (lab default is local-only anonymous — do not expose publicly) |

## Checkpoint (before Lab 02)

- [ ] Correct terminal open (Windows: **PowerShell 7+** · macOS/Linux: Terminal/shell)
- [ ] Broker running (`twin-mqtt`)
- [ ] MQTTX connected to `localhost:1883`
- [ ] Pub/sub on `lab01/hello` works
- [ ] Subscribed to `twin/#`
- [ ] Know how to `docker compose down` and start again

When this checklist is done, keep the broker running and go to **Lab 02** (`labs/simulator/`).
