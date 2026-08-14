# Topic 16: Validation and Final Demonstration

## Theme

Demonstrate the complete digital twin as an engineering experiment and evaluate whether it satisfies its original requirements.

## Learning outcomes

- execute a repeatable end-to-end validation procedure;
- compare manual, automatic, and AI-supervisory operation;
- communicate limitations and evidence clearly;
- identify what must change for future physical HIL deployment.

## Preparation

- Freeze the demonstration version and configuration.
- Submit the scenario order and acceptance criteria before class.
- Prepare a fallback recording only for recovery from technical failure.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:20 | Setup, broker check, and scenario randomization |
| 00:20-02:20 | Team demonstrations and technical questioning |
| 02:20-02:40 | Cross-team comparison of results and architectures |
| 02:40-03:00 | Course reflection and physical-HIL extension discussion |

## Required demonstration

Each team demonstrates:

1. normal closed-loop operation;
2. a setpoint or operating-mode change;
3. synchronized telemetry and 3D state;
4. one sensor, actuator, process, or communication fault;
5. deterministic alarm or residual detection;
6. AI-agent diagnosis and a validated supervisory action;
7. verification of the resulting process state;
8. rejection of one unsafe or invalid command.

## Final deliverables

- complete source repository and reproducible setup instructions;
- system architecture and interface specification;
- process and instrumentation model documentation;
- test scenarios, logs, plots, and metric calculations;
- concise engineering report discussing results and limitations;
- live demonstration.

## Evaluation criteria

Use this rubric to score demonstration quality during the final integration assessment (Quiz 3 / project close-out). Formal course weights remain Quiz 1–3 (25% / 35% / 40%).

| Area | Share of demonstration score |
|---|---:|
| Process model and deterministic control | 20% |
| Digital-twin synchronization and engineering UI | 20% |
| Communication, safety, and fault handling | 20% |
| AI-agent tools, diagnosis, and guarded actions | 20% |
| Experimental evidence, reproducibility, and presentation | 20% |

## Instructor notes

Evaluate what the evidence demonstrates, not only whether the presentation runs smoothly. Conclude by asking teams which simulated interfaces could remain unchanged when replacing the process model with a real controller and physical I/O.
