# Topic 5: Godot Scenes and Engineering UI

## Theme

Assemble the digital-twin environment and a compact operator interface.

## Learning outcomes

- use scenes, nodes, signals, and reusable components;
- configure cameras, lighting, and environment settings;
- display process values with units and status quality;
- design an interface for observation rather than decoration.

## Preparation

- Complete the relevant Godot scene-creation playlist sections.
- Bring the signal dictionary from Topic 2.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Scene composition and responsibility boundaries |
| 00:30-01:00 | Guided main scene, camera controls, and equipment instances |
| 01:00-01:30 | Engineering UI: values, units, alarms, trends, and modes |
| 01:30-02:30 | Team implementation |
| 02:30-02:50 | Usability review using normal and alarm states |
| 02:50-03:00 | Checkpoint demonstration |

## Laboratory

Build the main process scene and operator display. At minimum, show process values, setpoints, actuator outputs, connection quality, current mode, and alarm state. Use consistent colors and never rely on color alone to communicate a fault.

## Deliverable

An interactive Godot scene populated with the project equipment and placeholder engineering data.

## Evidence and assessment

- Values include units and sensible precision
- Layout remains readable at the target resolution
- Equipment instances are reusable
- Mode, connection, and alarm states are visible

## Instructor notes

The UI should resemble a focused supervisory display, not a marketing dashboard. Require students to justify which information the operator needs for diagnosis and action.
