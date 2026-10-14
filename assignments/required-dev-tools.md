# Software Preparation

Install and verify all software below **before the first lab session**. Bring your laptop to class — installations and troubleshooting will not be done during class time.

## Required software

| Software | Purpose in this course |
|---|---|
| Docker | Local MQTT broker and lab services |
| MQTTX | MQTT client for inspecting telemetry |
| Python | Process models and controllers |
| Node.js | Tooling and agent integrations |
| Git Bash (**Windows only**) | Unix-style terminal and Git for lab work |

## 1. Docker

Install **Docker Desktop** (macOS / Windows) or **Docker Engine** (Linux).

- Download: <https://www.docker.com/products/docker-desktop/>
- **macOS:** choose the correct chip — Apple silicon (M1/M2/M3/M4) or Intel.
- **Windows:** requires WSL 2; the installer will prompt you to enable it if needed.
- **Linux:** follow <https://docs.docker.com/engine/install/> for your distribution.

Verify after installation (start Docker Desktop first):

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
python --version    # Windows
python3 --version   # macOS / Linux
pip --version       # or: pip3 --version
```

## 4. Node.js

Install the **LTS** version of Node.js.

- Download: <https://nodejs.org/> (choose the LTS release)
- Alternatively on macOS: `brew install node`.

Verify:

```bash
node --version
npm --version
```

## 5. Git Bash (Windows only)

Windows users must install **Git for Windows**, which includes **Git Bash** — a Unix-style terminal. All terminal commands in this course (`cd`, `ls`, `docker`, `python`) are written for bash; use Git Bash instead of Command Prompt or PowerShell so the commands work as written.

- Download: <https://gitforwindows.org/>
- During installation, keep the default options; make sure **Git Bash Here** is enabled.
- macOS and Linux users already have a suitable terminal and can skip this section.

Verify: open **Git Bash** and run:

```bash
git --version
```

## Verification checklist

Run every command below and confirm the output before class:

| Check | Command | Expected result |
|---|---|---|
| Docker installed | `docker --version` | `Docker version 26.x` or newer |
| Docker running | `docker run hello-world` | "Hello from Docker!" |
| MQTTX installed | launch the app | Connection screen opens |
| Python installed | `python --version` or `python3 --version` | `Python 3.11` or newer |
| pip installed | `pip --version` or `pip3 --version` | prints a version |
| Node.js installed | `node --version` | `v20.x` or newer (LTS) |
| npm installed | `npm --version` | prints a version |
| Git Bash installed (Windows) | open Git Bash, run `git --version` | `git version 2.x` |

If any check fails, reinstall that software and try again. If it still fails, ask the instructor **before** the lab session.
