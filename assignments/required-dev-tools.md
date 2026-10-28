# Software Preparation

Install and verify all software below **before the first lab session**. Bring your laptop to class — installations and troubleshooting will not be done during class time.

This guide covers **Windows**, **macOS**, and **Linux**.

## Required software

| Software | Purpose in this course | Who needs it |
|---|---|---|
| Docker | Local MQTT broker and lab services | All |
| MQTTX | MQTT client for inspecting telemetry | All |
| Python | Process models and controllers | All |
| Node.js | Tooling and agent integrations | All |
| Git | Version control | All |
| **PowerShell 7+** | Terminal for all lab commands | **Windows only** |
| Terminal / bash / zsh | Terminal for lab commands | macOS / Linux (built-in) |

## Terminal by OS (important)

| OS | Use this | Do **not** use |
|---|---|---|
| **Windows** | **PowerShell 7+** (`pwsh`) | Windows PowerShell 5.x, Command Prompt |
| **macOS** | Terminal.app or iTerm2 | — |
| **Linux** | bash or zsh | — |

Lab commands (`docker compose`, `cd`, Python/venv) are written to work in these terminals.

## 1. Docker

Install **Docker Desktop** (macOS / Windows) or **Docker Engine** (Linux).

- Download: <https://www.docker.com/products/docker-desktop/>
- **macOS:** choose the correct chip — Apple silicon (M1/M2/M3/M4) or Intel.
- **Windows:** requires WSL 2; the installer will prompt you to enable it if needed.
- **Linux:** follow <https://docs.docker.com/engine/install/> for your distribution.

Verify after installation (start Docker Desktop first on Windows/macOS):

```bash
docker --version
docker run hello-world
```

`hello-world` must print "Hello from Docker!".

## 2. MQTTX

MQTTX is a desktop MQTT 5.0 client used to inspect broker traffic.

- Download: <https://mqttx.app/>
- Install the desktop app for your OS (a CLI is also available but not required).

Verify: launch MQTTX and confirm you can create a new connection profile.

## 3. Python

Install **Python 3.11 or newer**.

- Download: <https://www.python.org/downloads/>
- **Windows:** tick **"Add python.exe to PATH"** during installation.
- **macOS:** use the python.org installer or `brew install python`.
- **Linux:** use your distribution's package manager (e.g. `sudo apt install python3 python3-pip python3-venv`).

Verify:

```bash
python --version    # Windows (PowerShell 7+)
python3 --version   # macOS / Linux
pip --version       # or: pip3 --version
```

## 4. Node.js

Install the **LTS** version of Node.js.

- Download: <https://nodejs.org/> (choose the LTS release)
- Alternatively on macOS: `brew install node`.
- Alternatively on Linux: use your distro packages or NodeSource/nvm as preferred.

Verify:

```bash
node --version
npm --version
```

## 5. PowerShell 7+ (Windows only)

Windows students **must** use **PowerShell 7 or newer** for all lab work in this course.

- Download: <https://learn.microsoft.com/powershell/scripting/install/installing-powershell-on-windows>
- Or with winget:

```powershell
winget install --id Microsoft.PowerShell -e
```

- Open **PowerShell 7** from the Start menu (not “Windows PowerShell”).

Verify:

```powershell
pwsh --version
# Expected: PowerShell 7.x.x (or higher)
```

macOS and Linux users already have a suitable shell and can skip this section.

## 6. Git (all OS)

Install Git so you can clone and update the course repository.

- **Windows:** <https://gitforwindows.org/> (Git for Windows)
- **macOS:** `xcode-select --install` or `brew install git`
- **Linux:** e.g. `sudo apt install git`

Verify (in your course terminal):

```bash
git --version
```

## Verification checklist

Run every check below on your machine before class:

| Check | Command / action | Expected result |
|---|---|---|
| Terminal (Windows) | `pwsh --version` | PowerShell **7.x** or newer |
| Terminal (macOS/Linux) | open Terminal / shell | ready to run commands |
| Docker installed | `docker --version` | `Docker version 26.x` or newer |
| Docker running | `docker run hello-world` | "Hello from Docker!" |
| MQTTX installed | launch the app | Connection screen opens |
| Python installed | `python --version` or `python3 --version` | `Python 3.11` or newer |
| pip installed | `pip --version` or `pip3 --version` | prints a version |
| Node.js installed | `node --version` | `v20.x` or newer (LTS) |
| npm installed | `npm --version` | prints a version |
| Git installed | `git --version` | `git version 2.x` |

If any check fails, reinstall that software and try again. If it still fails, ask the instructor **before** the lab session.
