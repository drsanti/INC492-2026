# Topic 8: Feedback Control

## Theme

Close the loop around the simulated process using deterministic control.

## Learning outcomes

- implement a discrete PID or suitable state controller;
- account for sample time, saturation, and anti-windup;
- separate manual, automatic, and supervisory modes;
- evaluate transient and steady-state performance.

## Preparation

- Review PID tuning and closed-loop performance measures.
- Bring the verified process model from Topic 7.

## Three-hour plan

| Time | Activity |
|---|---|
| 00:00-00:30 | Digital control structure and operating modes |
| 00:30-01:10 | Guided PID implementation with output limits |
| 01:10-01:30 | Anti-windup and bumpless mode transfer |
| 01:30-02:30 | Controller tuning and experiment execution |
| 02:30-02:50 | Compare performance metrics |
| 02:50-03:00 | Closed-loop review |

## Laboratory

Test setpoint tracking and disturbance rejection. Record rise time, settling time, overshoot, steady-state error, and control effort. Demonstrate actuator saturation and explain how the controller responds.

## Deliverable

Closed-loop simulator, controller configuration, plots, and a short tuning justification.

## Evidence and assessment

- Controller timing is explicit and repeatable
- Outputs respect actuator limits
- Mode transitions do not cause unsafe command jumps
- Performance claims are supported by measurements

## Instructor notes

The AI agent added later must not replace this fast deterministic loop. It will supervise setpoints, operating modes, diagnosis, and tests at a slower rate.
