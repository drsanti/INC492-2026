# Topic 1: Digital Twins, SIL, and HIL

## Theme

Define the course project and distinguish a 3D visualization from an operational digital twin. The class uses simulated hardware, so the implementation is Software-in-the-Loop (SIL) or virtual HIL while preserving a structure that could later connect to physical hardware.

## Learning outcomes

By the end of the class, students can:

- distinguish a simulation, 3D visualization, digital shadow, and digital twin;
- compare Model-in-the-Loop, SIL, virtual HIL, and physical HIL;
- identify the process, controller, communication, twin, and AI-agent layers;
- define a feasible industrial process for the semester project.

## Preparation

- Install Blender, Godot, Python, Git, and a code editor.
- Review feedback-control terminology: process variable, setpoint, manipulated variable, disturbance, and fault.
- Bring one candidate process idea per team.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Course goals, project demonstration, and assessment overview |
| 00:30-01:00 | Digital-twin concepts and SIL/HIL comparison |
| 01:00-01:30 | Map the reference coupled-tank system into architectural layers |
| 01:30-02:30 | Team exercise: define a system boundary and operating scenario |
| 02:30-02:50 | Teams present scope and receive feasibility feedback |
| 02:50-03:00 | Exit check and repository setup |

## Laboratory

Create a one-page project concept containing:

- system purpose and boundary;
- controlled and manipulated variables;
- simulated sensors and actuators;
- normal operating scenario;
- at least two candidate faults;
- proposed role of the AI agent.

The agent must supervise the process rather than directly animate the Godot scene.

## Deliverable

`project-concept.md` with an initial architecture diagram and team responsibilities.

## Evidence and assessment

- Correct classification of the proposed system as SIL or virtual HIL
- Clear physical meaning for every input and output
- Scope can be demonstrated within the semester

## Instructor notes

Approve projects that have visible dynamics and measurable control performance. Coupled tanks, temperature chambers, conveyors, motor control, and mixing systems work well. Avoid projects that are primarily games or static 3D scenes.
