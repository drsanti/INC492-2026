# Student Learning Plan

Self-paced practice outside class, plus three graded quizzes.

Use this document for **what to practice each calendar week** and **when quizzes happen**.  
For the full 16-topic engineering curriculum, see [teaching-schedule.md](teaching-schedule.md) and [topics/](topics/).

## Quiz dates and weights

All quizzes are held at the same time and place:

| | |
|---|---|
| **Time** | 09:30–11:30 |
| **Classroom** | CB40610 |

| Quiz | Date | Week | Weight | Focus |
|---|---|---:|---:|---|
| Quiz 1 | **2 September 2026** | 4 | 25% | Blender modeling fundamentals and workshop outcomes |
| Quiz 2 | **7 October 2026** | 9 | 35% | Godot digital-twin scene, scripting, and asset workflow |
| Quiz 3 | **11 November 2026** | 14 | 40% | Data telemetry and AI integration (Weeks 10–13) |

Total: **100%**.

## Required playlists

| Tool | Playlist | Link |
|---|---|---|
| Blender | INC111-2021 (11 episodes) | [YouTube playlist](https://www.youtube.com/watch?v=V0gtC1EGuX0&list=PLBPFpqyTjzeVCRoOEIDrqF07M8cXTfIXY) |
| Godot | Digital Twin: Godot+Blender (15 episodes) | [YouTube playlist](https://www.youtube.com/watch?v=omxoBna0mQg&list=PLBPFpqyTjzeVOlpJCVfwJpBwCGZvXoce1) |

## Calendar overview

| Weeks | Mode | Goal |
|---:|---|---|
| 1–3 | Self practice | Complete the Blender playlist and build simple engineering models |
| 4 | Quiz 1 | Blender assessment (25%) — **2 Sep 2026**, 09:30–11:30, CB40610 |
| 5–8 | Self practice | Complete the Godot playlist and assemble a digital-twin scene |
| 9 | Quiz 2 | Godot assessment (35%) — **7 Oct 2026**, 09:30–11:30, CB40610 |
| 10–13 | In class | Data telemetry and AI integration |
| 14 | Quiz 3 | Telemetry/AI assessment (40%) — **11 Nov 2026**, 09:30–11:30, CB40610 |

---

## Phase A — Blender practice (Weeks 1–3)

**Playlist:** [INC111-2021](https://www.youtube.com/watch?v=V0gtC1EGuX0&list=PLBPFpqyTjzeVCRoOEIDrqF07M8cXTfIXY)

### Week 1 — Blender basics

| Episode | Title | Approx. length |
|---|---|---|
| EP01 | Introduction and Assignment | 24 min |
| EP02 | Download and Install | 4 min |
| EP03 | Preference Settings | 6 min |
| EP04 | Viewport Navigation | 29 min |
| EP05 | Selection | 22 min |
| EP06 | Transformation | 39 min |

**Checkpoint**

- Navigate the viewport confidently (orbit, pan, zoom, orthographic views).
- Select, move, rotate, and scale objects with precision.
- Save a starter `.blend` file with a clean scene and engineering-friendly units.

### Week 2 — Edit mode and resistor workshop

| Episode | Title | Approx. length |
|---|---|---|
| EP07 | Working with Edit Mode | 14 min |
| EP08 | Workshop 1.1 Resistor Shape (Body) | 48 min |
| EP09 | Workshop 1.2 Resistor Materials | 24 min |
| EP10 | Workshop 1.3 Resistor Pins (Legs) | 46 min |

**Checkpoint**

- Complete the full resistor model (body, materials, pins).
- Keep objects named clearly and organized in collections.
- Save a screenshot of the finished resistor in your practice log.

### Week 3 — Capacitor workshop and project transfer

| Episode | Title | Approx. length |
|---|---|---|
| EP11 | Workshop 2 Electrolytic Capacitor | 56 min |

Then apply the same workflow to one **simple industrial part** for the course project (for example a tank shell, pipe segment, valve body, or sensor housing).

**Checkpoint**

- Finish the electrolytic capacitor workshop.
- Model one low-complexity process component with correct relative scale.
- Separate any part that will later move or change state in the twin.
- Be ready for Quiz 1.

### Week 4 — Quiz 1 (25%) · 2 September 2026 · 09:30–11:30 · CB40610

**Scope:** Blender fundamentals from Weeks 1–3.

Expect checks on:

- viewport navigation and transforms
- edit-mode modeling
- materials and object organization
- workshop results (resistor / capacitor)
- ability to produce a clean engineering mesh for later Godot import

---

## Phase B — Godot digital twin practice (Weeks 5–8)

**Playlist:** [Digital Twin: Godot+Blender](https://www.youtube.com/watch?v=omxoBna0mQg&list=PLBPFpqyTjzeVOlpJCVfwJpBwCGZvXoce1)

### Week 5 — Godot setup and scene basics

| Episode | Title |
|---|---|
| EP01 | Introduction and Preparation |
| EP02 | System/Environment Testing |
| EP03 | Basic Navigation |
| EP04 | MeshInstance3D and Transformation |

**Checkpoint**

- Install/verify Godot and create a working project.
- Place, transform, and inspect `MeshInstance3D` nodes.
- Build a blank plant-floor scene with camera and lighting.

### Week 6 — Physics, collision, and materials

| Episode | Title |
|---|---|
| EP05 | Project and Editor Settings |
| EP06 | Static Body and Rigid Body |
| EP07 | Collision Shapes |
| EP08 | Materials and Textures |

**Checkpoint**

- Distinguish static vs rigid bodies for equipment vs movable parts.
- Add collision shapes that match simplified equipment geometry.
- Apply materials suitable for an operator visualization (not photoreal games).

### Week 7 — Scripting and controlled motion

| Episode | Title |
|---|---|
| EP09 | World and Material Properties |
| EP10 | C# in Godot and VS Code |
| EP11 | Keyboard and Object Translation |
| EP12 | Using VS Code as Code Editor |

**Checkpoint**

- Set up C# scripting with VS Code.
- Move an object from scripted input or a simple parameter.
- Treat scripted motion as a stand-in for later telemetry-driven twin updates.

### Week 8 — Hierarchy, CSG, and twin assembly

| Episode | Title |
|---|---|
| EP13 | Rotation and Movement Direction |
| EP14 | Constructive Solid Geometry (CSG) |
| EP15 | Child Parent, Compound Object and Pivot Point |

Then assemble a mini digital twin:

1. Import or recreate your Week 3 Blender component in Godot.
2. Parent moving parts under clear pivots.
3. Drive at least one visual state from a script variable (level, angle, color, or label).

**Checkpoint**

- Correct parent/child hierarchy and pivot placement.
- One reusable compound equipment scene.
- Be ready for Quiz 2.

### Week 9 — Quiz 2 (35%) · 7 October 2026 · 09:30–11:30 · CB40610

**Scope:** Godot digital-twin skills from Weeks 5–8.

Expect checks on:

- Godot project structure and scene navigation
- meshes, materials, collisions, and rigid/static bodies
- C# scripting for translation/rotation
- parent/child hierarchy and pivots
- a small digital-twin scene driven by scripted state

---

## Phase C — In-class telemetry and AI (Weeks 10–13)

These weeks are taught in class. Playlist practice is complete; focus on integrating:

- process and sensor telemetry
- MQTT or equivalent structured data exchange
- Godot twin synchronization from live data
- AI-agent supervision on top of deterministic control and safety rules

Bring your Blender assets and Godot twin scene as the visualization layer for this phase. Related curriculum detail: Topics 10–15 in [teaching-schedule.md](teaching-schedule.md).

### Week 14 — Quiz 3 (40%) · 11 November 2026 · 09:30–11:30 · CB40610

**Scope:** Data telemetry and AI integration from Weeks 10–13.

Expect checks on:

- telemetry message structure and data flow
- twin update from external data
- AI integration at the supervisory level
- safe/validated actions versus deterministic control

---

## Weekly study rhythm

1. Watch the assigned episodes.
2. Rebuild the demo yourself (do not only watch).
3. Push one extra step toward the course project.
4. Keep a short practice log: date, episodes completed, file names, blockers.

## Deliverables to keep ready

| After | Keep these files ready |
|---|---|
| Week 3 | Resistor, capacitor, and one process-component `.blend` |
| Week 8 | Godot project with a reusable equipment scene and one scripted visual state |
| Week 13 | Twin connected to class telemetry/AI workflow |
| Week 14 | Materials and demo notes for Quiz 3 |

## How practice supports the course project

```text
Blender models (Weeks 1–3)
  -> Godot digital twin scene (Weeks 5–8)
    -> telemetry synchronization (Weeks 10–13)
      -> AI supervisory layer (Weeks 10–13)
```
