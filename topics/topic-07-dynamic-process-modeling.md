# Topic 7: Dynamic Process Modeling in Python

## Theme

Implement a process model that evolves over simulated time and exposes physically meaningful telemetry.

## Learning outcomes

- select an appropriate physical, transfer-function, or state-space model;
- implement numerical time stepping with explicit units;
- separate model state, inputs, disturbances, and outputs;
- verify the model with open-loop experiments.

## Preparation

- Derive or select equations for the team process.
- Review numerical integration and Python array operations.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Modeling choices, assumptions, and simulation time |
| 00:30-01:10 | Guided nonlinear tank model in Python |
| 01:10-01:30 | Logging and reproducible open-loop tests |
| 01:30-02:30 | Implement and test the team process model |
| 02:30-02:50 | Plot step and disturbance responses |
| 02:50-03:00 | Model sanity review |

## Laboratory

Run at least three open-loop experiments: zero input, a step input, and a disturbance. Check conservation laws, steady-state behavior, bounds, and numerical stability. Save inputs and outputs to CSV with timestamps.

## Deliverable

Python simulator source, model equations and assumptions, plus plots from the three verification experiments.

## Evidence and assessment

- Units and parameter sources are documented
- State variables remain physically valid
- Results are repeatable from a fixed configuration
- Step response agrees qualitatively with engineering expectations

## Instructor notes

Do not reward model complexity by itself. A transparent model that supports controlled experiments is better than an elaborate model that students cannot validate.
