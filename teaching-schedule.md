# Teaching Schedule

**Course format:** 16 topics · one 3-hour class per topic  
**Implementation path:** Simulated process and instrumentation → deterministic control → Godot digital twin → AI-agent supervision

This document is the **engineering curriculum outline**.  
Student calendar, playlists, and quiz dates are in [student-learning-plan.md](student-learning-plan.md).  
Detailed class plans are in [topics/](topics/).

## How this relates to the student calendar

| Student weeks | Student focus | Related topics |
|---:|---|---|
| 1–3 | Blender self-practice | Topics 1–3 (concepts + modeling foundation) |
| 4 | Quiz 1 | Blender assessment |
| 5–8 | Godot self-practice | Topics 4–6 (asset pipeline, scenes, state-driven views) |
| 9 | Quiz 2 | Godot assessment |
| 10–13 | Telemetry and AI in class | Topics 7–15 (process, control, MQTT, safety, AI) |
| 14 | Quiz 3 | Integration assessment |

Topics remain the technical roadmap even when early weeks emphasize self-paced Blender/Godot practice.

## Learning path

| Phase | Topics | Focus |
|---|---:|---|
| Foundations | 1–2 | Digital-twin concepts, project scope, and system architecture |
| Visualization twin | 3–6 | Blender modeling, Godot scenes, and state-driven operator views |
| Process and control | 7–9 | Dynamic models, feedback control, and realistic I/O behavior |
| Integration | 10–11 | MQTT exchange and real-time twin synchronization |
| Supervision and AI | 12–15 | Safety-gated commands, fault experiments, and agent diagnosis |
| Capstone | 16 | End-to-end validation and final demonstration |

## Topic schedule

| Topic | Engineering focus | Teaching and learning outcome |
|---:|---|---|
| 1 | Digital twins, Model-in-the-Loop, Software-in-the-Loop, virtual HIL, and physical HIL | Select an industrial process and define its purpose, system boundary, variables, simulated instruments, candidate faults, and AI-agent role. |
| 2 | System architecture and engineering requirements | Produce an architecture diagram, signal dictionary, sampling plan, operating modes, MQTT topics, safety limits, and measurable acceptance tests. |
| 3 | Blender modeling for engineering visualization | Create correctly scaled, low-complexity 3D models of the principal process equipment and separate all moving or state-dependent parts. |
| 4 | Blender-to-Godot asset workflow | Export models as GLB, import them into Godot with correct scale and orientation, and package reusable equipment scenes. |
| 5 | Godot scenes and engineering user interfaces | Assemble the process environment and operator display with values, units, setpoints, actuator outputs, modes, alarms, and connection status. |
| 6 | State-driven 3D visualization | Map engineering variables to equipment transforms, animations, materials, labels, and alarm states using repeatable test data. |
| 7 | Dynamic process modeling in Python | Implement and verify a physical, transfer-function, or state-space process model with open-loop step and disturbance experiments. |
| 8 | Discrete feedback control | Integrate PID or state feedback with sample time, saturation, anti-windup, and manual/automatic modes; evaluate closed-loop performance. |
| 9 | Simulated sensors and actuators | Add sensor noise, bias, drift, quantization, and sampling, plus actuator delay, deadband, rate limits, and saturation. |
| 10 | MQTT and structured data exchange | Connect Python and Godot through versioned telemetry and command messages with timestamps, sequence numbers, validation, and reconnection handling. |
| 11 | Real-time twin synchronization | Detect stale, delayed, duplicated, and out-of-order data; implement visual interpolation and measure end-to-end update latency. |
| 12 | Supervisory control and deterministic safety | Send setpoints, mode changes, and equipment commands through a validation gateway with interlocks, acknowledgments, and an audit log. |
| 13 | Fault injection and experiment design | Run repeatable sensor, actuator, process, and communication fault scenarios and produce labeled datasets without exposing fault labels to detectors. |
| 14 | AI-agent tools and structured actions | Give the agent restricted tools to inspect telemetry, history, and alarms and to propose guarded supervisory actions through a defined schema. |
| 15 | Agent-driven diagnosis and testing | Run blind scenarios in which the agent forms fault hypotheses, requests safe tests, recommends actions, and verifies recovery against a conventional baseline. |
| 16 | System validation and final demonstration | Demonstrate normal control, synchronized visualization, fault injection, deterministic detection, AI-agent diagnosis, guarded action, verification, and unsafe-command rejection. |

## Course project data flow

```text
AI agent -> validated supervisory command -> deterministic controller
         -> simulated actuator and process -> simulated sensor telemetry
         -> MQTT -> Godot digital twin and operator interface
         -> telemetry history and alarms -> AI agent
```

The AI agent operates at the supervisory level. Fast feedback control, safety limits, and emergency behavior remain deterministic.

## Assessment note

Formal course grading follows the three quizzes in [student-learning-plan.md](student-learning-plan.md) (25% + 35% + 40%).  
Topic 16 includes a demonstration rubric for evaluating engineering completeness during the final integration assessment.
