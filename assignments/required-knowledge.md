# Required Knowledge and Practical Skills

Self-study topics you must complete **before they are needed in class**. The Blender and Godot playlists are scheduled in [student-learning-plan.md](../student-learning-plan.md) — this document covers the supporting knowledge those playlists do not teach.

**Install the software first:** several self-checks require the tools in [required-dev-tools.md](required-dev-tools.md) (Docker, MQTTX, Python, Node.js, Git Bash on Windows).

Each topic ends with a **self-check**. If you cannot complete the self-check, keep practicing before the deadline.

## Overview

| # | Topic | Needed by | Suggested effort |
|---|---|---|---|
| 1 | Python programming | Topic 7 (process modeling) | 2–3 weekends |
| 2 | C# and Godot scripting | Weeks 5–8 (twin practice) | 1–2 weekends |
| 3 | Command line, Git, and VS Code | All lab work | 1 evening |
| 4 | Docker basics | Topic 10 (MQTT lab) | 1 evening |
| 5 | MQTT and publish/subscribe | Topic 10 | 1 evening |
| 6 | JSON and structured data | Topic 10 | 1 evening |
| 7 | Instrumentation and control basics | Topics 7–9 | 1–2 evenings |

## Topic 1 — Python programming

**Learn:** variables and types, functions, classes and objects, dictionaries, lists, `for`/`while` loops, exceptions, file I/O, creating a virtual environment, installing packages with `pip`.

**Practice:** write a script that runs a loop at a fixed interval (e.g. every 100 ms), updates a value like `level += flow * dt`, and prints the result. Then read the loop parameters from a JSON file.

**Self-check:** without copying, write a ~50-line script that simulates a filling tank (inflow raises level, outflow lowers it) and prints the level every step.

**Resources:** [official Python tutorial](https://docs.python.org/3/tutorial/) — work through at least sections 1–9.

## Topic 2 — C# and Godot scripting

**Learn:** C# syntax, classes, properties, methods, `if`/`for`/`foreach`, and how Godot attaches scripts to nodes (`_Ready`, `_Process`, node references, signals).

**Practice:** in a Godot project, write a C# script that changes a node's rotation, position, or material color when a variable changes. The Godot playlist episodes on C# and VS Code (Week 7) cover the setup.

**Self-check:** drive one visual property of an object from a script variable — e.g. a tank-filling bar whose height follows a `level` variable.

## Topic 3 — Command line, Git, and VS Code

**Learn:** navigating directories in a terminal (`cd`, `ls`/`dir`), running programs (`python`, `node`, `docker`), `git clone` / `add` / `commit` / `push`, and editing code in VS Code.

**Self-check:** clone a repository, run a Python script from the terminal, modify it in VS Code, and commit the change — without a GUI file manager.

## Topic 4 — Docker basics

**Learn:** what images and containers are, `docker run`, `docker ps`, `docker logs`, `docker compose up -d` / `down`, port mapping (`-p 1883:1883`).

**Practice:** start the course MQTT broker using the `docker-compose.yml` in [system-architecture.md](system-architecture.md).

**Self-check:** start Mosquitto in Docker, then connect to `localhost:1883` from MQTTX.

**Resources:** [Docker Get Started](https://docs.docker.com/get-started/) — parts 1–3 are enough.

## Topic 5 — MQTT and publish/subscribe

**Learn:** the broker–client model, topics and wildcards (`#`, `+`), publish vs subscribe, QoS 0/1/2 at a concept level, retained messages, Last Will.

**Practice:** open two MQTTX connections (or MQTTX plus a script): publish on one, receive on the other. Try a retained message and observe a new subscriber receiving it immediately.

**Self-check:** explain, in one sentence each: what a broker does, what happens when two clients subscribe to the same topic, and what QoS 1 guarantees that QoS 0 does not.

## Topic 6 — JSON and structured data

**Learn:** objects, arrays, numbers/strings/booleans/null, nesting, and how to parse and produce JSON in both Python (`json` module) and C# (`System.Text.Json`).

**Practice:** write the telemetry envelope from [system-architecture.md](system-architecture.md) (`schema_version`, `source`, `timestamp_utc`, `seq`, `payload`) as a JSON string, then parse it back and print each field.

**Self-check:** given a JSON message with a missing field, your code detects and rejects it instead of crashing.

## Topic 7 — Instrumentation and control basics

**Learn:** the difference between sensors and actuators; process variable (PV), setpoint (SP), and output; open loop vs closed loop; what a PID controller does at a concept level; engineering units and ranges; measurement noise.

**Self-check:** describe the control loop for a water tank — what is measured, what is manipulated, what the setpoint is, and what happens when the measured level drifts above the setpoint.

## How to study

1. Work through topics in order — later ones assume earlier ones.
2. Every topic has a hands-on part: do it, don't just read.
3. Keep a practice log (date, topic, what you built, blockers) — same habit as the playlist phases.
4. If a self-check takes more than ~30 minutes of guessing, revisit the material or ask before class.
