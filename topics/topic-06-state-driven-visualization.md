# Topic 6: State-Driven Visualization

## Theme

Bind engineering variables to 3D behavior without allowing the visualization to become the source of process truth.

## Learning outcomes

- represent equipment with explicit operational states;
- map values to transforms, animation, materials, and labels;
- distinguish command, internal state, and measured feedback;
- implement manual test inputs for later integration.

## Preparation

- Complete the Godot transform and keyboard-control playlist.
- Review state machines and signal conditioning.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:25 | Data-driven scenes and single source of truth |
| 00:25-01:00 | Guided equipment-state component |
| 01:00-01:30 | Map level, flow, speed, valve position, and alarms |
| 01:30-02:30 | Implement project-specific bindings |
| 02:30-02:50 | Boundary-value and invalid-value tests |
| 02:50-03:00 | Demonstration and review |

## Laboratory

Create a local test-data panel or replay script. Drive each visual element through minimum, nominal, maximum, alarm, and invalid values. Keyboard input may change test values, but production visualization must consume the shared state model.

## Deliverable

A Godot scene demonstrating every important equipment and alarm state without the Python simulator.

## Evidence and assessment

- Visual states derive from named engineering values
- Out-of-range data is clamped or marked invalid
- Commands are not treated as measured feedback
- All important states can be tested repeatably

## Instructor notes

This is the end of the 3D foundation. Confirm that every team can visualize the process before moving attention to dynamic modeling and control.
