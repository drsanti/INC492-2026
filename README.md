# INC492 — Special Topic II (2026)

**AI-Driven Hardware-in-the-Loop with Digital Twins for Embedded Systems and Industrial Automation**

Course materials for INC492. Start here for assessment dates, practice playlists, the teaching schedule, and topic plans.

## Quick links

| Document | Description |
|---|---|
| [Student learning plan](student-learning-plan.md) | Weekly practice (Blender → Godot), deliverables, and quiz scope |
| [Quiz 1 — Blender (example)](quizzes/quiz1-example_blender.md) | ตัวอย่างโจทย์ Quiz 1: PCB + resistor + capacitor |
| [Quiz 1 template](quizzes/quiz1_template.blend) | ไฟล์ `.blend` เริ่มต้นที่ต้องใช้ในการสอบเท่านั้น |
| [Teaching schedule](teaching-schedule.md) | 16-topic engineering curriculum and project data flow |
| [Topics](topics/) | Detailed class plans for Topics 01–16 |
| [Course description](course-description.md) | Full course description (English / Thai) |

## Course at a glance

| Item | Detail |
|---|---|
| Course code | INC492 |
| Title | Special Topic II |
| Year | 2026 |
| Format | 16 topics · one 3-hour class per topic |
| Tools | Blender, Godot, Python, MQTT |
| Focus | Simulated process/instrumentation, deterministic control, Godot digital twin, AI-agent supervision |

```text
Simulated process and instruments
  -> deterministic control
  -> Godot digital twin / operator UI
  -> AI-agent supervision (validated commands only)
```

## Quiz dates (important)

All quizzes are held at the same time and place:

| | |
|---|---|
| **Time** | 09:30–11:30 |
| **Classroom** | CB40610 |

| Quiz | Date | Weight | Focus |
|---|---|---:|---|
| Quiz 1 | **2 September 2026** | 25% (เต็ม 100 คะแนน) | PCB + resistor + capacitor ใน Blender |
| Quiz 2 | **7 October 2026** | 35% | Godot digital-twin scene, scripting, and asset workflow |
| Quiz 3 | **11 November 2026** | 40% | Data telemetry and AI integration (Weeks 10–13) |

**Total: 100%.**

## Student calendar overview

| Weeks | Mode | Goal |
|---:|---|---|
| 1–3 | Self practice | Complete the Blender playlist and build simple engineering models |
| 4 | Quiz 1 | **2 Sep 2026** · 09:30–11:30 · CB40610 · 25% |
| 5–8 | Self practice | Complete the Godot playlist and assemble a digital-twin scene |
| 9 | Quiz 2 | **7 Oct 2026** · 09:30–11:30 · CB40610 · 35% |
| 10–13 | In class | Data telemetry and AI integration |
| 14 | Quiz 3 | **11 Nov 2026** · 09:30–11:30 · CB40610 · 40% |

Details and episode lists: [student-learning-plan.md](student-learning-plan.md)

## Required playlists

| Tool | Playlist | Link |
|---|---|---|
| Blender | INC111-2021 (11 episodes) | [YouTube](https://www.youtube.com/watch?v=V0gtC1EGuX0&list=PLBPFpqyTjzeVCRoOEIDrqF07M8cXTfIXY) |
| Godot | Digital Twin: Godot+Blender (15 episodes) | [YouTube](https://www.youtube.com/watch?v=omxoBna0mQg&list=PLBPFpqyTjzeVOlpJCVfwJpBwCGZvXoce1) |

## Topics index

| Topic | Title |
|---:|---|
| 1 | [Digital Twins, SIL, and HIL](topics/topic-01-digital-twin-sil-hil.md) |
| 2 | [Architecture and Requirements](topics/topic-02-architecture-and-requirements.md) |
| 3 | [Blender Modeling for Engineering Visualization](topics/topic-03-blender-modeling.md) |
| 4 | [Blender-to-Godot Workflow](topics/topic-04-blender-to-godot.md) |
| 5 | [Godot Scenes and Engineering UI](topics/topic-05-godot-scenes-and-ui.md) |
| 6 | [State-Driven Visualization](topics/topic-06-state-driven-visualization.md) |
| 7 | [Dynamic Process Modeling in Python](topics/topic-07-dynamic-process-modeling.md) |
| 8 | [Feedback Control](topics/topic-08-feedback-control.md) |
| 9 | [Sensor and Actuator Models](topics/topic-09-sensor-and-actuator-models.md) |
| 10 | [MQTT Communication](topics/topic-10-mqtt-communication.md) |
| 11 | [Real-Time Twin Synchronization](topics/topic-11-real-time-synchronization.md) |
| 12 | [Supervisory Control and Safety](topics/topic-12-supervisory-control.md) |
| 13 | [Fault Injection and Test Scenarios](topics/topic-13-fault-injection.md) |
| 14 | [AI Agent Tools and Structured Actions](topics/topic-14-ai-agent-tools.md) |
| 15 | [Agent-Driven Diagnosis and Testing](topics/topic-15-agent-diagnosis-and-testing.md) |
| 16 | [Validation and Final Demonstration](topics/topic-16-validation-and-demonstration.md) |

Full curriculum table: [teaching-schedule.md](teaching-schedule.md)

## Course project data flow

```text
AI agent -> validated supervisory command -> deterministic controller
         -> simulated actuator and process -> simulated sensor telemetry
         -> MQTT -> Godot digital twin and operator interface
         -> telemetry history and alarms -> AI agent
```

The AI agent works at the supervisory level. Fast feedback control, safety limits, and emergency behavior remain deterministic.

## Repository layout

```text
INC492-2026/
├── README.md                    ← course index (this file)
├── course-description.md        ← EN/TH course description
├── student-learning-plan.md     ← practice calendar + quizzes
├── teaching-schedule.md         ← 16-topic curriculum
├── quizzes/
│   ├── quiz1-example_blender.md ← ตัวอย่างโจทย์ Quiz 1 (เต็ม 100 คะแนน = 25% ของวิชา)
│   ├── quiz1_template.blend     ← ไฟล์เริ่มต้นบังคับสำหรับ Quiz 1
│   └── images/                  ← ภาพตัวอย่างผลลัพธ์
└── topics/                      ← detailed topic plans
    ├── README.md
    ├── topic-01-...
    └── ...
```

## Software to install

| Software | Purpose |
|---|---|
| Blender | 3D equipment modeling |
| Godot (with C# / .NET support) | Digital twin and operator UI |
| Python 3 | Process model and control |
| MQTT broker client tools | Telemetry inspection |
| Git + code editor (VS Code recommended) | Version control and scripting |

## Course description (short)

Students build simulated industrial processes, deterministic controllers, Godot-based digital twins, and AI supervisory agents that inspect telemetry and propose guarded actions—without replacing safety-critical control loops.

Full bilingual text: [course-description.md](course-description.md)
